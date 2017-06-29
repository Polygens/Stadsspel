// Just add this script to your camera. It doesn't need any configuration.

using UnityEngine;
using UnityEngine.UI;

public class TouchCamera : MonoBehaviour
{
	private enum OrientationStates
	{
		FreeCam,
		AlignNorth,
		Compass
	}

	private Vector2?[] m_OldTouchPositions = {
		null,
		null
	};

	[SerializeField]
	private BoxCollider2D m_CameraBounds;
	[SerializeField]
	private Vector2 m_Margin, m_Smoothing;

	private Vector2 m_OldTouchVector;
	private float m_OldTouchDistance;
	private Vector3 m_DefaultCameraPosition;
	private Transform m_PlayerTrans;
	private OrientationStates m_OrientationState;
	private Camera m_Camera;
	private Vector3 m_Min, m_Max;

	private Button btnPos, btnRot;

	void Start()
	{
		m_DefaultCameraPosition = transform.localPosition;
		m_PlayerTrans = gameObject.transform.parent;
		Input.location.Start();
		Input.compass.enabled = true;
		m_Camera = GetComponent<Camera>();
		m_Min = m_CameraBounds.bounds.min;
		m_Max = m_CameraBounds.bounds.max;

		btnPos = GameObject.Find("BtnPosition").GetComponent<Button>();
		btnPos.onClick.AddListener(ResetCameraPosition);

		btnRot = GameObject.Find("BtnRotation").GetComponent<Button>();
		btnRot.onClick.AddListener(ResetCameraRotation);
	}

	void Update()
	{
		if(Input.touchCount == 0) {
			m_OldTouchPositions[0] = null;
			m_OldTouchPositions[1] = null;
		}
		else if(Input.touchCount == 1) {
			if(m_OldTouchPositions[0] == null || m_OldTouchPositions[1] != null) {
				m_OldTouchPositions[0] = Input.GetTouch(0).position;
				m_OldTouchPositions[1] = null;
			}
			else {
				Vector2 newTouchPosition = Input.GetTouch(0).position;
				Vector3 positionOfCamera = transform.position;

				positionOfCamera += transform.TransformDirection((Vector3)((m_OldTouchPositions[0] - newTouchPosition) * m_Camera.orthographicSize / m_Camera.pixelHeight * 2f));

				var x = positionOfCamera.x;
				var y = positionOfCamera.y;

				float cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);
				x = Mathf.Clamp(x, m_Min.x + cameraHalfWidth, m_Max.x - cameraHalfWidth);
				y = Mathf.Clamp(y, m_Min.y + GetComponent<Camera>().orthographicSize, m_Max.y - GetComponent<Camera>().orthographicSize);

				transform.position = new Vector3(x, y, transform.position.z);

				m_OldTouchPositions[0] = newTouchPosition;
			}
		}
		else {
			if(m_OldTouchPositions[1] == null) {
				m_OldTouchPositions[0] = Input.GetTouch(0).position;
				m_OldTouchPositions[1] = Input.GetTouch(1).position;
				m_OldTouchVector = (Vector2)(m_OldTouchPositions[0] - m_OldTouchPositions[1]);
				m_OldTouchDistance = m_OldTouchVector.magnitude;
			}
			else {
				gameObject.transform.parent = null;
				m_OrientationState = OrientationStates.FreeCam;
				//Vector2 screen = new Vector2(mCamera.pixelWidth, mCamera.pixelHeight);

				Vector2[] newTouchPositions = {
					Input.GetTouch(0).position,
					Input.GetTouch(1).position
				};
				Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
				float newTouchDistance = newTouchVector.magnitude;

				//transform.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] + oldTouchPositions[1] - screen) * mCamera.orthographicSize / screen.y));
				transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(Mathf.Clamp((m_OldTouchVector.y * newTouchVector.x - m_OldTouchVector.x * newTouchVector.y) / m_OldTouchDistance / newTouchDistance, -1f, 1f)) / 0.0174532924f));
				m_Camera.orthographicSize *= m_OldTouchDistance / newTouchDistance;
				m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize, 50, 400);

				//transform.position -= transform.TransformDirection((newTouchPositions[0] + newTouchPositions[1] - screen) * mCamera.orthographicSize / screen.y);

				m_OldTouchPositions[0] = newTouchPositions[0];
				m_OldTouchPositions[1] = newTouchPositions[1];
				m_OldTouchVector = newTouchVector;
				m_OldTouchDistance = newTouchDistance;
			}
		}
		if(m_OrientationState == OrientationStates.Compass) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -Input.compass.trueHeading), Time.deltaTime * 2);
		}
	}

	public void ResetCameraPosition()
	{
		gameObject.transform.parent = m_PlayerTrans;
		transform.localPosition = m_DefaultCameraPosition;
	}

	public void ResetCameraRotation()
	{
		switch(m_OrientationState) {
			case OrientationStates.AlignNorth:
				m_OrientationState = OrientationStates.Compass;
				break;
			case OrientationStates.Compass:
				m_OrientationState = OrientationStates.AlignNorth;
				AlignNorth();
				break;
			case OrientationStates.FreeCam:
				m_OrientationState = OrientationStates.AlignNorth;
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
