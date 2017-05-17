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
		private string ID;

		/// <summary>
		/// Initialises the room UI elements with the passed parameters.
		/// </summary>
		public void InitializeRoom(string roomName, int amountPlayers, int maxPlayers, string password)
		{
			m_RoomNameTxt.text = roomName;
			m_RoomSlotsTxt.text = amountPlayers + "/" + maxPlayers;
			m_Password = password;
			if (m_Password != "")
			{
				m_PasswordTxt.gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Initialises the room UI elements with the passed parameters.
		/// </summary>
		public void InitializeRoom(string roomName, string ID)
		{
			this.ID = ID;
			m_RoomNameTxt.text = roomName;
			m_RoomSlotsTxt.text = "0/0";
			m_Password = "pow";
			if (m_Password != "")
			{
				m_PasswordTxt.gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Joins the clicked room and handles UI when no password is required. When it is required the pasword popup is shown.
		/// </summary>
		public void ClickJoinRoom()
		{
			NetworkManager.Singleton.PasswordLoginManager.EnableDisableMenu(true, this);
		}

		/// <summary>
		/// Checks if the password given matches the password of the room and returns result.
		/// </summary>
		public bool JoinProtectedRoom(string password)
		{
			//todo allow player name change
			try
			{
				ConnectionResource resource = Rest.RegisterPlayer(CurrentGame.Instance.LocalPlayer.clientID, CurrentGame.Instance.LocalPlayer.name, password, ID);
				CurrentGame.Instance.ClientToken = resource.clientToken;
				CurrentGame.Instance.GameId = ID;
				CurrentGame.Instance.PasswordUsed = password;
				CurrentGame.Instance.Connect();
				//StartCoroutine(CurrentGame.GetInstance().Ws.Connect("ws://localhost:8090/user", ID, SystemInfo.deviceUniqueIdentifier));//todo switch to heroku server

				NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(true);
				NetworkManager.Singleton.LobbyManager.EnableDisableMenu(false);
				NetworkManager.Singleton.RoomManager.EnableDisableMenu(true);

				//PhotonNetwork.JoinRoom(m_RoomNameTxt.text);
				return true;
			}
			catch (RestException e)
			{
				Debug.Log(e.Message);
				return false;
			}
		}
	}
}

