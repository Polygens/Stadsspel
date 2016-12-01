using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class GameManager : MonoBehaviour
{

    public int amountOfTeams = 6;
    public int maxPlayersPerTeam = 1;
    private int matchSize;

  public List<Team> teams = new List<Team>();

    //public GameObject lobbyServerEntry;

    // Use this for initialization
    void Start ()
    {
        teams.Add(new Team(Color.magenta, maxPlayersPerTeam));
        teams.Add(new Team(Color.red, maxPlayersPerTeam));
        teams.Add(new Team(Color.cyan, maxPlayersPerTeam));
        teams.Add(new Team(Color.blue, maxPlayersPerTeam));
        teams.Add(new Team(Color.yellow, maxPlayersPerTeam));
        teams.Add(new Team(Color.green, maxPlayersPerTeam));

        DontDestroyOnLoad(gameObject);
        //lobbyServerEntry = this.transform.parent.gameObject;

        //matchSize = lobbyServerEntry.GetComponent<LobbyServerEntry>().slotInfo.text.Split('/')...
        //maxPlayersPerTeam = Mathf.CeilToInt(match.currentSize / amountOfTeams);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
