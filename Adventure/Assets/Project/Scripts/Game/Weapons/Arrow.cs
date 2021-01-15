using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	public float velocitySpeed = 10f;
	public float lifetime = 1f;

    // Start is called before the first frame update
    void Start()
    {
		GetComponent<Rigidbody>().velocity = transform.forward * velocitySpeed;
    }

    // Update is called once per frame
    void Update()
    {
		lifetime -= Time.deltaTime;
		if( lifetime <= 0)
		{
			Destroy(gameObject);
		}
    }

	
}
