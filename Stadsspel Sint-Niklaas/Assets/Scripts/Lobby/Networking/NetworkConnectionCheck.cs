using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkConnectionCheck : MonoBehaviour {

	public GameObject GPSPopUp;

	IEnumerator checkInternetConnection(Action<bool> action){
		WWW www = new WWW("http://google.com");
		yield return www;
		if (www.error != null) {
			action (false);
		} else {
			action (true);
		}
	} 

	void Start(){
		StartCoroutine(checkInternetConnection((isConnected)=>{
			if (!isConnected)
			{
				GPSPopUp.SetActive(true);
			}
			else
			{
				LocationService service = Input.location;
				if (!service.isEnabledByUser)
				{
					GPSPopUp.SetActive(true);
				}
			}
		}));
	}
}
