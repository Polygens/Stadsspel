using Stadsspel.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Stadsspel
{
	public class Room : MonoBehaviour
	{
		[SerializeField]
		private Text m_RoomNameTxt;
		[SerializeField]
		private Text m_RoomSlotsTxt;
		[SerializeField]
		private Text m_PasswordTxt;

		private string m_Password;

		/// <summary>
		/// Initialises the room UI elements with the passed parameters.
		/// </summary>
		public void InitializeRoom(string roomName, int amountPlayers, int maxPlayers, string password)
		{
			m_RoomNameTxt.text = roomName;
			m_RoomSlotsTxt.text = amountPlayers + "/" + maxPlayers;
			m_Password = password;
			if(m_Password != "") {
				m_PasswordTxt.gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Joins the clicked room and handles UI when no password is required. When it is required the pasword popup is shown.
		/// </summary>
		public void ClickJoinRoom()
		{
			if(m_Password == "") {
				NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(true);
				NetworkManager.Singleton.LobbyManager.EnableDisableMenu(false);
				NetworkManager.Singleton.RoomManager.EnableDisableMenu(true);
				PhotonNetwork.JoinRoom(m_RoomNameTxt.text);
			}
			else {
				NetworkManager.Singleton.PasswordLoginManager.EnableDisableMenu(true, this);
			}
		}

		/// <summary>
		/// Checks if the password given matches the password of the room and returns result.
		/// </summary>
		public bool JoinProtectedRoom(string password)
		{
			if(m_Password == password) {
				NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(true);
				NetworkManager.Singleton.LobbyManager.EnableDisableMenu(false);
				NetworkManager.Singleton.RoomManager.EnableDisableMenu(true);
				PhotonNetwork.JoinRoom(m_RoomNameTxt.text);
				return true;
			}
			else {
				return false;
			}
		}
	}
}

