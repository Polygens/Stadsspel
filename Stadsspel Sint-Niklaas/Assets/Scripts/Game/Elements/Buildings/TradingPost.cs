using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

namespace Stadsspel.Elements
{
	public class TradingPost : Building
	{
		//	private SyncListInt visitedTeams = new SyncListInt();
		private List<int> m_visitedTeams = new List<int>();
		private List<float> m_TeamTimers = new List<float>();

		[SerializeField]
		private int m_CountdownDuration = 300;
		// 5 minuten * 60 = 300

		private float m_UpdateTimer = 0;
		private float m_UpdateTime = 1;

		[PunRPC]
		public void AddTeamToList(int teamID)
		{
			m_visitedTeams.Add(teamID);
			#if (UNITY_EDITOR)
			Debug.Log("Team: " + teamID + " added to visited list");
			#endif
			m_TeamTimers.Add(m_CountdownDuration); 
		}

		private void Update()
		{
			if(m_TeamTimers.Count > 0 && m_visitedTeams.Count > 0) {
				for(int i = 0; i < m_TeamTimers.Count; i++) {
					m_TeamTimers[i] -= Time.deltaTime;
					if(m_TeamTimers[i] <= 0) {
						photonView.RPC("RemoveTeamFromList", PhotonTargets.AllViaServer, i);
						break;
					} else {
						if(InGameUIManager.s_Singleton.TradingPostUI.gameObject.activeSelf) {
							m_UpdateTimer += Time.deltaTime;
							if(m_UpdateTimer > m_UpdateTime) {
								m_UpdateTimer = 0;
								if(m_visitedTeams[i] == (int)GameManager.s_Singleton.Player.Person.Team)
									photonView.RPC("UpdateUI", PhotonTargets.AllViaServer, Mathf.RoundToInt(m_TeamTimers[i]));
							}
						}
					}
				}
			}
		}

		[PunRPC]
		public void RemoveTeamFromList(int index)
    {
      if (m_visitedTeams.Count > 0) 
      {
        if (m_visitedTeams[index] == (int)GameManager.s_Singleton.Player.Person.Team)
        {
          m_visitedTeams.RemoveAt(index);
          m_TeamTimers.RemoveAt(index);
        }

      }

			InGameUIManager.s_Singleton.TradingPostUI.MessagePanel.SetActive(false);
			#if (UNITY_EDITOR)
			Debug.Log("Team: " + (int)GameManager.s_Singleton.Player.Person.Team + " removed from visited list");
			#endif
		}

		[PunRPC]
		private void UpdateUI(int time)
		{
			if(time > 60) {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        string message = string.Format("{0:0}:{1:00}", minutes, seconds);
				InGameUIManager.s_Singleton.TradingPostUI.MessagePanelText.text = "Je moet nog " + message + " minuten wachten om bij deze winkel goederen te kopen. Jou team is hier al reeds geweest.";
			} else {
				InGameUIManager.s_Singleton.TradingPostUI.MessagePanelText.text = "Je moet nog " + time + " seconden wachten om bij deze winkel goederen te kopen.";
			}
     
		}

		private new void Start()
		{
			m_BuildingType = BuildingType.Tradingpost;
			base.Start();
		}

		public List<int> VisitedTeams {
			get {
				//List<int> teams = new List<int>();
				//foreach (int team in m_visitedTeams)
				//{
				//    teams.Add(team);
				//}
				return m_visitedTeams;
			}
		}
	}
}