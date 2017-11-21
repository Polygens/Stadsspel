using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithCamera : MonoBehaviour {

	Camera maincamera;
	RectTransform localCanvas;
	float maxOrthographicSize;
	float lastOrthographicSize;
	float zRotation;

	// Use this for initialization
	void Start () {
		maincamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		maxOrthographicSize = GameObject.Find("Main Camera").GetComponent<TouchCamera>().GetOrthographicStartSize();
		localCanvas = GetComponent<RectTransform>();
		Debug.Log(maxOrthographicSize + " MAX SIZE ORTHO");
	}
	
	// Update is called once per frame
	void Update () {
		if (lastOrthographicSize != maincamera.orthographicSize || zRotation != maincamera.transform.rotation.z)
		{
			lastOrthographicSize = maincamera.orthographicSize;
			float scale = (lastOrthographicSize / maxOrthographicSize)/3;
			localCanvas.localScale = new Vector3(scale, scale, scale);
			localCanvas.transform.rotation = maincamera.transform.rotation;
		}

	}
}
