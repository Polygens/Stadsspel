﻿using UnityEngine;
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
		[SerializeField]
		private Button colorButton;
		[SerializeField]
		private InputField nameInput;
		[SerializeField]
		private Button readyButton;
		[SerializeField]
		private Button removePlayerButton;

		[SerializeField]
		private Text mIcon;

		//OnMyName function will be invoked on clients when server change the value of playerName
		[SyncVar(hook = "OnMyName")]
		public string mPlayerName = "";
		[SyncVar(hook = "OnMyColor")]
		public TeamID mPlayerTeam = TeamID.NotSet;

		public static TeamID mLocalPlayerTeam = TeamID.NotSet;
		public static int mLocalPlayerControllerID = 0;

		private Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
		private Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);
		private Color LocalPlayer = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

		private Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
		private Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
		private Color TransparentColor = new Color(0, 0, 0, 0);

		private bool mIsHost = false;
		private bool mIsReady = false;
		private static bool mHostInstance = false;

		private string mHostIcon = "";
		private string mLocalPlayerIcon = "";
		private string mOtherPlayerIcon = "";

		public override void OnClientEnterLobby()
		{
			base.OnClientEnterLobby();

			ChangeReadyButton(NotReadyColor, "...", Color.white);

			LobbyPlayerList._instance.DisplayDirectServerWarning(isServer && LobbyManager.s_Singleton.matchMaker == null);
		}

		public override void OnStartLocalPlayer()
		{
			base.OnStartAuthority();
			SetupLocalPlayer();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();

			if (LobbyPlayerList._instance.playerListContentTransform.childCount != 0) {
				SetupOtherPlayer();
			}
		}

		void SetupOtherPlayer()
		{
			Debug.Log("New Player joined");
			nameInput.interactable = false;
			colorButton.interactable = false;
			readyButton.interactable = false;

			OnMyName(mPlayerName);
			OnColorClicked();

			LobbyPlayerList._instance.AddPlayer(this);

			if (LobbyPlayerList._instance.playerListContentTransform.childCount == 1) {
				mIsHost = true;
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


		}

		void SetupLocalPlayer()
		{
			Debug.Log("You entered the lobby");
			GetComponent<Image>().color = LocalPlayer;

			CmdNameChanged("Speler " + (LobbyPlayerList._instance.playerListContentTransform.childCount));
			OnColorClicked();

			if (LobbyPlayerList._instance.playerListContentTransform.childCount == 0) {
				mIsHost = true;
				mHostInstance = true;
				mIcon.text = mHostIcon;

				OnReadyClicked();
				readyButton.interactable = false;
			}
			else {
				mIcon.text = mLocalPlayerIcon;
			}

			mLocalPlayerControllerID = playerControllerId;

			nameInput.onEndEdit.RemoveAllListeners();
			nameInput.onEndEdit.AddListener(OnNameChanged);

			colorButton.onClick.RemoveAllListeners();
			colorButton.onClick.AddListener(OnColorClicked);

			readyButton.onClick.RemoveAllListeners();
			readyButton.onClick.AddListener(OnReadyClicked);

			//we switch from simple name display to name input
			colorButton.interactable = true;
			nameInput.interactable = true;

			LobbyPlayerList._instance.AddPlayer(this);
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
			mPlayerName = newName;
			nameInput.text = mPlayerName;
		}

		public void OnMyColor(TeamID newTeam)
		{
			mPlayerTeam = newTeam;
			Debug.Log("Player " + playerControllerId + " is now in team: " + mPlayerTeam.ToString());
			colorButton.GetComponent<Image>().color = TeamData.GetColor(mPlayerTeam);
		}

		//===== UI Handler

		//Note that those handler use Command function, as we need to change the value on the server not locally
		//so that all client get the new value through syncvar
		public void OnColorClicked()
		{
			TeamID newTeam = mPlayerTeam;

			if (newTeam != TeamID.NotSet) {
				LobbyPlayerList._instance.RemovePlayer(this);
			}

			int count = 0;
			do {
				newTeam = TeamData.GetNextTeam(newTeam);
				count++;
				if (count > LobbyPlayerList._instance.AmountOfTeams) {
					newTeam = TeamID.NotSet;
					Debug.Log("No free team found!!!");
					break;
				}
			} while (LobbyPlayerList._instance.IsTeamFull(newTeam));

			mLocalPlayerTeam = newTeam;

			CmdColorChange(newTeam);
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
			mPlayerTeam = newTeam;
		}

		[Command]
		public void CmdNameChanged(string name)
		{
			mPlayerName = name;
		}

		//Cleanup thing when get destroy (which happen when client kick or disconnect)
		private void OnDestroy()
		{
			LobbyPlayerList._instance.RemovePlayer(this);
		}
	}
}
