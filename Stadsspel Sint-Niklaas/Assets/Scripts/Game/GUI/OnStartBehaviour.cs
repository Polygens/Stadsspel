using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class OnStartBehaviour : MonoBehaviour {

	public GameObject playerNameObject;
	public List<GameObject> activateAllOtherObjects = new List<GameObject>();
	public Text playerNameTextObj;
	private string playerName;

	// Use this for initialization
	void Start () {
		if (LoadEncodedName() == "")
		{
			playerNameObject.SetActive(true);
		}
		else
		{
			playerNameTextObj.text = playerName;
			for (int i = 0; i < activateAllOtherObjects.Count; i++)
			{
				activateAllOtherObjects[i].SetActive(true);
			}
		}
		Destroy(gameObject);
	}

	public string LoadEncodedName()
	{
		string path = Application.persistentDataPath;
		try
		{
			string tempString = File.ReadAllText(path + "/" + "StadsspelSpelerNaam" + ".txt");
			playerName = tempString;
			return tempString;
		}
		catch (System.Exception)
		{
			return "";
			throw;
		}
	}
}
