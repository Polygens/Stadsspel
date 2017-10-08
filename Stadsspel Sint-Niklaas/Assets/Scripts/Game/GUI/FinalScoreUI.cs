using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalScoreUI : MonoBehaviour
{
	public GameObject m_ScoreEntryPrefab;
	public Transform m_Content;

	private List<WinningTeamMessage.TeamScore> m_TeamOrder;

	/// <summary>
	/// Does nothing anymore because it is not certain the data is already present
	/// </summary>
	private void OnEnable(){}


	public void Update()
	{
		if (m_TeamOrder == null && CurrentGame.Instance.TeamScores != null)
		{
			SortTeams();

			for (int i = 0; i < m_TeamOrder.Count; i++)
			{
				GameObject scoreEntryInstance = Instantiate(m_ScoreEntryPrefab);
				scoreEntryInstance.transform.SetParent(m_Content, false);
				scoreEntryInstance.transform.Find("Place").GetComponent<Text>().text = (i + 1).ToString();

				Color c = new Color();
				ServerTeam st = CurrentGame.Instance.FindTeamByName(m_TeamOrder[i].name);
				ColorUtility.TryParseHtmlString(st.customColor, out c);
				scoreEntryInstance.transform.Find("Team").GetComponent<Image>().color = c;

				scoreEntryInstance.transform.Find("Money").GetComponent<Text>().text = ((int)m_TeamOrder[i].score).ToString();
			}

		}
		else
		{
			//todo show loading scores popup
		}
	}

	/// <summary>
	/// Sorts the teams based on the amount of money gained in game.
	/// </summary>
	private void SortTeams()
	{
		m_TeamOrder = CurrentGame.Instance.TeamScores;
		m_TeamOrder = m_TeamOrder.OrderByDescending(t => t.score).ToList();
		//m_TeamOrder.Sort(SortByTotalMoney);
		//m_TeamOrder.Reverse();
	}

	/// <summary>
	/// Returns an int, if smaller then 0 t1 has less money, if 0 they have equal money and if more then 0 t1 has more money then t2.
	/// </summary>
	static int SortByTotalMoney(Team t1, Team t2)
	{
		return t1.TotalMoney.CompareTo(t2.TotalMoney);
	}

	/// <summary>
	/// Event for clicking the leave game button. Leaves the current game.
	/// </summary>
	public void LeaveGame()
	{
		//PhotonNetwork.Disconnect();
		//todo leave game properly?
		SceneManager.LoadScene("Lobby");
	}
}
