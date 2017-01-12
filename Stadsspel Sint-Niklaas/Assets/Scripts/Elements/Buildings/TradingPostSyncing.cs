using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class TradingPostSyncing : NetworkBehaviour {

  SyncListInt visitedTeams = new SyncListInt();

  //[Command]
  public void CmdAddTeamToList()
  {
    visitedTeams.Add((int)GameObject.FindWithTag("Player").GetComponent<Player>().Team);
  }


  public List<int> VisitedTeams
  {
    get
    {
      List<int> teams = new List<int>();
      foreach (int team in visitedTeams)
      {
        teams.Add(team);
      }
      return teams;
    }
  }
}
