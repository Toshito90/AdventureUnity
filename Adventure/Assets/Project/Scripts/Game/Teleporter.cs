using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
	public Teleporter exitTeleporter;
	public float exitOffset = 2.0f;

	public Camera gCamera;
	public Camera gCameraDesactivate;

	void OnTriggerEnter(Collider otherCollider)
	{
		if (otherCollider.GetComponent<Player>() != null)
		{
			if (exitTeleporter != null)
			{
				Player player = otherCollider.GetComponent<Player>();
				player.Teleport( exitTeleporter.transform.position + exitTeleporter.transform.forward * exitOffset );

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
}
