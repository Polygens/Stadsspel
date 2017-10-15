using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stadsspel.Districts;
using Stadsspel.Elements;
using UnityEngine.UI;

public class AdditiveInstructions : MonoBehaviour {

	static string bankName = "Bank";
	static string tradingPostName = "Handelspost";
	static string grandMarketName = "Grote Markt";
	static string treasureName = "Schatkist";
	static string capturePointName = "Overneembaar Plein";
	static string bankExpl = "Bij de bank kan je jouw geld veilig zetten. Tegenstanders zullen er niet meer aan kunnen.";
	static string tradingPostExpl = "Dit is een handelspost waar je goederen kan kopen, zowel legaal als illegaal. Illegaal heeft meer risico's maar levert meer op.";
	static string grandMarketExpl = "De grote markt is waar je al jouw goederen verkoopt die je op zak hebt.";
	static string treasureExpl = "Uit een schatkist kan je belastingen innen, hoe meer pleinen een team bezit, hoe meer geld in hun schatkist komt.";
	static string capturePointExpl = "Dit is een plein dat je kan overnemen, als jouw team in bezit is van dit plein, zal je team meer geld krijgen in de schatkist";
	static string ownCapturePointExpl = "Dit plein is in het bezit van jouw team, verdedig het zodat je team meer geld blijft innen.";
	private float rayDist = 100f;
	private GameObject lastInfoBox = null;

	void Update()
	{
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; ++i)
			{
				Vector2 test = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
				if (Input.GetTouch(i).phase == TouchPhase.Ended)
				{
					LayerMask layerMask = ~(1 << LayerMask.NameToLayer("CameraBounds")); // ignore collisions with CameraBounds
					test = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
					RaycastHit2D hit = Physics2D.Raycast(test, (Input.GetTouch(i).position), rayDist ,layerMask);
					if (hit.collider)
					{
						SpawnInformationOfObject(hit.collider.gameObject);
					}
				}
			}
		}	
	}

	public void SpawnInformationOfObject(GameObject go)
	{
		Debug.Log("touched object: " + go.name);
		GameObject spawnInfoObj = Resources.Load("CanvasInfoBox") as GameObject;
		GameObject tempInfoBox = null;
		spawnInfoObj.transform.eulerAngles = new Vector3(0, 0, transform.rotation.z);
		if (go.GetComponent<Bank>() != null) {
			tempInfoBox = Instantiate(spawnInfoObj, go.transform.position, spawnInfoObj.transform.rotation) as GameObject;
			tempInfoBox.transform.Find("InfoBox").transform.Find("Uitleg").GetComponent<Text>().text = bankExpl;
			tempInfoBox.transform.Find("InfoBox").transform.Find("Title").GetComponent<Text>().text = bankName;
			tempInfoBox.name = "BankInfoBox";
			ResetListener(tempInfoBox);
		}
		else if (go.GetComponent<TradingPost>() != null)
		{
			tempInfoBox = Instantiate(spawnInfoObj, go.transform.position, spawnInfoObj.transform.rotation) as GameObject;
			tempInfoBox.transform.Find("InfoBox").transform.Find("Uitleg").GetComponent<Text>().text = tradingPostExpl;
			tempInfoBox.transform.Find("InfoBox").transform.Find("Title").GetComponent<Text>().text = tradingPostName;
			tempInfoBox.name = "TradingPostInfoBox";
			ResetListener(tempInfoBox);
		}
		else if (go.GetComponent<GrandMarket>() != null)
		{
			tempInfoBox = Instantiate(spawnInfoObj, go.transform.position, spawnInfoObj.transform.rotation) as GameObject;
			tempInfoBox.transform.Find("InfoBox").transform.Find("Uitleg").GetComponent<Text>().text = grandMarketExpl;
			tempInfoBox.transform.Find("InfoBox").transform.Find("Title").GetComponent<Text>().text = grandMarketName;
			tempInfoBox.name = "GrandMarketInfoBox";
			ResetListener(tempInfoBox);
		}
		else if (go.GetComponent<Treasure>() != null)
		{
			tempInfoBox = Instantiate(spawnInfoObj, go.transform.position, spawnInfoObj.transform.rotation) as GameObject;
			tempInfoBox.transform.Find("InfoBox").transform.Find("Uitleg").GetComponent<Text>().text = treasureExpl;
			tempInfoBox.transform.Find("InfoBox").transform.Find("Title").GetComponent<Text>().text = treasureName;
			tempInfoBox.name = "TreasureInfoBox";
			ResetListener(tempInfoBox);
		}
		else if (go.GetComponent<CapturePoint>() != null)
		{
			tempInfoBox = Instantiate(spawnInfoObj, go.transform.position, spawnInfoObj.transform.rotation) as GameObject;
			tempInfoBox.transform.Find("InfoBox").transform.Find("Uitleg").GetComponent<Text>().text = capturePointExpl;
			tempInfoBox.transform.Find("InfoBox").transform.Find("Title").GetComponent<Text>().text = capturePointName;
			tempInfoBox.name = "CapturePointInfoBox";
			ResetListener(tempInfoBox);
		}
		if (tempInfoBox != null)
		{
			tempInfoBox.transform.SetAsLastSibling();
			tempInfoBox.transform.rotation = transform.rotation;
			if (go.GetComponent<GrandMarket>() != null)
			{
				tempInfoBox.transform.position = new Vector3(0, 0, spawnInfoObj.transform.position.z);
			}
			else
			{
				tempInfoBox.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, spawnInfoObj.transform.position.z);
			}
			lastInfoBox = tempInfoBox;
		}

	}

	public void ResetListener(GameObject tempInfoBox)
	{
		if (lastInfoBox != null)
		{
			if (lastInfoBox.name == tempInfoBox.name)
			{
				Destroy(tempInfoBox);
			}
				Destroy(lastInfoBox);
				lastInfoBox = null;
		}
	}

	public void DestroyParent()
	{
		Destroy(transform.parent.parent.gameObject);
	}
}
