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

	private List<GameObject> mTeamGameObjects = new List<GameObject>();
	public static List<Team> mTeams = new List<Team>();

	//public GameObject lobbyServerEntry;

	// Use this for initialization
	void Start()
	{
		DontDestroyOnLoad(gameObject);
		//lobbyServerEntry = this.transform.parent.gameObject;

		//matchSize = lobbyServerEntry.GetComponent<LobbyServerEntry>().slotInfo.text.Split('/')...
		//maxPlayersPerTeam = Mathf.CeilToInt(match.currentSize / amountOfTeams);

		for (int i = 0; i < mAmountOfTeams; i++) {
			mTeamGameObjects.Add(new GameObject("Team" + (i + 1)));
			mTeamGameObjects[i].transform.parent = transform;
			mTeams.Add(mTeamGameObjects[i].AddComponent<Team>());
			mTeams[i].MaxPlayers = mMaxPlayersPerTeam;
			mTeams[i].TeamID = (TeamID)i + 1;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
