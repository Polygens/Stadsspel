using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Team : MonoBehaviour {

    Color teamcolor;

    int amountOfPlayers;
    int maxPlayers;

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

    public void AddPlayer(NetworkLobbyPlayer player)
    {
        teamMembers.Add(player);
        amountOfPlayers++;
    }

    public void RemovePlayer(NetworkLobbyPlayer player)
    {
        teamMembers.Remove(player);
        amountOfPlayers--;
    }
}
