using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System;

public class GameManager : MonoBehaviour
{
	static public GameManager s_Singleton;

	public int mMaxPlayersPerTeam = 1;

	private float mGameLength;
	private bool mGameIsRunning = false;

	private List<GameObject> mTeamGameObjects = new List<GameObject>();
	public static List<Team> mTeams = new List<Team>();



	//public GameObject lobbyServerEntry;

	// Use this for initialization
	void Start()
	{
		if (s_Singleton != null) {
			Destroy(this);
			return;
		}
		s_Singleton = this;

		DontDestroyOnLoad(gameObject);
		//lobbyServerEntry = this.transform.parent.gameObject;

		//matchSize = lobbyServerEntry.GetComponent<LobbyServerEntry>().slotInfo.text.Split('/')...
		//maxPlayersPerTeam = Mathf.CeilToInt(match.currentSize / amountOfTeams);


	}

	// Update is called once per frame
	void Update()
	{
		if (mGameIsRunning) {
			if (Time.timeSinceLevelLoad > mGameLength) {
				Debug.Log("The game has ended");
			}
		}
	}

	public void StartGame()
	{
		mGameIsRunning = true;
	}

	public void GenerateTeams(int numberOfTeams)
	{
		for (int i = 0; i < transform.childCount; i++) {
			Destroy(transform.GetChild(i));
		}

		for (int i = 0; i < numberOfTeams; i++) {
			mTeamGameObjects.Add(new GameObject("Team" + (i + 1)));
			mTeamGameObjects[i].transform.parent = transform;
			mTeams.Add(mTeamGameObjects[i].AddComponent<Team>());
			mTeams[i].MaxPlayers = mMaxPlayersPerTeam;
			mTeams[i].TeamID = (TeamID)i + 1;
		}
	}

	public void UpdateGameDuration(float duration)
	{
		Debug.Log(duration);
		mGameLength = duration;
	}
}
