using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameDurationDropdown : MonoBehaviour
{

	private TimeSpan[] m_Durations = new TimeSpan[] {
		new TimeSpan(0, 30, 0),
		new TimeSpan(1, 0, 0),
		new TimeSpan(1, 30, 0),
		new TimeSpan(2, 0, 0),
		new TimeSpan(2, 30, 0)
	};
	private Dropdown m_DropdownDro;
	private List<String> m_DropDownOptions = new List<String>();
	// Use this for initialization
	void Start()
	{
		GenerateDropdownOptions();
		m_DropdownDro = GetComponent<Dropdown>();
		m_DropdownDro.ClearOptions();

		m_DropdownDro.AddOptions(m_DropDownOptions);

	}

	private void GenerateDropdownOptions()
	{
		for(int i = 0; i < m_Durations.Length; i++) {
			string option = "";
			if(m_Durations[i].Hours > 0) {
				if(m_Durations[i].Hours > 1) {
					option += m_Durations[i].Hours + " uren ";
				} else {
					option += m_Durations[i].Hours + " uur ";
				}
			}
			if(m_Durations[i].Minutes > 0) {
				option += m_Durations[i].Minutes + " minuten ";
			}
			if(m_Durations[i].Seconds > 0) {
				option += m_Durations[i].Seconds + " seconden ";
			}
			m_DropDownOptions.Add(option);
		}
	}

	public void UpdatedSelectedDuration(int selected)
	{
		GameManager.s_Singleton.UpdateGameDuration((float)m_Durations[selected].TotalSeconds);
	}
}
