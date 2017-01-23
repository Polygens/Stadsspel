using UnityEngine;
using UnityEngine.Networking;

public class Enemy : MonoBehaviour
{
	private int mDetectionRadius = 100;
	private Person mPerson;

	private void Start()
	{
		mPerson = GetComponent<Person>();
		tag = "Enemy";
		Destroy(transform.GetChild(1).gameObject);
		Destroy(transform.GetChild(2).gameObject);
		Destroy(transform.GetChild(3).gameObject);

		NetworkProximityChecker proximityChecker = gameObject.AddComponent<NetworkProximityChecker>();
		proximityChecker.checkMethod = NetworkProximityChecker.CheckMethod.Physics2D;
		proximityChecker.visRange = mDetectionRadius;
	}
}
