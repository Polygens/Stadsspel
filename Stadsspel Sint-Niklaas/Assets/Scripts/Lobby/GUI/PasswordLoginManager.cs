using UnityEngine;
using UnityEngine.UI;

public class PasswordLoginManager : MonoBehaviour
{
	[SerializeField]
	private InputField m_PasswordTxt;

	private Stadsspel.Room m_Room;

	/// <summary>
	/// Generic function for handling switching between the different menus.
	/// </summary>
	public void EnableDisableMenu(bool newState, Stadsspel.Room room)
	{
		gameObject.SetActive(newState);
		m_Room = room;
	}

	/// <summary>
	/// Retrieves the password that was given in the inputfield and checks if correct. If not user is asked again for password and a warning is shown. When correct menu is hidden.
	/// </summary>
	public void ClickJoinProtectedRoom()
	{
		if(m_Room.JoinProtectedRoom(m_PasswordTxt.text)) {
			EnableDisableMenu(false, m_Room);
		}
		else {
			m_PasswordTxt.text = "";
			m_PasswordTxt.placeholder.GetComponent<Text>().text = "Paswoord is verkeerd!";
		}
	}
}
