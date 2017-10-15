using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stadsspel.Districts;
using Stadsspel.Elements;

public class AdditiveInstructions : MonoBehaviour {

	void Update()
	{
		if (Input.touchCount > 0)
		{

			LayerMask layerMask = ~(1 << LayerMask.NameToLayer("CameraBounds")); // ignore collisions with CameraBounds
			Vector3 pos = Input.touches[0].position;
			RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero,layerMask);
			if (hit)
			{
				Debug.Log("I'm hitting " + hit.collider.name);
				SpawnInformationOfObject(hit.collider.gameObject);
			}
		}
		
	}
	public void SpawnInformationOfObject(GameObject go)
	{
		GameObject spawnInfoObj = Resources.Load("CanvasInfoBox") as GameObject;
		if (go.GetComponent<Bank>() != null) {
			Instantiate(spawnInfoObj, go.transform.position, go.transform.rotation);
		}
		else if (go.GetComponent<TradingPost>() != null)
		{
			Instantiate(spawnInfoObj, go.transform.position, go.transform.rotation);
		}
		else if (go.GetComponent<GrandMarket>() != null)
		{
			Instantiate(spawnInfoObj, go.transform.position, go.transform.rotation);
		}
		else if (go.GetComponent<Treasure>() != null)
		{
			Instantiate(spawnInfoObj, go.transform.position, go.transform.rotation);
		}
		else if (go.GetComponent<CapturePoint>() != null)
		{
			Instantiate(spawnInfoObj, go.transform.position, go.transform.rotation);
		}
	}

}
