using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class TradingPost : Building
{
	private SyncListInt visitedTeams = new SyncListInt();

	//[Command]
	public void CmdAddTeamToList()
	{
		visitedTeams.Add((int)GameObject.FindWithTag("Player").GetComponent<Player>().Team);
	}

	private new void Start()
	{
		base.Start();
	}

	public List<int> VisitedTeams {
		get {
			List<int> teams = new List<int>();
			foreach (int team in visitedTeams) {
				teams.Add(team);
			}
			return teams;
		}
	}
}
