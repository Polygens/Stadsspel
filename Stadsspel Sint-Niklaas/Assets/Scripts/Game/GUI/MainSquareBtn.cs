using UnityEngine;

public class MainSquareBtn : MonoBehaviour
{
	private MainSquareArrow m_Arrow;
	private bool IsOn = true;

	// Use this for initialization

	/// <summary>
	/// Event for the show home button. Toggles the visibility state of the home arrow.
	/// </summary>
	public void ToggleMainSquareArrow()
	{
		if(!m_Arrow) {
			m_Arrow = GameManager.s_Singleton.Player.gameObject.GetComponentInChildren<MainSquareArrow>();
		}
		IsOn = !IsOn;
		m_Arrow.gameObject.SetActive(IsOn);
	}
}
