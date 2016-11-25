using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Team : NetworkBehaviour
{

    Color teamcolor;

    //[SyncVar]
    int amountOfPlayers;

    //[SyncVar]
    int maxPlayers;

    //[SyncVar]
    public List<NetworkLobbyPlayer> teamMembers = new List<NetworkLobbyPlayer>();

    int totalMoney = 0;

    public Team(Color pTeamColor, int pMaxPlayers)
    {
        teamcolor = pTeamColor;
        maxPlayers = pMaxPlayers;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //[Command]
    public void CmdAddPlayer(NetworkLobbyPlayer player)
    {
        teamMembers.Add(player);
        amountOfPlayers++;
    }

    //[Command]
    public void CmdRemovePlayer(NetworkLobbyPlayer player)
    {
        teamMembers.Remove(player);
        amountOfPlayers--;
    }
}
