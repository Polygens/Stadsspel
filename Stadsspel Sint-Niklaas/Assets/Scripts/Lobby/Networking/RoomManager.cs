using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Stadsspel.Networking
{
	public class RoomManager : PunBehaviour
	{
		[SerializeField]
		private RectTransform m_LobbyPlayerList;

		[SerializeField]
		private Button m_StartGameBtn;

		[SerializeField]
		private int m_CountdownDuration = 5;

		public const string RoomPasswordProp = "password";
		public const string RoomGameDurationProp = "gameDuration";

		public RectTransform LobbyPlayerList {
			get {
				return m_LobbyPlayerList;
			}
		}

		private void Start()
		{
			m_StartGameBtn.onClick.AddListener(() => {
				StartCoroutine(ServerCountdownCoroutine(m_CountdownDuration));
			});
		}

		[PunRPC]
		private void UpdateCountDown(byte time)
		{
			NetworkManager.Singleton.CountdownManager.SetText("Het spel start in...\n" + time);
		}

		[PunRPC]
		private void StartCountDown(bool state)
		{
			NetworkManager.Singleton.CountdownManager.EnableDisableMenu(state);
		}

		public IEnumerator ServerCountdownCoroutine(int time)
		{ 
			photonView.RPC("StartCountDown", PhotonTargets.AllViaServer, true);

			float remainingTime = time; 
			int floorTime = Mathf.FloorToInt(remainingTime); 

			while(floorTime > 0) { 
				remainingTime -= Time.deltaTime; 
				int newFloorTime = Mathf.FloorToInt(remainingTime); 

				if(newFloorTime != floorTime) {//to avoid flooding the nepunrtwork of message, we only send a notice to client when the number of plain seconds change. 
					floorTime = newFloorTime; 

					photonView.RPC("UpdateCountDown", PhotonTargets.AllViaServer, (byte)floorTime);
				} 
				yield return null; 
			} 
			photonView.RPC("StartCountDown", PhotonTargets.AllViaServer, false);
			SceneManager.LoadScene("Game");	
		}

		public void InitializeRoom(string roomName, string roomPassword, int gameDuration, byte amountPlayers)
		{
			EnableDisableMenu(true);

			string[] lobbyOptions = new string[2];
			lobbyOptions[0] = RoomPasswordProp;
			lobbyOptions[1] = RoomGameDurationProp;

			ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
			ht.Add(RoomPasswordProp, roomPassword);
			ht.Add(RoomGameDurationProp, gameDuration);

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

		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
			NetworkManager.Singleton.TopPanelManager.SetName(PhotonNetwork.room.Name);
			PhotonNetwork.Instantiate(NetworkManager.Singleton.LobbyPlayerPrefabName, Vector3.zero, Quaternion.identity, 0);
			NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(false);
		}

		public override void OnLeftRoom()
		{
			base.OnLeftRoom();
			NetworkManager.Singleton.KickedManager.EnableDisableMenu(true);
			EnableDisableMenu(false);
			NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
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

			#if (UNITY_EDITOR)
			m_StartGameBtn.gameObject.SetActive(true);
			m_StartGameBtn.transform.GetChild(0).GetComponent<Text>().text = "OVERRIDE START! Unity Editor Only";
			#endif
		}

		public void DisableStartButton()
		{
			m_StartGameBtn.gameObject.SetActive(false);
		}
	}
}