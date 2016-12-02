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

  //[SyncVar(hook = "CmdUpdateTeam")]

  SyncList<Team> listOfTeams; //= new SyncList<Team>();

	// Use this for initialization
	void Start () {
	  
	}
	
	// Update is called once per frame
	void Update () {

  }


  public void MakeTeams(List<Team> teams)
  {
    for (int i = 0; i < teams.Count; i++)
    {
      //Debug.Log(teams[i]);
      //listOfTeams.Add(teams[i]);
    }
  }

  public void CmdUpdateTeam(LobbyPlayer player, TeamID id)
  {
    int indexTeam = (int)id;
    Debug.Log(player.playerName);
    listOfTeams[indexTeam].AddPlayer(player);
    for (int i = 0; i < listOfTeams.Count; i++)
    {
      //Debug.Log(id.ToString() + listOfTeams[i].AmountOfPlayers);
    }
  }

  public void ChangeAmountOfPlayers(int nbrOfPlayers)
  {
    amountOfPlayers = nbrOfPlayers;
    Debug.Log(nbrOfPlayers);
  }


}

public enum teams
{

}

