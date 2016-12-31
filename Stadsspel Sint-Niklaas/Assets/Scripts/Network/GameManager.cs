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

	private Team[] mTeams;

	private Player mPlayer;

	public Player Player {
		get {
			return mPlayer;
		}

		set {
			mPlayer = value;
		}
	}

	public Team[] Teams {
		get {
			return mTeams;
		}

		private set {
			mTeams = value;
		}
	}

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

	public void GenerateTeams(int numberOfTeams)
	{
		for (int i = 0; i < transform.childCount; i++) {
			DestroyImmediate(transform.GetChild(i));
		}
		Teams = new Team[numberOfTeams];

		for (int i = 0; i < numberOfTeams; i++) {
			GameObject temp = new GameObject("Team" + (i + 1));
			temp.AddComponent<NetworkIdentity>();
			temp.transform.parent = transform;
			Teams[i] = temp.AddComponent<Team>();
			Teams[i].TeamID = (TeamID)i + 1;
		}
	}

	public void StartGame(LobbyPlayer[,] lobbyPlayers)
	{
		mGameIsRunning = true;

		for (int i = 0; i < lobbyPlayers.GetLength(0); i++) {
			for (int j = 0; j < lobbyPlayers.GetLength(1); j++) {
				if (lobbyPlayers[i, j] != null) {
					GameObject gamePlayer = lobbyPlayers[i, j].gameObject.transform.GetChild(lobbyPlayers[i, j].transform.childCount - 1).gameObject;
					gamePlayer.transform.SetParent(transform.GetChild(i));
					if (lobbyPlayers[i, j].netId.Value == LobbyPlayer.mLocalPlayerNetID) {
						Player = gamePlayer.gameObject.AddComponent<Player>();
						gamePlayer.name = "Player ID:" + lobbyPlayers[i, j].netId;
					}
					else if (lobbyPlayers[i, j].mPlayerTeam == LobbyPlayer.mLocalPlayerTeam) {
						gamePlayer.gameObject.AddComponent<Friend>();
						gamePlayer.name = "Friend ID:" + lobbyPlayers[i, j].netId;
					}
					else {
						gamePlayer.gameObject.AddComponent<Enemy>();
						gamePlayer.name = "Enemy ID:" + lobbyPlayers[i, j].netId;
					}
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
