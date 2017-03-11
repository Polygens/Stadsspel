using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSquareBtn : MonoBehaviour
{
	private GameObject m_Arrow;
	
	// Use this for initialization
	private void Start()
	{
		m_Arrow = GameObject.FindGameObjectWithTag("MainSquareArrowPivot"); 
	}

	public void ToggleMainSquareArrow(bool turnOn)
	{
		m_Arrow.SetActive(turnOn);
	}
}
