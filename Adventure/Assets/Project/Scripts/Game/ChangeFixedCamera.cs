using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFixedCamera : MonoBehaviour
{
	public Camera gCamera;
	public Camera gCameraDesactivate;

	void OnTriggerEnter(Collider otherCollider)
	{
		if (otherCollider.GetComponent<Player>() != null)
		{
			if (!gCamera.enabled)
			{
				if (gCamera)
				{
					gCamera.enabled = true;
				}

				if (gCameraDesactivate)
				{
					gCameraDesactivate.enabled = false;
				}
			}
		}
	}
}
