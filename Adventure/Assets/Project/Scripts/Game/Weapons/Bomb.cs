﻿//#define FriendlyBombDamage

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	//public GameObject player;

	public float duration = 5f;
	public float radius = 3f;
	public float explosionDuration = 0.25f;
	public GameObject explosionModel;

	private float explosionTimer;
	private bool exploded;

    // Start is called before the first frame update
    void Start()
    {
		explosionTimer = duration;

		explosionModel.transform.localScale = Vector3.one * radius;

		explosionModel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
		explosionTimer -= Time.deltaTime;
		if( explosionTimer <= 0f && exploded == false )
		{
			exploded = true;

			Collider[] hitobjects = Physics.OverlapSphere(transform.position, radius);
			foreach( Collider collider in hitobjects)
			{
				if( collider.GetComponent<Enemy>() != null)
				{
					collider.GetComponent<Enemy>().Hit();
				}

				#if FriendlyBombDamage
					if (collider.GetComponent<Player>() != null )
					{
						Vector3 playerPosition = collider.GetComponent<Player>().transform.position;
						collider.GetComponent<Player>().Hit((playerPosition - transform.position).normalized);
					}
				#endif
			}

			StartCoroutine(Explode());
			
		}
    }

	private IEnumerator Explode()
	{
		explosionModel.SetActive(true);

		yield return new WaitForSeconds(explosionDuration);

		explosionModel.SetActive(false);

		Destroy(gameObject);
	}
}
