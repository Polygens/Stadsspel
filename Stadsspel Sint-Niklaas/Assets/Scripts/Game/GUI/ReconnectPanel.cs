using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReconnectPanel : MonoBehaviour {

	CurrentGame CG;

	// Use this for initialization
	void Start ()
	{
		CG = CurrentGame.Instance;
		CG.ReconnectPanel = transform.Find("ReconnectingPanel").gameObject;
		CG.ReconnectPanel.GetComponentInChildren<Button>().onClick.AddListener(CG.StopReconnect);
	}

}
