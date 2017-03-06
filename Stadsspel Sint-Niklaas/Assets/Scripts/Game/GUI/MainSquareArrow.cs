using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSquareArrow : MonoBehaviour
{
    HeadDistrict headDistrict;

	void Start ()
    {
        headDistrict = FindObjectOfType<HeadDistrict>();
    }
	

	void Update ()
    {
        //transform.LookAt(headDistrict.transform);
	}   
}
