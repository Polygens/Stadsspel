using UnityEngine;
using System.Collections.Generic;

namespace Stadsspel.Elements
{
	public class TradingPost : Building
	{
		//	private SyncListInt visitedTeams = new SyncListInt();
		private List<int> m_visitedTeams = new List<int>();

		//[Command]
		public void CmdAddTeamToList()
		{
			m_visitedTeams.Add((int)GameObject.FindWithTag("Player").GetComponent<Person>().Team);
		}

		private new void Start()
		{
			m_BuildingType = BuildingType.Tradingpost;
			base.Start();
		}

		public List<int> VisitedTeams {
			get {
				List<int> teams = new List<int>();
				foreach(int team in m_visitedTeams) {
					teams.Add(team);
				}
				return teams;
			}
		}
	}
}