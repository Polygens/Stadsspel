using Stadsspel.Districts;
using UnityEngine;

public class MainSquareBtn : MonoBehaviour
{
	private MainSquareArrow m_Arrow;
	private bool IsOn = true;

	GameObject m_headDistrictObject;

	/// <summary>
	/// Initialises the class.
	/// </summary>
	private void Start()
	{
		GameObject district = GameManager.s_Singleton.DistrictManager.GetDistrictByName(CurrentGame.Instance.GetMainSquare());
		HeadDistrict hd = district.GetComponent<HeadDistrict>();
		Treasure t = hd.transform.GetComponentInChildren<Treasure>();
		m_headDistrictObject = t.gameObject;
	}

	// Use this for initialization

	/// <summary>
	/// Event for the show home button. Toggles the visibility state of the home arrow.
	/// </summary>
	public void ToggleMainSquareArrow()
	{
		if(m_Arrow == null) {
			m_Arrow = GameManager.s_Singleton.Player.gameObject.GetComponentInChildren<MainSquareArrow>();
		}
		IsOn = !IsOn;
		m_Arrow.gameObject.SetActive(IsOn);
	}
}
