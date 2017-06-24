using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDurationDropdown : MonoBehaviour
{
	public static TimeSpan[] m_Durations = new TimeSpan[] {
		#if (UNITY_EDITOR)
		new TimeSpan(0, 1, 0),
		#endif
		new TimeSpan(0, 30, 0),
		new TimeSpan(1, 0, 0),
		new TimeSpan(1, 30, 0),
		new TimeSpan(2, 0, 0),
		new TimeSpan(2, 30, 0)
	};
	private Dropdown m_DropdownDro;
	private List<String> m_DropDownOptions = new List<String>();

	/// <summary>
	/// Initialises the class.
	/// </summary>
	void Start()
	{
		GenerateDropdownOptions();
		m_DropdownDro = GetComponent<Dropdown>();
		m_DropdownDro.ClearOptions();

		m_DropdownDro.AddOptions(m_DropDownOptions);
	}

	/// <summary>
	/// Creates the dropdown options automatically based on an TimeSpan array (m_Durations).
	/// </summary>
	private void GenerateDropdownOptions()
	{
		for(int i = 0; i < m_Durations.Length; i++) {
			string option = "";
			if(m_Durations[i].Hours > 0) {
				if(m_Durations[i].Hours > 1) {
					option += m_Durations[i].Hours + " uren ";
				}
				else {
					option += m_Durations[i].Hours + " uur ";
				}
			}
			if(m_Durations[i].Minutes > 0)
			{
				if (m_Durations[i].Minutes > 1)
				{
					option += m_Durations[i].Minutes + " minuten ";
				} else
				{
					option += m_Durations[i].Minutes + " minuut ";
				}
			}
			if(m_Durations[i].Seconds > 0) {
				option += m_Durations[i].Seconds + " seconden ";
			}
			m_DropDownOptions.Add(option);
		}
	}
}
