using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Friend : Person
{
	private int mDetectionRadius = 120;

	private void Start()
	{
		GetComponent<NetworkProximityChecker>().visRange = mDetectionRadius;
	}
}