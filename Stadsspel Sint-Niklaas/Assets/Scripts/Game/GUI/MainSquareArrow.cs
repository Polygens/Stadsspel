using UnityEngine;
using Stadsspel.Districts;

public class MainSquareArrow : MonoBehaviour
{
	HeadDistrict headDistrict;

	void Start()
	{
		headDistrict = FindObjectOfType<HeadDistrict>();
	}


	void Update()
	{
		//transform.LookAt(headDistrict.transform);
	}
}
