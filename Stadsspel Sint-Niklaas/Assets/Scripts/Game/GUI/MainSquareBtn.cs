using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSquareBtn : MonoBehaviour
{
    private GameObject arrow;
	
    // Use this for initialization
	void Start ()
    {
        arrow = GameObject.FindGameObjectWithTag("MainSquareArrowPivot"); 
	}
	
    public void ToggleMainSquareArrow(bool turnOn)
    {
        arrow.SetActive(turnOn);
    }
}
