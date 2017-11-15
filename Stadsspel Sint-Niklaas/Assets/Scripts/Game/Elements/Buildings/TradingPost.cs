using System.Collections.Generic;
using UnityEngine;

namespace Stadsspel.Elements
{
	public class TradingPost : Building
	{
		public TradingpostType tradingpostType;
		public string naamTradingpost;

		//	private SyncListInt visitedTeams = new SyncListInt();
		private List<int> m_visitedTeams = new List<int>();
		private List<float> m_TeamTimers = new List<float>();

		[SerializeField]
		private int m_CountdownDuration = 300;
		// 5 minuten * 60 = 300

		//private float m_UpdateTimer = 0;
		//private float m_UpdateTime = 1;


		public List<int> VisitedTeams {
			get {
				return m_visitedTeams;
			}
		}

		public string TPId { get; set; }

		/// <summary>
		/// Initialises the class.
		/// </summary>
		private new void Start()
		{
			m_BuildingType = BuildingType.Tradingpost;
			base.Start();
		}


		/// <summary>
		/// [PunRPC] Adds the passed TeamID to the list of teams that have visited this TradingPost.
		/// </summary>
		
		public void AddTeamToList(int teamID)
		{
			m_visitedTeams.Add(teamID);
#if(UNITY_EDITOR)
			Debug.Log("Team: " + teamID + " added to visited list");
#endif
			m_TeamTimers.Add(m_CountdownDuration);
		}

		/// <summary>
		///  Updates the remaining time till the team can trade at this TradingPost again.
		/// </summary>
		
		private void UpdateUI(int time)
		{
			if(time > 60) {
				int minutes = Mathf.FloorToInt(time / 60F);
				int seconds = Mathf.FloorToInt(time - minutes * 60);
				string message = string.Format("{0:0}:{1:00}", minutes, seconds);
				InGameUIManager.s_Singleton.TradingPostUI.MessagePanelText.text = "Je moet nog " + message + " minuten wachten om bij deze handelspost goederen te kopen. Jouw team is hier al reeds geweest.";
			}
			else {
				InGameUIManager.s_Singleton.TradingPostUI.MessagePanelText.text = "Je moet nog " + time + " seconden wachten om bij deze handelspost goederen te kopen. Jouw team is hier al reeds geweest.";
			}

		}
	}

	public enum TradingpostType
	{
		Bloemen,
		Bier,
		Ijs,
		Textiel,
		Bakstenen,
		Kunst
	} 
}
