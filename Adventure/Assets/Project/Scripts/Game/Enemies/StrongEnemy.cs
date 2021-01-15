using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongEnemy : Enemy
{
	public Animator enemyAnimator;
	private Vector3 originalEnemyAnimatorPos;

    // Start is called before the first frame update
    void Start()
    {
		originalEnemyAnimatorPos = enemyAnimator.transform.localPosition;
		enemyAnimator.SetFloat("Forward", 0.3f);
    }

    void LateUpdate()
    {
		enemyAnimator.transform.localPosition = originalEnemyAnimatorPos;
	}

	public override void Hit()
	{
		base.Hit();
	}
}
