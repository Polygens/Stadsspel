using UnityEngine;

public class MainSquareBtn : MonoBehaviour
{
	private MainSquareArrow m_Arrow;

	// Use this for initialization

	public void ToggleMainSquareArrow(bool turnOn)
	{
		if(!m_Arrow) {
			m_Arrow = GameManager.s_Singleton.Player.gameObject.GetComponentInChildren<MainSquareArrow>();

		}

		m_Arrow.gameObject.SetActive(turnOn);
	}
}
