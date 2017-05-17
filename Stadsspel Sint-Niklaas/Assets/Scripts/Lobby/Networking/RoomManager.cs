using Photon;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace Stadsspel.Networking
{
	//public class RoomManager : PunBehaviour
	public class RoomManager : MonoBehaviour
	{
		[SerializeField]
		private RectTransform m_LobbyPlayerList;

		[SerializeField]
		private Button m_StartGameBtn;

		[SerializeField]
		private int m_CountdownDuration = 5;

		public const string RoomPasswordProp = "password";
		public const string RoomGameDurationProp = "gameDuration";

		public RectTransform LobbyPlayerList
		{
			get
			{
				return m_LobbyPlayerList;
			}
		}

		/// <summary>
		/// Initialises the class.
		/// </summary>
		private void Start()
		{
			m_StartGameBtn.onClick.AddListener(() =>
			{
				StartCoroutine(ServerCountdownCoroutine(m_CountdownDuration));
			});
		}

		/// <summary>
		/// [PunRPC] Updates the countdown popup with a given time in seconds.
		/// </summary>
		[PunRPC]
		private void UpdateCountDown(byte time)
		{
			NetworkManager.Singleton.CountdownManager.SetText("Het spel start in...\n" + time);
		}

		/// <summary>
		/// [PunRPC] Shows the countdown popup.
		/// </summary>
		[PunRPC]
		private void StartCountDown(bool state)
		{
			NetworkManager.Singleton.CountdownManager.EnableDisableMenu(state);
		}

		/// <summary>
		/// Coroutine for starting and updating the countdown when the game is about to begin. Hides the room from the lobby lists and loads the new scene on completion.
		/// </summary>
		public IEnumerator ServerCountdownCoroutine(int time)
		{
			//photonView.RPC("StartCountDown", PhotonTargets.AllViaServer, true); todo find out what this does
			StartCountDown(true);

			float remainingTime = time;
			byte floorTime =(byte) Mathf.FloorToInt(remainingTime);

			while (floorTime > 0)
			{
				remainingTime -= Time.deltaTime;
				byte newFloorTime = (byte) Mathf.FloorToInt(remainingTime);

				if (newFloorTime != floorTime)
				{//to avoid flooding the nepunrtwork of message, we only send a notice to client when the number of plain seconds change. 
					floorTime = newFloorTime;

					if (floorTime != 0)
					{
						//photonView.RPC("UpdateCountDown", PhotonTargets.AllViaServer, (byte)floorTime); todo find out what this does
						UpdateCountDown(floorTime);
					}
				}
				yield return null;
			}
			//PhotonNetwork.room.IsVisible = false;
			SceneManager.LoadScene("Game");
		}

		/// <summary>
		/// Sets up a new room in the lobby with the parameters passed by CreateJoinRoomManager. And the player will automatically join this new room.
		/// </summary>
		public void InitializeRoom(string roomName, string roomPassword, int gameDuration, byte amountPlayers)
		{
			EnableDisableMenu(true);

			string[] lobbyOptions = new string[2];
			lobbyOptions[0] = RoomPasswordProp;
			lobbyOptions[1] = RoomGameDurationProp;

			ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
			ht.Add(RoomPasswordProp, roomPassword);
			ht.Add(RoomGameDurationProp, gameDuration);

			RoomOptions roomOptions = new RoomOptions()
			{
				MaxPlayers = amountPlayers,
				IsVisible = true,
				CustomRoomPropertiesForLobby = lobbyOptions,
				CustomRoomProperties = ht
			};

			PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
		}

		/// <summary>
		/// Generic function for handling switching between the different menus.
		/// </summary>
		public void EnableDisableMenu(bool newState)
		{
			gameObject.SetActive(newState);
			if (newState)
			{
				NetworkManager.Singleton.TopPanelManager.EnableDisableButton(true, new UnityAction(() =>
				{
					EnableDisableMenu(false);
					NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
				}));
			}
			else
			{
				//PhotonNetwork.LeaveRoom(); todo DELETE
			}
		}

		/// <summary>
		/// [PunBehaviour] Gets called when a player joins the room.
		/// </summary>
		/* todo replace with something equivalent (new message type?)
		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();

			NetworkManager.Singleton.TopPanelManager.SetName(PhotonNetwork.room.Name);
			//PhotonNetwork.Instantiate(NetworkManager.Singleton.LobbyPlayerPrefabName, Vector3.zero, Quaternion.identity, 0); todo DELETE
			NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(false);
		}
		*/

		/// <summary>
		/// [PunBehaviour] Gets called when a player leaves the room. [PunBehaviour]
		/// </summary>
		/* todo replace with something equivalent (new message type?)
		public override void OnLeftRoom()
		{
			base.OnLeftRoom();
			NetworkManager.Singleton.KickedManager.EnableDisableMenu(true);
			EnableDisableMenu(false);
			NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
		}
		*/

		/// <summary>
		/// Iterates trough every player in the room and checks if every player has pressed check. If everyone is ready the start button gets shown.
		/// </summary>
		/// 
		/// <remarks>
		/// Editor and desktop debug builds are overridden and always show the start button for testing purposes.
		/// </remarks>
		public void CheckIfReadyToStart()
		{
			int playersReady = 0;
			foreach (Transform item in m_LobbyPlayerList)
			{
				if (item.GetComponent<LobbyPlayer>().IsReady)
				{
					++playersReady;
				}
			}
			if (playersReady == PhotonNetwork.room.MaxPlayers)
			{
				m_StartGameBtn.gameObject.SetActive(true);
			}
			else
			{
				m_StartGameBtn.gameObject.SetActive(false);
			}


#if (UNITY_EDITOR)
			m_StartGameBtn.gameObject.SetActive(true);
			m_StartGameBtn.transform.GetChild(0).GetComponent<Text>().text = "OVERRIDE START! Unity Editor Only";
#endif

#if (UNITY_STANDALONE)
			if (Debug.isDebugBuild)
			{
				m_StartGameBtn.gameObject.SetActive(true);
				m_StartGameBtn.transform.GetChild(0).GetComponent<Text>().text = "OVERRIDE START! Unity Editor Only";
			}
#endif
		}

		/// <summary>
		/// Allows public acces to disable the start button.
		/// </summary>
		public void DisableStartButton()
		{
			m_StartGameBtn.gameObject.SetActive(false);
		}
	}
}