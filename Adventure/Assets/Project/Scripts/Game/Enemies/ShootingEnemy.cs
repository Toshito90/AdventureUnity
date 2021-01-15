using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
	public GameObject model;
	public float timeToRotate = 2f;
	public float rotationSpeed = 6f;
	public float valueToRotate = 90.0f;
	public int startingAngle = 0;

	public GameObject bulletPrefab;
	//public float bulletHeightOffset = 0.7f;
	public GameObject bulletSpawnPoint;
	public float timeToShoot = 1f;

	private int targetAngle;
	private float rotationTimer;
	private float shootingTimer;

    // Start is called before the first frame update
    void Start()
    {
		rotationTimer = timeToRotate;
		shootingTimer = timeToShoot;

		targetAngle = startingAngle;
		transform.localRotation = Quaternion.Euler(0, targetAngle, 0);
	}

    // Update is called once per frame
    void Update()
    {
		// Update the enemy's angle
		rotationTimer -= Time.deltaTime;
		if( rotationTimer <= 0f)
		{
			rotationTimer = timeToRotate;

			targetAngle += (int)valueToRotate;
		}

		// Perform the enemy rotation
		//transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, targetAngle, 0), Time.deltaTime * rotationSpeed);
		transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, targetAngle, 0), Time.deltaTime * rotationSpeed);

		// Shoot bullets
		shootingTimer -= Time.deltaTime;
		if( shootingTimer <= 0f)
		{
			shootingTimer = timeToShoot;
			GameObject bulletObject = Instantiate(bulletPrefab);
			bulletObject.transform.SetParent(transform.parent);
			bulletObject.transform.position = bulletSpawnPoint.transform.position;
			bulletObject.transform.forward = model.transform.forward;
		}
    }
}
