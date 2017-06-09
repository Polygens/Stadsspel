using Stadsspel.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Stadsspel
{
	public class Room : MonoBehaviour
	{
		[SerializeField] private Text _mRoomNameTxt;
		[SerializeField] private Text _mRoomSlotsTxt;
		[SerializeField] private Text _mPasswordTxt;

		private string m_Password;
		private string ID;
		private bool hasPassword;

		/// <summary>
		/// Initialises the room UI elements with the passed parameters.
		/// </summary>
		public void InitializeRoom(string roomName, string id, int amountPlayers, int maxPlayers, bool hasPassword)
		{
			_mRoomNameTxt.text = roomName;
			_mRoomSlotsTxt.text = amountPlayers + "/" + maxPlayers;
			this.ID = id;
			this.hasPassword = hasPassword;
		}

		/// <summary>
		/// Initialises the room UI elements with the passed parameters.
		/// </summary>
		public void InitializeRoom(string roomName, string ID)
		{
			this.ID = ID;
			_mRoomNameTxt.text = roomName;
			_mRoomSlotsTxt.text = "0/0";
		}

		/// <summary>
		/// Joins the clicked room and handles UI when no password is required. When it is required the pasword popup is shown.
		/// </summary>
		public void ClickJoinRoom()
		{
			if (hasPassword)
				NetworkManager.Singleton.PasswordLoginManager.EnableDisableMenu(true, this);
			else
			{
				var serverGame = Rest.GetGameById(ID);
				var parsedGame = JsonUtility.FromJson<CurrentGame.Game>(serverGame);
				CurrentGame.Instance.gameDetail = parsedGame;
				
				var resource = Rest.RegisterPlayer(CurrentGame.Instance.LocalPlayer.clientID, CurrentGame.Instance.LocalPlayer.name, "", ID);
				CurrentGame.Instance.ClientToken = resource;
				CurrentGame.Instance.GameId = ID;
				CurrentGame.Instance.PasswordUsed = "";
				CurrentGame.Instance.Connect();
				//StartCoroutine(CurrentGame.GetInstance().Ws.Connect("ws://localhost:8090/user", ID, SystemInfo.deviceUniqueIdentifier));//todo switch to heroku server

				NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(true);
				NetworkManager.Singleton.LobbyManager.EnableDisableMenu(false);
				NetworkManager.Singleton.RoomManager.EnableDisableMenu(true);
			}
				
		}

		/// <summary>
		/// Checks if the password given matches the password of the room and returns result.
		/// </summary>
		public bool JoinProtectedRoom(string password)
		{
			//todo allow player name change
			try
			{
				string serverGame = Rest.GetGameById(ID);
				CurrentGame.Game parsed = JsonUtility.FromJson<CurrentGame.Game>(serverGame);
				CurrentGame.Instance.gameDetail = parsed;

				string resource = Rest.RegisterPlayer(CurrentGame.Instance.LocalPlayer.clientID, CurrentGame.Instance.LocalPlayer.name, password, ID);
				CurrentGame.Instance.ClientToken = resource;
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

