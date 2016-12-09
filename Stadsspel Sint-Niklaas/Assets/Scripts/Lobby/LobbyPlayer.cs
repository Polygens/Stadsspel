using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Prototype.NetworkLobby
{
	//Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
	//Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
	public class LobbyPlayer : NetworkLobbyPlayer
	{
		public Button colorButton;
		public InputField nameInput;
		public Button readyButton;
		public Button removePlayerButton;

		public Text mIcon;

		//OnMyName function will be invoked on clients when server change the value of playerName
		[SyncVar(hook = "OnMyName")]
		public string playerName = "";
		[SyncVar(hook = "OnMyColor")]
		public TeamID playerTeam = TeamID.NotSet;

		public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
		public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);
		public Color LocalPlayer = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

		private static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
		private static Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
		private static Color TransparentColor = new Color(0, 0, 0, 0);

		private bool mIsHost = false;
		private bool mIsReady = false;
		private static bool mHost;

		public bool IsHost {
			get {
				return mIsHost;
			}

			set {
				mIsHost = value;
			}
		}

		public override void OnClientEnterLobby()
		{
			base.OnClientEnterLobby();

			if (LobbyManager.s_Singleton != null)
				LobbyManager.s_Singleton.OnPlayersNumberModified(1);

			LobbyPlayerList._instance.AddPlayer(this);
			LobbyPlayerList._instance.DisplayDirectServerWarning(isServer && LobbyManager.s_Singleton.matchMaker == null);


			if (isLocalPlayer) {
				SetupLocalPlayer();
			}
			else {
				SetupOtherPlayer();
			}

			//setup the player data on UI. The value are SyncVar so the player
			//will be created with the right value currently on server
			OnMyName(playerName);
			OnMyColor(playerTeam);
		}

		public override void OnStartAuthority()
		{
			base.OnStartAuthority();
			//if we return from a game, color of text can still be the one for "Ready"
			readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;
			if (mIsHost) {
				mHost = true;
			}
			SetupLocalPlayer();
		}

		void SetupOtherPlayer()
		{
			nameInput.interactable = false;

			if (mIsHost) {
				mIcon.text = "";
			}
			else {
				mIcon.text = "";
				mIcon.fontSize = 59;
			}

			readyButton.gameObject.SetActive(true);

			if (mHost) {
				removePlayerButton.gameObject.SetActive(true);
			}

			OnClientReady(mIsReady);
		}

		void SetupLocalPlayer()
		{
			nameInput.interactable = true;

			GetComponent<Image>().color = LocalPlayer;

			if (mIsHost) {
				mIcon.text = "";
			}
			else {
				mIcon.text = "";
				readyButton.gameObject.SetActive(true);
				readyButton.interactable = true;

				ChangeReadyButtonColor(NotReadyColor);
			}

			if (playerTeam == TeamID.NotSet) {
				CmdColorChange();
			}

			//have to use child count of player prefab already setup as "this.slot" is not set yet
			if (playerName == "")
				CmdNameChanged("Player " + (LobbyPlayerList._instance.playerListContentTransform.childCount - 1));

			//we switch from simple name display to name input
			colorButton.interactable = true;
			nameInput.interactable = true;

			nameInput.onEndEdit.RemoveAllListeners();
			nameInput.onEndEdit.AddListener(OnNameChanged);

			colorButton.onClick.RemoveAllListeners();
			colorButton.onClick.AddListener(OnColorClicked);

			readyButton.onClick.RemoveAllListeners();
			readyButton.onClick.AddListener(OnReadyClicked);

			//when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
			//the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
			if (LobbyManager.s_Singleton != null)
				LobbyManager.s_Singleton.OnPlayersNumberModified(0);
		}

		//This enable/disable the remove button depending on if that is the only local player or not
		public void CheckRemoveButton()
		{
			if (!isLocalPlayer)
				return;

			int localPlayerCount = 0;
			foreach (PlayerController p in ClientScene.localPlayers)
				localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

			removePlayerButton.interactable = localPlayerCount > 1;
		}

		public override void OnClientReady(bool readyState)
		{
			if (readyState) {
				ChangeReadyButtonColor(TransparentColor);

				Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
				textComponent.text = "READY";
				textComponent.color = ReadyColor;
				colorButton.interactable = false;
				nameInput.interactable = false;
			}
			else {
				ChangeReadyButtonColor(NotReadyColor);

				Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
				textComponent.text = "...";
				textComponent.color = Color.white;
				colorButton.interactable = isLocalPlayer;
				nameInput.interactable = isLocalPlayer;
			}
		}

		private void ChangeReadyButtonColor(Color c)
		{
			ColorBlock b = readyButton.colors;
			b.normalColor = c;
			b.pressedColor = c;
			b.highlightedColor = c;
			b.disabledColor = c;
			readyButton.colors = b;
		}

		public void OnPlayerListChanged(int idx)
		{
			if (!isLocalPlayer) {
				GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
			}
		}

		///===== callback from sync var

		public void OnMyName(string newName)
		{
			playerName = newName;
			nameInput.text = playerName;
		}

		public void OnMyColor(TeamID newTeam)
		{
			if (playerTeam != TeamID.NotSet) {
				RemoveFromOldTeam(playerTeam);
				playerTeam = newTeam;
				int count = 0;
				while (GameManager.mTeams[(byte)playerTeam - 1].TeamIsFull) {
					CmdColorChange();
					count++;
					if (count > GameManager.mTeams.Count) {
						playerTeam = TeamID.NotSet;
						Debug.Log("No free team found!!!");
						break;
					}
				}
				GameManager.mTeams[(byte)playerTeam - 1].AddPlayer(this);
			}
			else {
				playerTeam = newTeam;
			}
		}

		//===== UI Handler

		//Note that those handler use Command function, as we need to change the value on the server not locally
		//so that all client get the new value throught syncvar
		public void OnColorClicked()
		{
			CmdColorChange();
		}

		private void OnReadyClicked()
		{
			mIsReady = !mIsReady;
			OnClientReady(mIsReady);
			if (mIsReady) {
				SendReadyToBeginMessage();
			}
			else {
				SendNotReadyToBeginMessage();
			}
		}

		public void OnNameChanged(string str)
		{
			CmdNameChanged(str);
		}

		public void OnRemovePlayerClick()
		{
			if (isLocalPlayer) {
				RemovePlayer();
			}
		}

		public void ToggleJoinButton(bool enabled)
		{
			readyButton.gameObject.SetActive(enabled);
		}

		[ClientRpc]
		public void RpcUpdateCountdown(int countdown)
		{
			LobbyManager.s_Singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
			LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
		}

		[ClientRpc]
		public void RpcUpdateRemoveButton()
		{
			CheckRemoveButton();
		}

		//====== Server Command

		[Command]
		public void CmdColorChange()
		{
			playerTeam = TeamData.GetNextTeam(playerTeam);
			Debug.Log("Player is now in team: " + playerTeam.ToString());
			colorButton.GetComponent<Image>().color = TeamData.GetColor(playerTeam);
		}

		[Command]
		public void CmdNameChanged(string name)
		{
			playerName = name;
		}

		//Cleanup thing when get destroy (which happen when client kick or disconnect)
		public void OnDestroy()
		{
			LobbyPlayerList._instance.RemovePlayer(this);
			if (LobbyManager.s_Singleton != null)
				LobbyManager.s_Singleton.OnPlayersNumberModified(-1);

			if ((byte)playerTeam < 1)
				return;
		}

		public void RemoveFromOldTeam(TeamID oldTeam)
		{
			GameManager.mTeams[(int)oldTeam - 1].CmdRemovePlayer(this);
		}
	}
}
