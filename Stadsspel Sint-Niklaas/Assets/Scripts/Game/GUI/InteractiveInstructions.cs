using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Stadsspel.Districts;

public class InteractiveInstructions : MonoBehaviour {

	string path;
	string textToWrite = "No more instructions wanted";
	InstructionsState instructionState;
	public GameObject objectConfirmInstructions;
	public GameObject teamMoneyInstruction;
	public GameObject personalMoneyInstruction;
	public GameObject menuInstruction;
	public GameObject timeInstruction;
	public GameObject targetButtonInstruction;
	public GameObject compasInstruction;
	public GameObject chestInstruction;
	public GameObject actionBar;
	public GameObject blackArrowInstruction;
	public GameObject headDistrict;
	TouchCamera TCamera;
	ServerTeam m_team;

	private void Awake()
	{
		path = Application.persistentDataPath;
	}

	public void StartTutorial(TouchCamera tc, ServerTeam team)
	{
		TCamera = tc;
		TCamera.enabled = false;
		m_team = team;
		instructionState = InstructionsState.confirmIfInstructionsNeeded;
		InstructionExecution();
		//if (LoadEncodedFile() == textToWrite)
		//{
		//	StopInstructions();
		//}
		//else
		//{
		//	Debug.Log("No File Founded");
		//	instructionState = InstructionsState.confirmIfInstructionsNeeded;
		//	InstructionExecution();

		//}
	}

	void WriteToFile()
	{
		File.WriteAllText(path + "/StadsspelInstructionState.txt", textToWrite);
	}

	public string LoadEncodedFile()
	{
		string path = Application.persistentDataPath;
		try
		{
			return File.ReadAllText(path + "/" + "StadsspelInstructionState" + ".txt");
		}
		catch (System.Exception)
		{
			return "";
			throw;
		}
	}

	void InstructionExecution()
	{
		switch (instructionState)
		{
			case InstructionsState.confirmIfInstructionsNeeded:
				objectConfirmInstructions.SetActive(true);
				break;
			case InstructionsState.teamMoney:
				objectConfirmInstructions.SetActive(false);
				teamMoneyInstruction.SetActive(true);
				break;
			case InstructionsState.personalMoney:
				teamMoneyInstruction.SetActive(false);
				personalMoneyInstruction.SetActive(true);
				break;
			case InstructionsState.menu:
				personalMoneyInstruction.SetActive(false);
				menuInstruction.SetActive(true);
				break;
			case InstructionsState.time:
				menuInstruction.SetActive(false);
				timeInstruction.SetActive(true);
				break;
			case InstructionsState.targetButton:
				timeInstruction.SetActive(false);
				targetButtonInstruction.SetActive(true);
				break;
			case InstructionsState.Compas:
				targetButtonInstruction.SetActive(false);
				compasInstruction.SetActive(true);
				break;
			case InstructionsState.Chest:
				compasInstruction.SetActive(false);
				chestInstruction.SetActive(true);
				break;
			case InstructionsState.ActionBar:
				chestInstruction.SetActive(false);
				actionBar.SetActive(true);
				break;
			case InstructionsState.HeadDistrict:
				actionBar.SetActive(false);
				headDistrict.SetActive(true);
				TCamera.enabled = true;
				GameObject tempObj = null;
				foreach (GameObject go in GameObject.FindGameObjectsWithTag("Treasure"))
				{
					if (go.transform.parent.GetComponent<HeadDistrict>() != null)
					{
						if (go.transform.parent.GetComponent<HeadDistrict>().Team == m_team) tempObj = go;
					}
				}
				TCamera.ChangeCameraPosition(tempObj.transform);
				TCamera.enabled = false;
				break;
			case InstructionsState.FollowBlackArrow:
				headDistrict.SetActive(false);
				blackArrowInstruction.SetActive(true);
				TCamera.enabled = true;
				TCamera.ResetCameraPosition();
				TCamera.enabled = false;
				break;
			case InstructionsState.end:
				blackArrowInstruction.SetActive(false);
				WriteToFile();
				StopInstructions();
				break;
			default:
				break;
		}
	}

	public void NextInstruction()
	{
		int currentState = (int)instructionState;
		currentState++;
		instructionState = (InstructionsState)currentState;
		InstructionExecution();
	}

	public void StopInstructions()
	{
		TCamera.enabled = true;
		Destroy(gameObject);
	}
}

enum InstructionsState
{
	confirmIfInstructionsNeeded,
	teamMoney,
	personalMoney,
	menu,
	time,
	targetButton,
	Compas,
	Chest,
	ActionBar,
	HeadDistrict,
	FollowBlackArrow,
	end
}
