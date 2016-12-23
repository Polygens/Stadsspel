using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy : Person
{
	private int mDetectionRadius = 100;

	private void Start()
	{
		GetComponent<NetworkProximityChecker>().visRange = mDetectionRadius;
	}
}