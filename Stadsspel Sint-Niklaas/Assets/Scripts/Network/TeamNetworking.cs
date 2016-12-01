using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections.Generic;

public class TeamNetworking : NetworkBehaviour {

	[SyncVar(hook = "ChangeAmountOfPlayers")]
	int amountOfPlayers;
  
	//[SyncVar]
	int maxPlayers;

	//[SyncVar]
	public List<LobbyPlayer> teamMembers = new List<LobbyPlayer>();

  //[SyncVar(hook ="UpdateTeam")]
  //List<Team> listOfTeams = new List<Team>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void MakeTeams()
  {

  }

  //public void UpdateTeam(Team team)
  //{
  //  //teamColor = team;
  //}

  public void ChangeAmountOfPlayers(int nbrOfPlayers)
  {
    amountOfPlayers = nbrOfPlayers;
    Debug.Log(nbrOfPlayers);
  }
}
