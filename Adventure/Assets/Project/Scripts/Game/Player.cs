// Define Rules for Movement for the player
// Player Movement
#define AlwaysRunStraightForward
//#define RotateAndRun

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("Visuals")]
	public GameObject model;
	public Animator playerAnimator;
	public float rotatingSpeed = 8f;

	[Header("Movement")]
	public float jumpingVelocity = 6f;
	public float movingVelocity = 2.5f;
	public float sprintMovingVelocity = 5f;
	// public float mountSpeedVelocity = 5f; for future purposes
	public float knockbackForce;

	[Header("Equipments")]
	public Sword sword;
	public Bow bow;
	public GameObject bombPrefab;
	public int bombAmount = 3;
	public int arrowAmount = 15;
	public int orbsAmount = 0;
	public int health = 10;

	public float throwingSpeed = 200;

	private bool canJump = false;
	private Rigidbody playerRigidBody;

	private Quaternion targetModelRotation;

	private float knockbackTimer;
	private bool justTeleported;
	private Vector3 originalPlayerAnimationsPos;
	private bool isMoving;

	private Dungeon currentDungeon;

	#if (RotateAndRun)
		const float rotationRight = 90f;
		const float rotationLeft = 270f;
		const float rotationForward = 0f;
		const float rotationBack = 180f;
	#endif

	#if (AlwaysRunStraightForward)
		const float playerRotatingSpeed = 500f;
		const float moveBackWardSpeed = 1.5f;
	#endif

	public bool JustTeleported
	{
		get
		{
			bool returnvalue = justTeleported;
			JustTeleported = false;
			return returnvalue;
		}

		set
		{

		}
	}

	public Dungeon CurrentDungeon
	{
		get
		{
			return currentDungeon;
		}
	}

	public bool IsMoving
	{
		get
		{
			return isMoving;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		playerRigidBody = GetComponent<Rigidbody>();
		targetModelRotation = Quaternion.Euler(0, 0, 0);
		sword.gameObject.SetActive(true);
		bow.gameObject.SetActive(false);

		originalPlayerAnimationsPos = playerAnimator.transform.localPosition;
	}

    // Update is called once per frame
    void Update()
    {
		VerifyJumpingAbility();
		
		if (knockbackTimer > 0)
		{
			knockbackTimer -= Time.deltaTime;
		}
		else {
			ListenInput();
		}

		playerAnimator.SetBool("OnGround", canJump);
	}

	void LateUpdate()
	{
		// Keep the player animator's game object in place
		playerAnimator.transform.localPosition = originalPlayerAnimationsPos;
	}

	void ListenInput()
	{
		playerRigidBody.velocity = new Vector3(
				0,
				playerRigidBody.velocity.y,
				0);

		bool isPlayerMoving = false;
		bool isSprinting = false;
		float animationSpeed = 1.0f;
		float playerMovementSpeed = 1f;

		if(Input.GetKey("left shift") && canJump)
		{
			isSprinting = true;
		}


		if ( Input.GetKey("d")) // right
		{

			#if (RotateAndRun)
				playerRigidBody.velocity = new Vector3(

					movingVelocity,
					playerRigidBody.velocity.y,
					playerRigidBody.velocity.z );
				
				isPlayerMoving = true;
				
				targetModelRotation = Quaternion.Euler(0, rotationRight, 0);

			#elif (AlwaysRunStraightForward)

				targetModelRotation = Quaternion.Euler(
					0,
					model.transform.localEulerAngles.y + playerRotatingSpeed * Time.deltaTime,
					0
				);

			#endif
		}
		if (Input.GetKey("a")) // left
		{

			#if (RotateAndRun)

				playerRigidBody.velocity = new Vector3(
					-movingVelocity,
					playerRigidBody.velocity.y,
					playerRigidBody.velocity.z);
			
				isPlayerMoving = true;
			
				targetModelRotation = Quaternion.Euler(0, rotationLeft, 0);

			#elif (AlwaysRunStraightForward)

				targetModelRotation = Quaternion.Euler(
					0,
					model.transform.localEulerAngles.y - playerRotatingSpeed * Time.deltaTime,
					0
				);

			#endif
		}
		if (Input.GetKey("w")) // forward
		{

			#if (RotateAndRun)
			
				playerRigidBody.velocity = new Vector3(
					playerRigidBody.velocity.x,
					playerRigidBody.velocity.y,
					movingVelocity);
				
				isPlayerMoving = true;
				
				targetModelRotation = Quaternion.Euler(0, rotationForward, 0);
			
			#elif (AlwaysRunStraightForward)

				isPlayerMoving = true;

				if (isPlayerMoving)
				{
					if( isSprinting)
					{
						animationSpeed = 1.0f;
						playerMovementSpeed = sprintMovingVelocity;
					}
					else
					{
						animationSpeed = 0.5f;
						playerMovementSpeed = movingVelocity;
					}
				}

				playerRigidBody.velocity = new Vector3(
					model.transform.forward.x * playerMovementSpeed,
					playerRigidBody.velocity.y,
					model.transform.forward.z * playerMovementSpeed
				);

				playerAnimator.SetFloat("Forward", animationSpeed);

			#endif
		}
		if (Input.GetKey("s")) // down
		{
			#if (RotateAndRun)
						
				playerRigidBody.velocity = new Vector3(
					playerRigidBody.velocity.x,
					playerRigidBody.velocity.y,
					movingVelocity);
				
				isPlayerMoving = true;
				
				targetModelRotation = Quaternion.Euler(0, rotationback, 0);
						
			#elif (AlwaysRunStraightForward)
			
				playerRigidBody.velocity = new Vector3(
					-model.transform.forward.x * moveBackWardSpeed,
					playerRigidBody.velocity.y,
					-model.transform.forward.z * moveBackWardSpeed
					);

				isPlayerMoving = true;

				playerAnimator.SetFloat("Forward", isPlayerMoving ? 0.5f : 0.0f);

			#endif
		}
		if ( Input.GetKeyDown("space") && canJump )
		{
			float jumpingSpeed;

			if (isSprinting)
			{
				jumpingSpeed = jumpingVelocity + 2.0f;
			}
			else
			{
				jumpingSpeed = jumpingVelocity;
			}

			playerRigidBody.velocity = new Vector3(
				playerRigidBody.velocity.x,
				jumpingSpeed,
				playerRigidBody.velocity.z
				);

			canJump = false;
		}
		
		// Check for equipment interaction.
		if ( Input.GetKeyDown("z"))
		{
			sword.gameObject.SetActive(true);
			bow.gameObject.SetActive(false);
			sword.Attack();
		}

		if( Input.GetKeyDown("x"))
		{
			if (arrowAmount > 0) {
				bow.gameObject.SetActive(true);
				sword.gameObject.SetActive(false);
				bow.Attack();
				arrowAmount--;
			};
		}

		if( Input.GetKeyDown("c"))
		{
			ThrowBomb();
		}

		#if (RotateAndRun)
			if( isSprinting )
			{
				animationSpeed = 1.0f;
			}
			else
			{
				animationSpeed = 0.5f;
			}	

			playerAnimator.SetFloat("Forward", isPlayerMoving ? animationSpeed : 0.0f);

		#elif (AlwaysRunStraightForward)
			if( !isPlayerMoving) playerAnimator.SetFloat("Forward", 0.0f);
		#endif

		isMoving = isPlayerMoving;

		// When changing directions
		model.transform.rotation = Quaternion.Lerp(model.transform.rotation, targetModelRotation, Time.deltaTime * rotatingSpeed);
	}

	private void VerifyJumpingAbility()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.31f))
		{
			canJump = true;
		}
	}

	private void ThrowBomb()
	{
		if (bombAmount > 0)
		{
			GameObject bombObject = Instantiate(bombPrefab);
			bombObject.transform.position = transform.position + model.transform.forward;

			Vector3 throwingDirection = (model.transform.forward + Vector3.up).normalized;

			bombObject.GetComponent<Rigidbody>().AddForce(throwingDirection * throwingSpeed);

			bombAmount--;
		}
	}

	private void OnTriggerEnter(Collider otherCollider)
	{
		if( otherCollider.GetComponent<EnemyBullet>() != null)
		{
			Hit((transform.position - otherCollider.transform.position ).normalized);
			Destroy( otherCollider.gameObject );
		}
		if( otherCollider.GetComponent<Treasure>() != null)
		{
			orbsAmount++;
			Destroy(otherCollider.gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<Enemy>() != null)
		{
			Hit((transform.position - collision.transform.position).normalized);
		}
	}

	private void OnTriggerStay(Collider otherCollider)
	{
		if( otherCollider.GetComponent<Dungeon>() != null)
		{
			currentDungeon = otherCollider.GetComponent<Dungeon>();
		}
	}

	private void OnTriggerExit(Collider otherCollider)
	{
		if (otherCollider.GetComponent<Dungeon>() != null)
		{
			Dungeon exitDungeon = otherCollider.GetComponent<Dungeon>();
			if( exitDungeon = currentDungeon)
			{
				currentDungeon = null;
			}	
		}
	}

	public void Hit( Vector3 direction )
	{
		Vector3 knockbackDirection = ( direction + Vector3.up ).normalized;

		playerRigidBody.AddForce(knockbackDirection * knockbackForce);
		knockbackTimer = 1f;

		health--;
		if( health <= 0)
		{
			Destroy(gameObject);
		}

	}

	public void Teleport( Vector3 target)
	{
		transform.position = target;
		justTeleported = true;
	}
}