// Just add this script to your camera. It doesn't need any configuration.

using Stadsspel.Districts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
	private Transform m_PlayerTrans;
	private OrientationStates m_OrientationState;
	private Camera m_Camera;
	private Vector3 m_Min, m_Max;
	private float m_cameraSpeed = 3f;

	private Button btnPos, btnRot, hdPos;
	private float lerpSpeed = 2.5f;

	private float startOrthoGraphicSize;

	Vector3 touchPosWorld;
	GameObject m_headDistrictObject;

	public float GetOrthographicStartSize() {
		return startOrthoGraphicSize;
	}

	void Start()
	{

		m_PlayerTrans = gameObject.transform.parent;
		Input.location.Start();
		Input.compass.enabled = true;
		m_Camera = GetComponent<Camera>();
		startOrthoGraphicSize = m_Camera.orthographicSize;
		m_CameraBounds = GameObject.Find("CameraBoundsGlobal").GetComponent<BoxCollider2D>();
		m_Min = m_CameraBounds.bounds.min;
		m_Max = m_CameraBounds.bounds.max;

		GameObject district = GameManager.s_Singleton.DistrictManager.GetDistrictByName(CurrentGame.Instance.GetMainSquare());
		HeadDistrict hd = district.GetComponent<HeadDistrict>();
		Treasure t = hd.transform.GetComponentInChildren<Treasure>();
		m_headDistrictObject = t.gameObject;

		btnPos = GameObject.Find("BtnPosition").GetComponent<Button>();
		btnPos.onClick.AddListener(ResetCameraPosition);

		hdPos = GameObject.Find("BtnMainSquare").GetComponent<Button>();
		hdPos.onClick.AddListener(SetHdPosition);

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
			if (m_OldTouchPositions[0] == null || m_OldTouchPositions[1] != null) {
				m_OldTouchPositions[0] = Input.GetTouch(0).position;
				m_OldTouchPositions[1] = null;
			}
			else {
				Vector2 newTouchPosition = Input.GetTouch(0).position;
				Vector3 positionOfCamera = transform.position;

				positionOfCamera += transform.TransformDirection((Vector3)((m_OldTouchPositions[0] - newTouchPosition) * (m_Camera.orthographicSize / m_Camera.pixelHeight) * m_cameraSpeed));

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
		if(gameObject.transform.parent == null) gameObject.transform.parent = m_PlayerTrans;
		//transform.localPosition = m_DefaultCameraPosition;
		ChangeCameraPosition(gameObject.transform.parent.position);
	}

	public void SetHdPosition()
	{
		gameObject.transform.parent = null;
		ChangeCameraPosition(m_headDistrictObject.transform.position);
	}

	public void ChangeCameraPosition(Transform destTransform)
	{
		gameObject.AddComponent<LerpCamera>().BeginLerp(transform.position, destTransform.position, lerpSpeed);
	}

	public void ChangeCameraPosition(Vector3 destTransform)
	{
		gameObject.AddComponent<LerpCamera>().BeginLerp(transform.position, destTransform, lerpSpeed);
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
