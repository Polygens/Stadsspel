using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

namespace Stadsspel.Elements
{
	public class TradingPost : Building
	{
    TradingPostUI tradingPostUI;
		//	private SyncListInt visitedTeams = new SyncListInt();
		private List<int> m_visitedTeams = new List<int>();
    private List<float> m_TeamTimers = new List<float>();

    [SerializeField]
    private int m_CountdownDuration = 1200; // 20 minuten * 60

    private float m_UpdateTimer = 0;
    private float m_UpdateTime = 1;

    GameObject messagePanel;


    [PunRPC]
		public void AddTeamToList()
		{
			m_visitedTeams.Add((int)GameManager.s_Singleton.Player.Person.Team);
      m_TeamTimers.Add(m_CountdownDuration); 
    }

    private void Update()
    {
      if (m_TeamTimers.Count > 0)
      {
        for (int i = 0; i < m_TeamTimers.Count; i++)
        {
          m_TeamTimers[i] -= Time.deltaTime;
          if (m_TeamTimers[i] <= 0)
          {
            photonView.RPC("RemoveTeamFromList",PhotonTargets.AllViaServer, i);
            return;
          }
          else
          {
            if (messagePanel.activeSelf)
            {
              m_UpdateTimer += Time.deltaTime;
              if (m_UpdateTimer > m_UpdateTime)
              {
                m_UpdateTimer = 0;
                if (m_visitedTeams[i] == (int)GameManager.s_Singleton.Player.Person.Team)
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
      m_visitedTeams.RemoveAt(index);
      m_TeamTimers.RemoveAt(index);
      messagePanel.SetActive(false);
    }

    [PunRPC]
    private void UpdateUI(int time)
    {
      if (time > 60)
      {
        int seconds = time % 60;
        if (seconds < 10)
        {
          messagePanel.GetComponentInChildren<Text>().text = "Je moet nog " + (time / 60).ToString() + " : 0" + seconds.ToString() + " minuten wachten om bij deze winkel goederen te kopen.";
        }
        messagePanel.GetComponentInChildren<Text>().text = "Je moet nog " + (time / 60).ToString() + " : " + seconds.ToString() + " minuten wachten om bij deze winkel goederen te kopen.";
      }
      else
      {
        messagePanel.GetComponentInChildren<Text>().text = "Je moet nog " + time + " seconden wachten om bij deze winkel goederen te kopen.";
      }
     
    }

    private new void Start()
		{
			m_BuildingType = BuildingType.Tradingpost;
			base.Start();
      tradingPostUI = (TradingPostUI)GameObject.Find("Panels").GetComponentInChildren(typeof(TradingPostUI), true);
      messagePanel = GameObject.Find("Panels").transform.FindChild("TradingPost").transform.FindChild("MessagePanel").gameObject;
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