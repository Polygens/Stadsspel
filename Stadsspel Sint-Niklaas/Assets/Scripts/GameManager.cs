using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public int amountOfTeams = 6;
    public int maxPlayersPerTeam = 0;

    public Team teamMagenta, teamRed, teamCyan, teamBlue, teamYellow, teamGreen;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
        //maxPlayersPerTeam = amount of players in lobby / amountOfTeams

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
