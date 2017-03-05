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
}
