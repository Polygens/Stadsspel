using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreUI : MonoBehaviour
{
    public GameObject scoreEntryPrefab;
    public Transform content;

    private List<Team> teamOrder = new List<Team>();

    private void OnEnable()
    {
        SortTeams();

        for (int i = 0; i < teamOrder.Count; i++)
        {
            GameObject scoreEntryInstance = Instantiate(scoreEntryPrefab, content);
            scoreEntryInstance.transform.FindChild("Place").GetComponent<Text>().text = (i + 1).ToString();
            scoreEntryInstance.transform.FindChild("Team").GetComponent<Image>().color = TeamData.GetColor(teamOrder[i].TeamID);
            scoreEntryInstance.transform.FindChild("Money").GetComponent<Text>().text = teamOrder[i].TotalMoney.ToString();
        }
    }

    private void SortTeams()
    {
        for (int i = 0; i < GameManager.s_Singleton.Teams.Length; i++)
        {
            teamOrder.Add(GameManager.s_Singleton.Teams[i]);
        }

        teamOrder.Sort(SortByTotalMoney);
    }

    static int SortByTotalMoney(Team t1, Team t2)
    {
        return t1.TotalMoney.CompareTo(t2.TotalMoney);
    }
}
