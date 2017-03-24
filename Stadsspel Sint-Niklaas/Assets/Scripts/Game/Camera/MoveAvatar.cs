using System.Collections;
using UnityEngine;

public class MoveAvatar : MonoBehaviour
{
	private Transform m_AvatarDirection;

	// Use this for initialization
	void Start()
	{
		m_AvatarDirection = transform.GetChild(1);
		GameManager.s_Singleton.LocationManager.onOriginSet += OnOriginSet;
		GameManager.s_Singleton.LocationManager.onLocationChanged += OnLocationChanged;
	}

	private void Update()
	{
		m_AvatarDirection.transform.rotation = Quaternion.Lerp(m_AvatarDirection.transform.rotation, Quaternion.Euler(0, 0, -Input.compass.trueHeading), Time.deltaTime * 2);
	}

	private void OnOriginSet(Coordinates currentLocation)
	{

		//Position
		Vector3 currentPosition = currentLocation.convertCoordinateToVector();
		currentPosition.z = transform.position.z;

		transform.position = currentPosition;

	}

	private void OnLocationChanged(Coordinates currentLocation)
	{
		Vector3 lastPosition = transform.position;

		//Position
		Vector3 currentPosition = currentLocation.convertCoordinateToVector();
		currentPosition.z = transform.position.z;

		if(lastPosition == Vector3.zero) {
			lastPosition = currentPosition;
		}

		//		transform.position = currentPosition;
		//		rotateAvatar (lastPosition);

		moveAvatar(lastPosition, currentPosition);
		GameManager.s_Singleton.DistrictManager.CheckDisctrictState();
	}

	private void moveAvatar(Vector3 lastPosition, Vector3 currentPosition)
	{

		StartCoroutine(move(lastPosition, currentPosition, 0.5f));
	}

	private IEnumerator move(Vector3 lastPosition, Vector3 currentPosition, float time)
	{
		float elapsedTime = 0;

		while(elapsedTime < time) {
			transform.position = Vector3.Lerp(lastPosition, currentPosition, (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}
}
