using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class PlayerInitializer : NetworkBehaviour
{
	[SyncVar(hook = "OnMyTeam")]
	private TeamID mTeam = TeamID.NotSet;

	[SyncVar(hook = "OnMyName")]
	private string mName = "";

	private bool mIsReady = false;

	public TeamID Team {
		get {
			return mTeam;
		}

		set {
			mTeam = value;
		}
	}

	public string Name {
		get {
			return mName;
		}

		set {
			mName = value;
		}
	}

	public void OnMyTeam(TeamID team)
	{
		mTeam = team;

		GetComponent<Renderer>().material.color = TeamData.GetColor(mTeam);
		transform.SetParent(GameManager.s_Singleton.transform.GetChild(0).GetChild(0));
	}

	public void OnMyName(string name)
	{
		mName = name;

		transform.GetChild(0).GetComponent<TextMesh>().text = name;
	}

	public void Update()
	{
		int amountOfTeams = LobbyPlayerList._instance.LobbyPlayerMatrix.GetLength(0);
		int players = GameManager.s_Singleton.transform.GetChild(0).GetChild(0).childCount;
		if (players > 0 && mTeam != TeamID.NotSet && mName != "" && !mIsReady && GameManager.s_Singleton.transform.GetChild(0).GetChild(1).childCount == amountOfTeams) {
			mIsReady = true;

			Debug.Log("Starting game");

			GameManager.s_Singleton.Teams = new Team[amountOfTeams];

			for (int i = 0; i < amountOfTeams; i++) {
				GameManager.s_Singleton.Teams[i] = GameManager.s_Singleton.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Team>();
				GameManager.s_Singleton.Teams[i].gameObject.transform.SetParent(GameManager.s_Singleton.transform.transform);
				GameManager.s_Singleton.Teams[i].gameObject.name = "Team: " + (i + 1);
			}

			for (int i = 0; i < players; i++) {
				GameObject playerObject = GameManager.s_Singleton.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
				playerObject.transform.SetParent(GameManager.s_Singleton.transform.GetChild((int)playerObject.GetComponent<PlayerInitializer>().Team));
				NetworkIdentity networkIdentity = playerObject.GetComponent<NetworkIdentity>();
				TeamID teamID = playerObject.GetComponent<PlayerInitializer>().Team;
				string name = playerObject.GetComponent<PlayerInitializer>().Name;
				if (networkIdentity.isLocalPlayer) {
					Player player = playerObject.AddComponent<Player>();
					player.Team = teamID;
					playerObject.name = "Player ID:" + networkIdentity.netId + " (" + name + ")";
				}
				else if (LobbyPlayer.mLocalPlayerTeam == teamID) {
					Friend friend = playerObject.AddComponent<Friend>();
					friend.Team = teamID;
					playerObject.name = "Friend ID:" + networkIdentity.netId + " (" + name + ")";
				}
				else {
					Enemy enemy = playerObject.AddComponent<Enemy>();
					enemy.Team = teamID;
					playerObject.name = "Enemy ID:" + networkIdentity.netId + " (" + name + ")";
				}
			}
		}
	}
}
