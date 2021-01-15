using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasingEnemy : Enemy
{
	public NavMeshAgent agent;
	public Animator enemyAnimator;
	public float attackRadius = 20f;
	public float attackUpdateDuration = 1f;
	public float movementRadius = 15f;
	public float movementUpdateDuration = 3f;

	private Vector3 initialPosition;
	private Vector3 originalEnemyAnimatorPos;
	private Vector3 previousPos;
	private float movementUpdateTimer;
	private float attackUpdateTimer;

	private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
		initialPosition = transform.position;
		movementUpdateTimer = movementUpdateDuration;

		previousPos = transform.position;
		originalEnemyAnimatorPos = enemyAnimator.transform.localPosition;
		attackUpdateTimer = attackUpdateDuration;

		MoveAroundStart();
	}

    // Update is called once per frame
    void Update()
    {
		// Walk around if didn't saw the player
		if( !isAttacking)
		{
			movementUpdateTimer -= Time.deltaTime;
			if (movementUpdateTimer <= 0.0f)
			{
				MoveAroundStart();
				movementUpdateTimer = movementUpdateDuration;
			}
		}
		
		// Search for the player
		attackUpdateTimer -= Time.deltaTime;
		if( attackUpdateTimer <= 0.0f)
		{
			attackUpdateTimer = attackUpdateDuration;
			SearchPlayer();
		}

		// Animate the chasing Enemy
		if( agent.velocity.magnitude > 0)
		{
			enemyAnimator.SetFloat("Forward", 0.6f);
		}

		if( Vector3.Distance(transform.position, previousPos) < 0.03f)
		{
			enemyAnimator.SetFloat("Forward", 0.0f);
		}

		previousPos = transform.position;
	}

	void LateUpdate()
	{
		enemyAnimator.transform.localPosition = originalEnemyAnimatorPos;
	}

	private void SearchPlayer()
	{
		isAttacking = false;

		RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRadius, transform.forward);
		foreach( RaycastHit hit in hits)
		{
			if( hit.transform.GetComponent<Player>() != null)
			{
				agent.SetDestination(hit.transform.position);
				isAttacking = true;
				break;
			}
		}
	}

	private void MoveAroundStart()
	{
		agent.SetDestination(initialPosition + new Vector3(
			Random.Range(-movementRadius, movementRadius),
			0,
			Random.Range(-movementRadius, movementRadius)
			));
	}
}
