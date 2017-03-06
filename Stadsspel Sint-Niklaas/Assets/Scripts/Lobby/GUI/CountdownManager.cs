using UnityEngine;
using UnityEngine.UI;

public class CountdownManager : MonoBehaviour
{
	[SerializeField]
	private Text m_CountDownTxt;

	public void EnableDisableMenu(bool newState)
	{
		gameObject.SetActive(newState);
	}

	public void SetText(string text)
	{
		m_CountDownTxt.text = text;
	}
}
