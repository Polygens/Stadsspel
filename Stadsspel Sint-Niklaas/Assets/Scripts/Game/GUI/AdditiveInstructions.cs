using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stadsspel.Districts;
using Stadsspel.Elements;

public class AdditiveInstructions : MonoBehaviour {

	public void SpawnInformationOfObject(GameObject go)
	{
		//GameObject spawnInfoObj = Resources.Load("InfoObj") as GameObject;
		if (go.GetComponent<Bank>() != null) {

		}
		else if (go.GetComponent<TradingPost>() != null)
		{

		}
		else if (go.GetComponent<GrandMarket>() != null)
		{

		}
		else if (go.GetComponent<Treasure>() != null)
		{

		}
		else if (go.GetComponent<CapturePoint>() != null)
		{

		}
	}
}
