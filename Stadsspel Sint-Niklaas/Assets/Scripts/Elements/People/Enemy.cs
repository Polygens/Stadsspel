using UnityEngine;
using UnityEngine.Networking;

public class Enemy : Person
{
	private int mDetectionRadius = 100;

	private new void Start()
	{
		//base.Start();
		Destroy(transform.GetChild(1).gameObject);
		Destroy(transform.GetChild(2).gameObject);
		Destroy(transform.GetChild(3).gameObject);
		Destroy(GetComponent<MoveAvatar>());
		GetComponent<NetworkIdentity>().localPlayerAuthority = false;

		NetworkProximityChecker proximityChecker = gameObject.AddComponent<NetworkProximityChecker>();
		proximityChecker.checkMethod = NetworkProximityChecker.CheckMethod.Physics2D;
		proximityChecker.visRange = mDetectionRadius;
	}
}