using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace Stadsspel.Networking
{
	public class RoomManager : MonoBehaviour
	{
		[SerializeField] private RectTransform _mLobbyPlayerList;
		[SerializeField] private Button _mStartGameBtn;

		public const string RoomPasswordProp = "password";
		public const string RoomGameDurationProp = "gameDuration";
		private List<ServerTeam> teams;
		private IDictionary<string, GameObject> playerObjects;

		public RectTransform LobbyPlayerList
		{
			get { return _mLobbyPlayerList; }
		}

		/// <summary>
		/// Initialises the class.
		/// </summary>
		private void Start()
		{
			_mStartGameBtn.onClick.AddListener(() =>
			{
				if (CurrentGame.Instance.HostingLoginToken != null || !CurrentGame.Instance.HostingLoginToken.Equals(""))
					Rest.StartGame(CurrentGame.Instance.GameId, CurrentGame.Instance.HostingLoginToken);
			});
		}

		/// <summary>
		///  Updates the countdown popup with a given time in seconds.
		/// </summary>
		private void UpdateCountDown(byte time)
		{
			NetworkManager.Singleton.CountdownManager.SetText("Het spel start in...\n" + time);
		}

		/// <summary>
		///  Shows the countdown popup.
		/// </summary>
		private void StartCountDown(bool state)
		{
			NetworkManager.Singleton.CountdownManager.EnableDisableMenu(state);
		}

		/// <summary>
		/// Coroutine for starting and updating the countdown when the game is about to begin. Hides the room from the lobby lists and loads the new scene on completion.
		/// </summary>
		public IEnumerator ServerCountdownCoroutine(int time)
		{
			StartCountDown(true);

			float remainingTime = time;
			byte floorTime = (byte) Mathf.FloorToInt(remainingTime);

			while (floorTime > 0)
			{
				remainingTime -= Time.deltaTime;
				byte newFloorTime = (byte) Mathf.FloorToInt(remainingTime);

				if (newFloorTime != floorTime)
				{
//to avoid flooding the nepunrtwork of message, we only send a notice to client when the number of plain seconds change. 
					floorTime = newFloorTime;

					if (floorTime != 0)
					{
						UpdateCountDown(floorTime);
					}
				}
				yield return null;
			}
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
					Rest.UnregisterPlayer(CurrentGame.Instance.LocalPlayer.clientID, CurrentGame.Instance.GameId);
					CurrentGame.Instance.ClearPersistentData();
					EnableDisableMenu(false);
					NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
				}));
			}
		}

		/// <summary>
		/// todo replace with a muxing message
		/// </summary>
		public void OnLobbyLoad()
		{
			NetworkManager.Singleton.TopPanelManager.SetName(CurrentGame.Instance.gameDetail.roomName);

			//todo replace with detect and update
			if (playerObjects != null)
			{
				foreach (var gameObj in playerObjects.Values)
					Destroy(gameObj);
				
				playerObjects = null;
			}

			teams = CurrentGame.Instance.gameDetail.teams;
			playerObjects = new Dictionary<string, GameObject>();
			foreach (var serverTeam in teams)
			{
				foreach (var player in serverTeam.players)
				{
					GameObject go = (GameObject) Instantiate(Resources.Load(NetworkManager.Singleton.LobbyPlayerPrefabName),
						Vector3.zero, Quaternion.identity);
					LobbyPlayer lobbyPlayer = go.GetComponent<LobbyPlayer>();
					if (lobbyPlayer != null)
					{
						lobbyPlayer.Initialise(serverTeam, player);
					}
					go.transform.SetParent(_mLobbyPlayerList.transform, false);
					playerObjects.Add(player.clientID, go);
				}
			}

			if (!CurrentGame.Instance.isHost)
			{
				DisableStartButton();
			}
			else
			{
				_mStartGameBtn.gameObject.SetActive(true);
			}

			NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(false);
		}

		/// <summary>
		/// Allows public acces to disable the start button.
		/// </summary>
		public void DisableStartButton()
		{
			_mStartGameBtn.gameObject.SetActive(false);
		}
	}
}
