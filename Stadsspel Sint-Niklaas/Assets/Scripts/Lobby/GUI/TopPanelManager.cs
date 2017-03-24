using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TopPanelManager : MonoBehaviour
{
	[SerializeField]
	private Button m_BackBtn;
	[SerializeField]
	private Text m_NameTxt;

	/// <summary>
	/// Sets the text of the name of the room.
	/// </summary>
	public void SetName(string text)
	{
		m_NameTxt.text = text;
	}

	/// <summary>
	/// Generic function for handling switching between the different menus.
	/// </summary>
	public void EnableDisableButton(bool newState, UnityAction action = null)
	{
		m_BackBtn.gameObject.SetActive(newState);
		if(newState) {
			m_BackBtn.onClick.AddListener(action);
		}
		else {
			m_BackBtn.onClick.RemoveAllListeners();
		}
	}
}
