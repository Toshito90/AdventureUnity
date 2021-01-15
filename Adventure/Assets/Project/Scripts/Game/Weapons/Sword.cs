using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
	public Vector3 defaultAngle;
	public Vector3 attackAngle;

	public float swingingSpeed = 8f;
	public float cooldownSpeed = 2f;
	public float attackDuration = 0.35f;

	public float cooldownDuration = 0.6f;

	private Quaternion targetRotation;
	private float cooldownTimer;
	private bool isAttacking;
	private bool justAttacked;

    // Start is called before the first frame update
    void Start()
    {
		targetRotation = Quaternion.Euler(defaultAngle);
    }

    // Update is called once per frame
    void Update()
    {
		if (isAttacking)
		{

			transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * swingingSpeed);
		}
		else
		{
			transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * (isAttacking ? swingingSpeed : cooldownSpeed));
		}

		cooldownTimer -= Time.deltaTime;
    }

	public void Attack()
	{
		if(cooldownTimer > 0f || isAttacking )
		{
			return;
		}

		targetRotation = Quaternion.Euler(attackAngle);

		cooldownTimer = cooldownDuration;

		StartCoroutine(CooldownWait());
	}

	public bool IsAttacking
	{
		get{
			return isAttacking;
		}
	}

	public bool JustAttacked
	{
		get
		{
			return justAttacked;
		}
	}

	private IEnumerator CooldownWait()
	{
		isAttacking = true;
		justAttacked = true;

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		justAttacked = false;

		yield return new WaitForSeconds(attackDuration);

		isAttacking = false;

		targetRotation = Quaternion.Euler(defaultAngle);
	}
}
