using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class GameManager : MonoBehaviour
{

	public int mAmountOfTeams = 6;
	public int mMaxPlayersPerTeam = 1;
	private int mMatchSize;

	public static List<Team> mTeams = new List<Team>();

	//public GameObject lobbyServerEntry;

	// Use this for initialization
	void Start()
	{
		DontDestroyOnLoad(gameObject);
		//lobbyServerEntry = this.transform.parent.gameObject;

		//matchSize = lobbyServerEntry.GetComponent<LobbyServerEntry>().slotInfo.text.Split('/')...
		//maxPlayersPerTeam = Mathf.CeilToInt(match.currentSize / amountOfTeams);


		for (int i = 1; i <= mAmountOfTeams; i++) {
			mTeams.Add(new Team((TeamID)i, mMaxPlayersPerTeam));
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
