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

		private Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
		private Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
		private Color TransparentColor = new Color(0, 0, 0, 0);

		private bool mIsHost = false;
		private bool mIsReady = false;
		private static bool mHostInstance = false;

		private string mHostIcon = "";
		private string mLocalPlayerIcon = "";
		private string mOtherPlayerIcon = "";

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

			ChangeReadyButton(NotReadyColor, "...", Color.white);

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
		}

		public override void OnStartAuthority()
		{
			base.OnStartAuthority();
			//if we return from a game, color of text can still be the one for "Ready"
			ChangeReadyButton(NotReadyColor, "...", Color.white);

			if (mIsHost) {
				mHostInstance = true;
			}
			SetupLocalPlayer();
		}

		void SetupOtherPlayer()
		{
			nameInput.interactable = false;
			colorButton.interactable = false;

			if (mIsHost) {
				mIcon.text = mHostIcon;
				ChangeReadyButton(TransparentColor, "GEREED", ReadyColor);
			}
			else {
				mIcon.text = mOtherPlayerIcon;
				mIcon.fontSize = 59;
			}

			if (mHostInstance) {
				removePlayerButton.gameObject.SetActive(true);
				removePlayerButton.onClick.RemoveAllListeners();
				removePlayerButton.onClick.AddListener(OnRemovePlayerClicked);
			}

			OnMyName(playerName);
			OnMyColor(playerTeam);
		}

		void SetupLocalPlayer()
		{
			GetComponent<Image>().color = LocalPlayer;

			if (mIsHost) {
				mIcon.text = mHostIcon;

				OnReadyClicked();
				readyButton.interactable = false;
			}
			else {
				mIcon.text = mLocalPlayerIcon;
			}

			//setup the player data on UI. The value are SyncVar so the player
			//will be created with the right value currently on server
			OnMyName(playerName);
			if (playerTeam == TeamID.NotSet) {
				OnColorClicked();
			}

			//have to use child count of player prefab already setup as "this.slot" is not set yet
			if (playerName == "")
				CmdNameChanged("Speler " + (LobbyPlayerList._instance.playerListContentTransform.childCount));

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

			//we switch from simple name display to name input
			colorButton.interactable = true;
			nameInput.interactable = true;
		}

		public override void OnClientReady(bool readyState)
		{
			if (readyState) {
				ChangeReadyButton(TransparentColor, "GEREED", ReadyColor);

				if (!mIsHost && isLocalPlayer) {
					colorButton.interactable = false;
					nameInput.interactable = false;
				}
			}
			else {
				ChangeReadyButton(NotReadyColor, "...", Color.white);

				if (isLocalPlayer) {
					colorButton.interactable = true;
					nameInput.interactable = true;
				}
			}
		}

		private void ChangeReadyButton(Color c, string text, Color textColor)
		{
			ColorBlock b = readyButton.colors;
			b.normalColor = c;
			b.pressedColor = c;
			b.highlightedColor = c;
			b.disabledColor = c;
			readyButton.colors = b;

			Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
			textComponent.text = text;

			textComponent.color = textColor;
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
			Debug.Log("Player is now in team: " + playerTeam.ToString());
			colorButton.GetComponent<Image>().color = TeamData.GetColor(playerTeam);
		}

		//===== UI Handler

		//Note that those handler use Command function, as we need to change the value on the server not locally
		//so that all client get the new value through syncvar
		public void OnColorClicked()
		{
			if (playerTeam != TeamID.NotSet) {
				RemoveFromOldTeam(playerTeam);
			}

			int count = 0;
			do {
				playerTeam = TeamData.GetNextTeam(playerTeam);
				count++;
				if (count > GameManager.mTeams.Count) {
					playerTeam = TeamID.NotSet;
					Debug.Log("No free team found!!!");
					break;
				}
			} while (GameManager.mTeams[(int)playerTeam - 1].TeamIsFull);

			GameManager.mTeams[(int)playerTeam - 1].AddPlayer(this);
			CmdColorChange(playerTeam);
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

		public void OnRemovePlayerClicked()
		{
			if (isLocalPlayer) {
				RemovePlayer();
			}
			else if (isServer) {
				LobbyManager.s_Singleton.KickPlayer(connectionToClient);
			}
		}

		[ClientRpc]
		public void RpcUpdateCountdown(int countdown)
		{
			LobbyManager.s_Singleton.countdownPanel.UIText.text = "Match start in " + countdown;
			LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
		}

		//====== Server Command

		[Command]
		public void CmdColorChange(TeamID newTeam)
		{
			playerTeam = newTeam;
		}

		[Command]
		public void CmdNameChanged(string name)
		{
			playerName = name;
		}

		//Cleanup thing when get destroy (which happen when client kick or disconnect)
		private void OnDestroy()
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
