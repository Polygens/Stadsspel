using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour {

	string path;
	string textToWrite;
	public Text playerNameTextObj;

	private void Awake()
	{
		path = Application.persistentDataPath;
	}

	public void SetPlayerName(string name)
	{
		if (name != "" && name != null)
		{
			textToWrite = name;
			playerNameTextObj.text = name;
		}
	}

	public void WriteToFile()
	{
		File.WriteAllText(path + "/StadsspelSpelerNaam.txt", textToWrite);
	}
}
