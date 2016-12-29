using Prototype.NetworkLobby;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
	static public GameManager s_Singleton;

	private float mGameLength;
	private bool mGameIsRunning = false;
	private float nextMoneyUpdateTime;
	private float moneyUpdateTimeInterval = 5;

	public SyncList<GameObject> mTeamGameObjects;

	public TeamID mLocalPlayerTeamID = TeamID.NotSet;
	public int mLocalPlayerControllerID = 0;

	public int mTeams = 0;

	//public GameObject lobbyServerEntry;

	// Use this for initialization
	void Start()
	{
		if (s_Singleton != null) {
			Destroy(this);
			return;
		}
		s_Singleton = this;
		nextMoneyUpdateTime = moneyUpdateTimeInterval;
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update()
	{
		if (mGameIsRunning) {
			if (Time.timeSinceLevelLoad > mGameLength) {
				//Debug.Log("The game has ended");
			}
			if (Time.timeSinceLevelLoad > nextMoneyUpdateTime) {
				// Call GainMoneyOverTime() from each financial object
				nextMoneyUpdateTime = Time.timeSinceLevelLoad + moneyUpdateTimeInterval;
			}
		}
	}

	public void StartGame(LobbyPlayer[,] players)
	{
		if (mTeamGameObjects != null) {
			for (int i = 0; i < mTeamGameObjects.Count; i++) {
				DestroyImmediate(mTeamGameObjects[i]);
			}
			mTeamGameObjects.Clear();
		}

		for (int i = 0; i < players.GetLength(0); i++) {
			mTeamGameObjects.Add(new GameObject("Team" + (i + 1)));
			mTeamGameObjects[i].transform.parent = transform;
			mTeamGameObjects[i].AddComponent<NetworkIdentity>();
			Team Temp = mTeamGameObjects[i].AddComponent<Team>();
			Temp.TeamID = (TeamID)i + 1;
		}

		mGameIsRunning = true;

		for (int i = 0; i < players.GetLength(0); i++) {
			for (int j = 0; j < mTeamGameObjects[i].transform.childCount; j++) {
				Transform player = mTeamGameObjects[i].transform.GetChild(j);
				if (player.name == mLocalPlayerControllerID.ToString()) {
					player.gameObject.AddComponent<Player>();
					player.name = "Player ID:" + player.name;
				}
				else if (j == (int)mLocalPlayerTeamID - 1) {
					player.gameObject.AddComponent<Friend>();
					player.name = "Friend ID:" + player.name;
				}
				else {
					player.gameObject.AddComponent<Enemy>();
					player.name = "Enemy ID:" + player.name;
				}
			}
		}
	}

	public void UpdateGameDuration(float duration)
	{
		Debug.Log(duration);
		mGameLength = duration;
	}
}
