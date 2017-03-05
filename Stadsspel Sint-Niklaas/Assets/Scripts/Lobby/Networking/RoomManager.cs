using UnityEngine;
using UnityEngine.Events;

namespace Stadsspel.Networking
{
	public class RoomManager : MonoBehaviour
	{
		[SerializeField]
		private RectTransform m_LobbyPlayerList;

		public RectTransform LobbyPlayerList {
			get {
				return m_LobbyPlayerList;
			}
		}

		public void InitializeRoom(string roomName, string roomPassword, int gameDuration, byte amountPlayers)
		{
			EnableDisableMenu(true);

			string passwordKey = "password";
			string gameDurationKey = "gameDuration";

			string[] lobbyOptions = new string[2];
			lobbyOptions[0] = passwordKey;
			lobbyOptions[1] = gameDurationKey;

			ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
			ht.Add(passwordKey, roomPassword);
			ht.Add(gameDurationKey, gameDuration);

			RoomOptions roomOptions = new RoomOptions() {
				MaxPlayers = amountPlayers,
				IsVisible = true,
				CustomRoomPropertiesForLobby = lobbyOptions,
				CustomRoomProperties = ht
			};

			PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);

		}

		public void EnableDisableMenu(bool newState)
		{
			gameObject.SetActive(newState);
			if(newState) {
				NetworkManager.Singleton.TopPanelManager.EnableDisableButton(true, new UnityAction(() => {
					EnableDisableMenu(false);
					NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
				}));
			} else {
				PhotonNetwork.LeaveRoom();
			}
		}

		void OnJoinedRoom()
		{
			NetworkManager.Singleton.TopPanelManager.SetName(PhotonNetwork.room.Name);
			PhotonNetwork.Instantiate(NetworkManager.Singleton.LobbyPlayerPrefabName, Vector3.zero, Quaternion.identity, 0);
		}

		void OnLeftRoom()
		{
			NetworkManager.Singleton.KickedManager.EnableDisableMenu(true);
			EnableDisableMenu(false);
			NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
		}
	}
}