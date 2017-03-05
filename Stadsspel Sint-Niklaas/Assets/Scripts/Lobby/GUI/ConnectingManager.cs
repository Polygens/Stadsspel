using UnityEngine;
using UnityEngine.UI;

public class ConnectingManager : MonoBehaviour
{
	[SerializeField]
	private Button m_CancelConnectingBtn;

	public void EnableDisableMenu(bool newState)
	{
		gameObject.SetActive(newState);
	}
}
