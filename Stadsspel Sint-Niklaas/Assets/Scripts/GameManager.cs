using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class GameManager : NetworkBehaviour
{

    public int amountOfTeams = 6;
    public int maxPlayersPerTeam = 1;
    private int matchSize;

    public Team teamMagenta, teamRed, teamCyan, teamBlue, teamYellow, teamGreen;

    public GameObject lobbyServerEntry;

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        lobbyServerEntry = this.transform.parent.gameObject;

        //matchSize = lobbyServerEntry.GetComponent<LobbyServerEntry>().slotInfo.text.Split('/')...
        //maxPlayersPerTeam = Mathf.CeilToInt(match.currentSize / amountOfTeams);
       
        teamMagenta = new Team(Color.magenta, maxPlayersPerTeam);
        teamRed = new Team(Color.red, maxPlayersPerTeam);
        teamCyan = new Team(Color.cyan, maxPlayersPerTeam);
        teamBlue = new Team(Color.blue, maxPlayersPerTeam);
        teamYellow = new Team(Color.yellow, maxPlayersPerTeam);
        teamGreen = new Team(Color.green, maxPlayersPerTeam);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
