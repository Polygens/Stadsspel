using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
	public Transform listPanel;
	public Sprite switchUpArrow;
	public Sprite switchDownArrow;

	private bool panelNeeded = false;
	private float lerpTimer = 0;
	private float lerpDuration = 0.2f;
	private float listPanelStartY = 0;

	private void Start()
	{
		gameObject.SetActive(false);
	}

	private void Update()
	{
		LerpPanel();

		if (GameManager.s_Singleton.Player.mNumberOfButtonsInlistPanel == 0) {
			panelNeeded = false;
			GetComponent<Image>().sprite = switchUpArrow;
		}
	}

	private void OnEnable()
	{
		panelNeeded = false;
		GetComponent<Image>().sprite = switchUpArrow;
		lerpTimer = 0;
	}

	public void ButtonListSwitch()
	{
		if (GameManager.s_Singleton.Player.mNumberOfButtonsInlistPanel != 0) {
			listPanelStartY = listPanel.transform.localPosition.y;
			panelNeeded = !panelNeeded;
			lerpTimer = 0;

			if (panelNeeded) // SpriteSwap
			{
				GetComponent<Image>().sprite = switchDownArrow;
			}
			else {
				GetComponent<Image>().sprite = switchUpArrow;
			}
		}
	}

	private void LerpPanel()
	{
		if (lerpTimer >= 0 && lerpTimer < lerpDuration) {
			lerpTimer += Time.deltaTime;
		}

		if (panelNeeded) {
			listPanel.localPosition = new Vector2(listPanel.localPosition.x, Mathf.Lerp(listPanelStartY, 65 + 55 * (GameManager.s_Singleton.Player.mNumberOfButtonsInlistPanel - 1), lerpTimer / lerpDuration));
		}
		else // !panelNeeded
		{
			listPanel.localPosition = new Vector2(listPanel.localPosition.x, Mathf.Lerp(listPanelStartY, -70 - 55 * (GameManager.s_Singleton.Player.mNumberOfButtonsInlistPanel - 1), lerpTimer / lerpDuration));
		}
	}
}
