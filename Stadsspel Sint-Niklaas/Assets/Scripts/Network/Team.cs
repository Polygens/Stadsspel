using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class Team
{

		Color teamcolor;

		int amountOfPlayers;

		int maxPlayers;

		int totalMoney = 0;
		TeamNetworking TM;

	List<LobbyPlayer> teamMembers = new List<LobbyPlayer>();

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

		public void AddPlayer(LobbyPlayer player)
		{
				teamMembers.Add(player);
				amountOfPlayers++;
				//TM.ChangeAmountOfPlayers(amountOfPlayers);
				
		}

		//[Command]
		public void CmdRemovePlayer(LobbyPlayer player)
		{
				//teamMembers.Remove(player);
				amountOfPlayers--;
		}



  public int MaxPlayers
  {
    get { return maxPlayers; }
    set { maxPlayers = value; }
  }

  public int AmountOfPlayers
  {
    get { return amountOfPlayers; }
    set { amountOfPlayers = value; }
  }


}
