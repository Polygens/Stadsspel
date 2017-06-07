using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stadsspel.Networking
{
	public class CreateJoinRoomManager : MonoBehaviour
	{
		[SerializeField]
		private InputField m_RoomNameInp;
		[SerializeField]
		private InputField m_RoomPasswordInp;
		[SerializeField]
		private Dropdown m_RoomGameDurationDro;
		[SerializeField]
		private Slider m_RoomAmountOfPlayersSli;
		[SerializeField]
		private Text m_AmountOfPlayersTxt;


		public void Awake()
		{
			GameObject network = new GameObject("Network");
			network.AddComponent(typeof(WebsocketImpl));
			network.AddComponent(typeof(CurrentGame));
			DontDestroyOnLoad(network);
		}
		/// <summary>
		/// Generic function for handling switching between the different menus.
		/// </summary>
		public void EnableDisableMenu(bool newState)
		{
			gameObject.SetActive(newState);
			if (newState)
			{
				NetworkManager.Singleton.TopPanelManager.EnableDisableButton(false);
				NetworkManager.Singleton.TopPanelManager.SetName("");
			}
		}

		public void UpdateAmountOfPlayersUI(float numPlayers)
		{
			m_AmountOfPlayersTxt.text = "AANTAL SPELERS: " + numPlayers;
		}

		public void MakeRoom()
		{
			CurrentGame.Instance.HostingLoginToken = Rest.DeviceLogin(CurrentGame.Instance.LocalPlayer.clientID);
			CurrentGame.Instance.HostedGameId = Rest.NewGame(new GameResource(CurrentGame.Instance.HostingLoginToken, m_RoomNameInp.text, 2, 6,m_RoomPasswordInp.text));
		}

		public void ShowLobby()
		{
			NetworkManager.Singleton.LobbyManager.EnableDisableMenu(true);
			EnableDisableMenu(false);
		}
	}
}