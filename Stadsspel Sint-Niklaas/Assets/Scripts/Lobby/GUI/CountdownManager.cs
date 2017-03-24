using UnityEngine;
using UnityEngine.UI;

public class CountdownManager : MonoBehaviour
{
	[SerializeField]
	private Text m_CountDownTxt;

	/// <summary>
	/// Generic function for handling switching between the different menus.
	/// </summary>
	public void EnableDisableMenu(bool newState)
	{
		gameObject.SetActive(newState);
	}

	/// <summary>
	/// Sets the text of the countdown popup.
	/// </summary>
	public void SetText(string text)
	{
		m_CountDownTxt.text = text;
	}
}
