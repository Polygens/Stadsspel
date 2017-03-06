using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon;

namespace Stadsspel.Networking
{
	public class RoomManager : PunBehaviour
	{
		[SerializeField]
		private RectTransform m_LobbyPlayerList;

		[SerializeField]
		private Button m_StartGameBtn;

		public RectTransform LobbyPlayerList {
			get {
				return m_LobbyPlayerList;
			}
		}

		private uint m_PlayersReady = 0;

		private void Start()
		{
			m_StartGameBtn.onClick.AddListener(() => {
				photonView.RPC("StartCountDown", PhotonTargets.AllBufferedViaServer);
			});
		}

		[PunRPC]
		private void StartCountDown()
		{
			NetworkManager.Singleton.CountdownManager.EnableDisableMenu(true);
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
			NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(false);
		}

		void OnLeftRoom()
		{
			NetworkManager.Singleton.KickedManager.EnableDisableMenu(true);
			EnableDisableMenu(false);
			NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
			m_PlayersReady = 0;
		}

		public void CheckIfReadyToStart()
		{
			int playersReady = 0;
			foreach(Transform item in m_LobbyPlayerList) {
				if(item.GetComponent<LobbyPlayer>().IsReady) {
					++playersReady;
				}
			}
			if(playersReady == PhotonNetwork.room.MaxPlayers) {
				m_StartGameBtn.gameObject.SetActive(true);
			} else {
				m_StartGameBtn.gameObject.SetActive(false);
			}
		}
	}
}