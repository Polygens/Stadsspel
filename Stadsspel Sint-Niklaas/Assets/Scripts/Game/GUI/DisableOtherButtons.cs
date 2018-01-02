using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOtherButtons : MonoBehaviour {

	public List<GameObject> buttons = new List<GameObject>();

  private void OnEnable() {
		foreach (GameObject button in buttons)
		{
			button.SetActive(false);
		}
  }
	
}
