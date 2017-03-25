using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalScoreUI : MonoBehaviour
{
	public GameObject m_ScoreEntryPrefab;
	public Transform m_Content;

	private List<Team> m_TeamOrder = new List<Team>();

	/// <summary>
	/// Gets called when the GameObject becomes active. Creates the scoreboard and calls sorting of teams.
	/// </summary>
	private void OnEnable()
	{
		SortTeams();

		for(int i = 0; i < m_TeamOrder.Count; i++) {
			GameObject scoreEntryInstance = Instantiate(m_ScoreEntryPrefab);
			scoreEntryInstance.transform.SetParent(m_Content, false);
			scoreEntryInstance.transform.FindChild("Place").GetComponent<Text>().text = (i + 1).ToString();
			scoreEntryInstance.transform.FindChild("Team").GetComponent<Image>().color = TeamData.GetColor(m_TeamOrder[i].TeamID);
			scoreEntryInstance.transform.FindChild("Money").GetComponent<Text>().text = m_TeamOrder[i].TotalMoney.ToString();
		}
	}

	/// <summary>
	/// Sorts the teams based on the amount of money gained in game.
	/// </summary>
	private void SortTeams()
	{
		for(int i = 0; i < GameManager.s_Singleton.Teams.Length; i++) {
			m_TeamOrder.Add(GameManager.s_Singleton.Teams[i]);
		}

		m_TeamOrder.Sort(SortByTotalMoney);
		m_TeamOrder.Reverse();
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
		PhotonNetwork.Disconnect();
		SceneManager.LoadScene("Lobby");
	}
}
