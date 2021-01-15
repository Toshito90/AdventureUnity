using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
	[Header("Player to follow")]
	public GameObject target;                    //the target the camera follows

	private Camera myCameraRef;

	// Use this for initialization
	void Start()
	{
		if(this.gameObject.GetComponent<Camera>() != null)
		{
			myCameraRef = this.gameObject.GetComponent<Camera>();
			myCameraRef.enabled = false;
		}
	}

	void Update()
	{
		if (myCameraRef)
		{
			if (myCameraRef.enabled)
			{
				transform.LookAt(target.transform);
			}
		}

	}

	public GameObject TargetInfo {
		get
		{
			if (target)
			{
				return target;
			}
			return null;
		}

		set
		{
			target = value;
		}
	}
}
