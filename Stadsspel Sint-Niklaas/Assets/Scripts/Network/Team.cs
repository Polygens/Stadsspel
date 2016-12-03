using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class Team : NetworkBehaviour
{
	[SyncVar(hook = "UpdateAmountOfPlayers")]
	private int mAmountOfPlayers;
	private TeamID mTeamID;
	private int mMaxPlayers;
	[SyncVar]
	private bool mTeamIsFull;
	[SyncVar]
	private int mTotalMoney = 0;
	private List<LobbyPlayer> teamMembers = new List<LobbyPlayer>();

	public TeamID TeamID {
		get {
			return mTeamID;
		}

		set {
			mTeamID = value;
		}
	}

	public int MaxPlayers {
		get {
			return mMaxPlayers;
		}

		set {
			mMaxPlayers = value;
			mTeamIsFull = CheckIfFull();
		}
	}

	public bool TeamIsFull {
		get {
			return mTeamIsFull;
		}
	}

	public bool AddPlayer(LobbyPlayer player)
	{
		if (!TeamIsFull) {
			teamMembers.Add(player);
			mAmountOfPlayers++;
			mTeamIsFull = CheckIfFull();
			return true;
		}

		return false;
	}

	//[Command]
	public void CmdRemovePlayer(LobbyPlayer player)
	{
		if (teamMembers.Contains(player)) {
			teamMembers.Remove(player);
			mAmountOfPlayers--;
			mTeamIsFull = CheckIfFull();
		}
	}

	private bool CheckIfFull()
	{
		if (mAmountOfPlayers >= MaxPlayers) {
			return true;
		}
		return false;
	}

	private void UpdateAmountOfPlayers(int nbrOfPlayers)
	{
		mAmountOfPlayers = nbrOfPlayers;
		mTeamIsFull = CheckIfFull();
	}
}
