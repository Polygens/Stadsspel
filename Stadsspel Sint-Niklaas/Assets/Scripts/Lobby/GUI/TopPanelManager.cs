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
	public void EnableDisableButton(bool newState, string name, UnityAction action = null)
	{
		m_BackBtn.gameObject.SetActive(newState);
		if (name == "lobby")
		{
			m_BackBtn.GetComponentInChildren<Text>().text = "Terug";
		}
		else if (name == "room")
		{
			m_BackBtn.GetComponentInChildren<Text>().text = "Verlaten";
		}
		if(newState) {
			m_BackBtn.onClick.AddListener(action);
		}
		else {
			m_BackBtn.onClick.RemoveAllListeners();
		}
	}

	public void EnableDisableButton(bool newState, UnityAction action = null)
	{
		m_BackBtn.gameObject.SetActive(newState);
		if (newState)
		{
			m_BackBtn.onClick.AddListener(action);
		}
		else
		{
			m_BackBtn.onClick.RemoveAllListeners();
		}
	}
}
