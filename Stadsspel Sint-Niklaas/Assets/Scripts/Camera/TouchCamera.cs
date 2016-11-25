// Just add this script to your camera. It doesn't need any configuration.

using UnityEngine;

public class TouchCamera : MonoBehaviour
{
	private enum OrientationStates
	{
		FreeCam,
		AlignNorth,
		Compass
	}
	private Vector2?[] mOldTouchPositions = {
		null,
		null
	};
	private Vector2 mOldTouchVector;
	private float mOldTouchDistance;
	private Vector3 mDefaultCameraPosition;
	private Transform mPlayerTrans;
	private OrientationStates mOrientationState;
	private Camera mCamera;

	void Start()
	{
		mDefaultCameraPosition = transform.localPosition;
		mPlayerTrans = gameObject.transform.parent;
		Input.location.Start();
		Input.compass.enabled = true;
		mCamera = GetComponent<Camera>();
	}

	void Update()
	{
		if (Input.touchCount == 0) {
			mOldTouchPositions[0] = null;
			mOldTouchPositions[1] = null;
		}
		else if (Input.touchCount == 1) {
			if (mOldTouchPositions[0] == null || mOldTouchPositions[1] != null) {
				mOldTouchPositions[0] = Input.GetTouch(0).position;
				mOldTouchPositions[1] = null;
			}
			else {
				Vector2 newTouchPosition = Input.GetTouch(0).position;

				transform.position += transform.TransformDirection((Vector3)((mOldTouchPositions[0] - newTouchPosition) * mCamera.orthographicSize / mCamera.pixelHeight * 2f));

				mOldTouchPositions[0] = newTouchPosition;
			}
		}
		else {
			if (mOldTouchPositions[1] == null) {
				mOldTouchPositions[0] = Input.GetTouch(0).position;
				mOldTouchPositions[1] = Input.GetTouch(1).position;
				mOldTouchVector = (Vector2)(mOldTouchPositions[0] - mOldTouchPositions[1]);
				mOldTouchDistance = mOldTouchVector.magnitude;
			}
			else {
				gameObject.transform.parent = null;
				mOrientationState = OrientationStates.FreeCam;
				//Vector2 screen = new Vector2(mCamera.pixelWidth, mCamera.pixelHeight);

				Vector2[] newTouchPositions = {
					Input.GetTouch(0).position,
					Input.GetTouch(1).position
				};
				Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
				float newTouchDistance = newTouchVector.magnitude;

				//transform.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] + oldTouchPositions[1] - screen) * mCamera.orthographicSize / screen.y));
				transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(Mathf.Clamp((mOldTouchVector.y * newTouchVector.x - mOldTouchVector.x * newTouchVector.y) / mOldTouchDistance / newTouchDistance, -1f, 1f)) / 0.0174532924f));
				mCamera.orthographicSize *= mOldTouchDistance / newTouchDistance;
				mCamera.orthographicSize = Mathf.Clamp(mCamera.orthographicSize, 50, 400);
				//transform.position -= transform.TransformDirection((newTouchPositions[0] + newTouchPositions[1] - screen) * mCamera.orthographicSize / screen.y);

				mOldTouchPositions[0] = newTouchPositions[0];
				mOldTouchPositions[1] = newTouchPositions[1];
				mOldTouchVector = newTouchVector;
				mOldTouchDistance = newTouchDistance;
			}
		}
		if (mOrientationState == OrientationStates.Compass) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -Input.compass.trueHeading), Time.deltaTime * 2);
		}
	}

	public void ResetCameraPosition()
	{
		gameObject.transform.parent = mPlayerTrans;
		transform.localPosition = mDefaultCameraPosition;
	}

	public void ResetCameraRotation()
	{
		switch (mOrientationState) {
			case OrientationStates.AlignNorth:
				mOrientationState = OrientationStates.Compass;
				break;
			case OrientationStates.Compass:
				mOrientationState = OrientationStates.AlignNorth;
				AlignNorth();
				break;
			case OrientationStates.FreeCam:
				mOrientationState = OrientationStates.AlignNorth;
				AlignNorth();
				break;
			default:
				break;
		}
	}

	private void AlignNorth()
	{
		transform.rotation = Quaternion.Euler(0, 0, 0);
	}
}
