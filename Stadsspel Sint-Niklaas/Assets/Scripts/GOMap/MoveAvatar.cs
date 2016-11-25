using UnityEngine;
using System.Collections;
using GoMap;

public class MoveAvatar : MonoBehaviour
{

	public LocationManager locationManager;
	public Transform avatarDirection;


	// Use this for initialization
	void Start()
	{
		locationManager.onOriginSet += OnOriginSet;
		locationManager.onLocationChanged += OnLocationChanged;
	}

	private void Update()
	{
		avatarDirection.transform.rotation = Quaternion.Lerp(avatarDirection.transform.rotation, Quaternion.Euler(0, 0, -Input.compass.trueHeading), Time.deltaTime * 2);
	}

	void OnOriginSet(Coordinates currentLocation)
	{

		//Position
		Vector3 currentPosition = currentLocation.convertCoordinateToVector();
		currentPosition.z = transform.position.z;

		transform.position = currentPosition;

	}

	void OnLocationChanged(Coordinates currentLocation)
	{

		Vector3 lastPosition = transform.position;

		//Position
		Vector3 currentPosition = currentLocation.convertCoordinateToVector();
		currentPosition.z = transform.position.z;

		if (lastPosition == Vector3.zero) {
			lastPosition = currentPosition;
		}

		//		transform.position = currentPosition;
		//		rotateAvatar (lastPosition);

		moveAvatar(lastPosition, currentPosition);

	}

	void moveAvatar(Vector3 lastPosition, Vector3 currentPosition)
	{

		StartCoroutine(move(lastPosition, currentPosition, 0.5f));
	}

	private IEnumerator move(Vector3 lastPosition, Vector3 currentPosition, float time)
	{
		float elapsedTime = 0;

		while (elapsedTime < time) {
			transform.position = Vector3.Lerp(lastPosition, currentPosition, (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}
}
