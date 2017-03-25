using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
	[SerializeField]
	private Transform m_ListPanel;
	[SerializeField]
	private Sprite m_SwitchUpArrow;
	[SerializeField]
	private Sprite m_SwitchDownArrow;

	private bool m_PanelNeeded = false;
	private float m_LerpTimer = 0;
	private float m_LerpDuration = 0.2f;
	private float m_ListPanelStartY = 0;

	/// <summary>
	/// Initialises the class.
	/// </summary>
	private void Start()
	{
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Gets called every frame.
	/// </summary>
	private void Update()
	{
		LerpPanel();

		if (GameManager.s_Singleton.Player.NumberOfButtonsInlistPanel == 0) {
			m_PanelNeeded = false;
			GetComponent<Image>().sprite = m_SwitchUpArrow;
		}
	}

	/// <summary>
	/// Gets called when the GameObject becomes active.
	/// </summary>
	private void OnEnable()
	{
		m_PanelNeeded = false;
		GetComponent<Image>().sprite = m_SwitchUpArrow;
		m_LerpTimer = 0;
	}

	/// <summary>
	/// Event for pressing the more actions button. Shows the additional panel and updates the button's sprites.
	/// </summary>
	public void ButtonListSwitch()
	{
		if(GameManager.s_Singleton.Player.NumberOfButtonsInlistPanel != 0) {
			m_ListPanelStartY = m_ListPanel.transform.localPosition.y;
			m_PanelNeeded = !m_PanelNeeded;
			m_LerpTimer = 0;

			if(m_PanelNeeded) { // SpriteSwap
				GetComponent<Image>().sprite = m_SwitchDownArrow;
			}
			else {
				GetComponent<Image>().sprite = m_SwitchUpArrow;
			}
		}
	}

	/// <summary>
	/// Animates the more actions panel.
	/// </summary>
	private void LerpPanel()
	{
		if(m_LerpTimer >= 0 && m_LerpTimer < m_LerpDuration) {
			m_LerpTimer += Time.deltaTime;
		}

		if(m_PanelNeeded) {
			m_ListPanel.localPosition = new Vector2(m_ListPanel.localPosition.x, Mathf.Lerp(m_ListPanelStartY, 65 + 55 * (GameManager.s_Singleton.Player.NumberOfButtonsInlistPanel - 1), m_LerpTimer / m_LerpDuration));
		}
		else { // !panelNeeded
			m_ListPanel.localPosition = new Vector2(m_ListPanel.localPosition.x, Mathf.Lerp(m_ListPanelStartY, -70 - 55 * (GameManager.s_Singleton.Player.NumberOfButtonsInlistPanel - 1), m_LerpTimer / m_LerpDuration));
		}
	}
}
