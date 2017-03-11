using UnityEngine;
using Stadsspel.Districts;

public class MainSquareArrow : MonoBehaviour
{
	HeadDistrict m_HeadDistrict;

	private void Start()
	{
		m_HeadDistrict = FindObjectOfType<HeadDistrict>();
	}


	private void Update()
	{
		//transform.LookAt(headDistrict.transform);
	}
}
