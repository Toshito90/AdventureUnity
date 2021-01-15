// Define for Game Camera rules
#define CameraRotate360
#define PlayerMovementFollowBack
#define CameraZoomEnable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
	[Header("Camera Properties")]
	public float distanceAway;                     //how far the camera is from the player.

	public float currentDistanceThreshold;

	public float minDistance = 1;                //min camera distance
	public float maxDistance = 2;                //max camera distance

	public float distanceUp = -2;                    //how high the camera is above the player
	public float smoothPreference = 2.0f;
	private float smooth;                    //how smooth the camera moves into place
	public float rotateAround = 70f;            //the angle at which you will rotate the camera (on an axis)

	[Header("Player to follow")]
	public GameObject target;                    //the target the camera follows

	[Header("Layer(s) to include")]
	public LayerMask camOcclusion;                //the layers that will be affected by collision

	[Header("Map coordinate script")]
	//    public worldVectorMap wvm;
	RaycastHit hit;
	float cameraHeight = 55f;
	float cameraPan = 0f;
	float camRotateSpeed = 180f;
	Vector3 camPosition;
	Vector3 camMask;
	Vector3 followMask;

	private float horizontalAxis;
	private float verticalAxis;

	private float mouseX;
	private float mouseY;

	// Use this for initialization
	void Start()
	{
		//the statement below automatically positions the camera behind the target.
		rotateAround = target.transform.eulerAngles.y - 45f;

		cameraHeight = target.transform.position.y + 55f;

		currentDistanceThreshold = maxDistance;

		smooth = smoothPreference;
	}

	private void Update()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			ZoomCamera(1f);
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			ZoomCamera(-1f);
		}

		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");
	}

	void LateUpdate()
	{

		horizontalAxis = Input.GetAxis("Horizontal");
		verticalAxis = Input.GetAxis("Vertical");

		//Offset of the targets transform (Since the pivot point is usually at the feet).
		Vector3 targetOffset = new Vector3(target.transform.position.x, (target.transform.position.y + 2f), target.transform.position.z);
		Quaternion rotation = Quaternion.Euler(cameraHeight, rotateAround, cameraPan);
		Vector3 vectorMask = Vector3.one;
		Vector3 rotateVector = rotation * vectorMask;
		//this determines where both the camera and it's mask will be.
		//the camMask is for forcing the camera to push away from walls.
		camPosition = targetOffset + Vector3.up * distanceUp - rotateVector * distanceAway;
		camMask = targetOffset + Vector3.up * distanceUp - rotateVector * distanceAway;

		OccludeRay(ref targetOffset);
		smoothCamMethod();

		transform.LookAt(target.transform);

		#region wrap the cam orbit rotation
		if (rotateAround > 360)
		{
			rotateAround = 0f;
		}
		else if (rotateAround < 0f)
		{
			rotateAround = (rotateAround + 360f);
		}
		#endregion

		rotateAround += horizontalAxis + mouseX * camRotateSpeed * Time.deltaTime;
		distanceAway = Mathf.Clamp(distanceAway += verticalAxis, minDistance, currentDistanceThreshold);

	}
	void smoothCamMethod()
	{

		smooth = smoothPreference;
		transform.position = Vector3.Lerp(transform.position, camPosition,  smooth);
	}

	void OccludeRay(ref Vector3 targetFollow)
	{
		#region prevent wall clipping
		//declare a new raycast hit.
		RaycastHit wallHit = new RaycastHit();
		//linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
		if (Physics.Linecast(targetFollow, camMask, out wallHit, camOcclusion))
		{
			//the smooth is increased so you detect geometry collisions faster.
			smooth = 10f;
			//the x and z coordinates are pushed away from the wall by hit.normal.
			//the y coordinate stays the same.
			camPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.5f);
		}
		#endregion
	}

	public void ZoomCamera(float newZoomValue)
	{
		if(distanceUp < 5.0f && newZoomValue > 0 || distanceUp > 0.0f && newZoomValue < 0)
		{
			distanceUp += newZoomValue;
		}

		if (distanceAway > minDistance && newZoomValue < 0 || distanceAway < maxDistance && newZoomValue > 0)
		{
			distanceAway += newZoomValue;
			currentDistanceThreshold = distanceAway;
		}
	}
}
