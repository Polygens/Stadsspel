using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class Team
{

	private TeamID mTeamID;

	private int mAmountOfPlayers;

	private int mMaxPlayers;

	private int mTotalMoney = 0;
	TeamNetworking mTM;

	public List<LobbyPlayer> teamMembers = new List<LobbyPlayer>();

	public Team(TeamID id, int pMaxPlayers)
	{
		mTeamID = id;
		mMaxPlayers = pMaxPlayers;
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void AddPlayer(LobbyPlayer player)
	{
		teamMembers.Add(player);
		mAmountOfPlayers++;
		//TM.ChangeAmountOfPlayers(amountOfPlayers);

	}

	//[Command]
	public void CmdRemovePlayer(LobbyPlayer player)
	{
		teamMembers.Remove(player);
		mAmountOfPlayers--;
	}
}
