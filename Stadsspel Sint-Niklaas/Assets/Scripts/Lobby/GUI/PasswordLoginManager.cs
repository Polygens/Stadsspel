using UnityEngine;
using UnityEngine.UI;

public class PasswordLoginManager : MonoBehaviour
{
	[SerializeField]
	private InputField m_PasswordTxt;

	private Stadsspel.Room m_Room;

	public void EnableDisableMenu(bool newState, Stadsspel.Room room)
	{
		gameObject.SetActive(newState);
		m_Room = room;
	}

	public void ClickJoinProtectedRoom()
	{
		if(m_Room.JoinProtectedRoom(m_PasswordTxt.text)) {
			EnableDisableMenu(false, m_Room);
		} else {
			m_PasswordTxt.text = "";
			m_PasswordTxt.placeholder.GetComponent<Text>().text = "Paswoord is verkeerd!";
		}
	}
}
