using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InteractiveInstructions : MonoBehaviour {

	string path = Application.persistentDataPath;
	string textToWrite = "No more instructions wanted";
	InstructionsState instructionState;
	public GameObject objectConfirmInstructions;

	// Use this for initialization
	void Start () {
		if (LoadEncodedFile() == textToWrite)
		{
			StopInstructions();
		}
		else
		{
			Debug.Log("No File Founded");
			instructionState = InstructionsState.confirmIfInstructionsNeeded;
			InstructionExecution();
		}
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
				break;
			case InstructionsState.teamColor:
				break;
			case InstructionsState.personalMoney:
				break;
			case InstructionsState.menu:
				break;
			case InstructionsState.time:
				break;
			case InstructionsState.targetButton:
				break;
			case InstructionsState.Compas:
				break;
			case InstructionsState.Chest:
				break;
			case InstructionsState.FollowBlackArrow:
				break;
			case InstructionsState.end:
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
		Destroy(gameObject);
	}
}

enum InstructionsState
{
	confirmIfInstructionsNeeded,
	teamMoney,
	teamColor,
	personalMoney,
	menu,
	time,
	targetButton,
	Compas,
	Chest,
	FollowBlackArrow,
	end
}
