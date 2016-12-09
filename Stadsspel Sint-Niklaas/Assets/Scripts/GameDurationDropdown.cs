using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using Prototype.NetworkLobby;

public class GameDurationDropdown : MonoBehaviour
{

	private TimeSpan[] mDurations = new TimeSpan[] { new TimeSpan(0, 30, 0), new TimeSpan(1, 0, 0), new TimeSpan(1, 30, 0), new TimeSpan(2, 0, 0), new TimeSpan(2, 30, 0) };
	private Dropdown mDropdown;
	private List<String> mDropDownOptions = new List<String>();
	// Use this for initialization
	void Start()
	{
		GenerateDropdownOptions();
		mDropdown = GetComponent<Dropdown>();
		mDropdown.ClearOptions();

		mDropdown.AddOptions(mDropDownOptions);

	}

	private void GenerateDropdownOptions()
	{
		for (int i = 0; i < mDurations.Length; i++) {
			string option = "";
			if (mDurations[i].Hours > 0) {
				if (mDurations[i].Hours > 1) {
					option += mDurations[i].Hours + " uren ";
				}
				else {
					option += mDurations[i].Hours + " uur ";
				}
			}
			if (mDurations[i].Minutes > 0) {
				option += mDurations[i].Minutes + " minuten ";
			}
			if (mDurations[i].Seconds > 0) {
				option += mDurations[i].Seconds + " seconden ";
			}
			mDropDownOptions.Add(option);
		}
	}

	public void UpdatedSelectedDuration(int selected)
	{
		LobbyManager.mGameManager.UpdateGameDuration((float)mDurations[selected].TotalSeconds);
	}
}
