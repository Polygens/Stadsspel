using UnityEngine;
using UnityEngine.UI;
using Stadsspel.Networking;

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

		public void InitializeRoom(string roomName, int amountPlayers, int maxPlayers, string password)
		{
			m_RoomNameTxt.text = roomName;
			m_RoomSlotsTxt.text = amountPlayers + "/" + maxPlayers;
			m_Password = password;
			if(m_Password != "") {
				m_PasswordTxt.gameObject.SetActive(true);
			}
		}

		public void ClickJoinRoom()
		{
			if(m_Password == "") {
				NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(true);
				NetworkManager.Singleton.LobbyManager.EnableDisableMenu(false);
				NetworkManager.Singleton.RoomManager.EnableDisableMenu(true);
				PhotonNetwork.JoinRoom(m_RoomNameTxt.text);
			} else {
				NetworkManager.Singleton.PasswordLoginManager.EnableDisableMenu(true, this);
			}
		}

		public bool JoinProtectedRoom(string password)
		{
			if(m_Password == password) {
				NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(true);
				NetworkManager.Singleton.LobbyManager.EnableDisableMenu(false);
				NetworkManager.Singleton.RoomManager.EnableDisableMenu(true);
				PhotonNetwork.JoinRoom(m_RoomNameTxt.text);
				return true;
			} else {
				return false;
			}
		}
	}
}

