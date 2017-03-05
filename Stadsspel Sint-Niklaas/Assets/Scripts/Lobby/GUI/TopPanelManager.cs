using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TopPanelManager : MonoBehaviour
{
	[SerializeField]
	private Button m_BackBtn;
	[SerializeField]
	private Text m_NameTxt;

	public void SetName(string text)
	{
		m_NameTxt.text = text;
	}

	public void EnableDisableButton(bool newState, UnityAction action = null)
	{
		m_BackBtn.gameObject.SetActive(newState);
		if(newState) {
			m_BackBtn.onClick.AddListener(action);
		} else {
			m_BackBtn.onClick.RemoveAllListeners();
		}
	}
}
