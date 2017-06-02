using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Domain;
using UnityEngine;

/// <summary>
/// A singleton that holds all information regarding the current (or last) game known.
/// </summary>
public class CurrentGame : Singleton<CurrentGame>
{
	public WebsocketImpl Ws { get; private set; }
	public string ClientToken { get; set; }
	public bool isHost { get; set; }
	public string GameId { get; set; }
	public string PasswordUsed { get; set; }
	public Game gameDetail { get; set; }
	public LocalPlayer LocalPlayer { get; set; }
	public ServerTeam PlayerTeam { get; set; }
	public bool IsGameRunning { get; set; }
	public bool IsInLobby { get; set; }
	public bool IsTaggingPermitted { get; set; }
	public string nearBank { get; set; }
	public string nearTP { get; set; }
	public string currentDistrict { get; set; }
	public string currentDistrictID { get; set; }
	public IDictionary<string, ServerTradePost.ServerItem> KnownItems;

	[Serializable]
	public class Game
	{
		public string id;
		public string roomName;
		public List<AreaLocation> districts;
		public List<AreaLocation> markets;
		public List<ServerTradePost> tradePosts;
		public List<PointLocation> banks;
		public List<ServerTeam> teams;

		public List<ServerPlayer> hosts;
		public int maxPlayersPerTeam;
		public int maxTeams;
		public long startTime;
		public long endTime;
		public String webAppToken;
		public bool treasuriesOpen;

		public Game(int mAmountOfTeams)
		{
			teams = new List<ServerTeam>(0);
			for (int i = 0; i < mAmountOfTeams; i++)
			{
				teams.Add(new ServerTeam {
					BankAccount = 0,
					TeamName = "TEAM" + (i + 1),
					Treasury = 0,
					Players = new List<ServerPlayer>(),
					CustomColor = "#FF" + (i + 1) + "000"
				}
				);
			}
		}

		/// <summary>
		/// Searches for the team a specific client belongs to.
		/// Returns null if no team contains the given id.
		/// </summary>
		/// <param name="clientId"></param>
		/// <returns></returns>
		public ServerTeam findTeamByPlayer(string clientId)
		{
			foreach (ServerTeam serverTeam in teams)
			{
				if (serverTeam.ContainsPlayer(clientId))
				{
					return serverTeam;
				}
			}
			return null;
		}

		/// <summary>
		/// Searches for the index of a specific team.
		/// Returns -1 if the requested team is not found.
		/// </summary>
		/// <param name="team"></param>
		/// <returns></returns>
		public int IndexOfTeam(ServerTeam team)
		{
			for (int i = 0; i < teams.Count; i++)
			{
				if (team.TeamName.Equals(teams[i].TeamName))
				{
					return i;
				}
			}
			return -1;
		}

		public ServerTeam GetTeamByIndex(int i)
		{
			return teams[i];
		}
	}

	private CurrentGame()
	{
		LocalPlayer = new LocalPlayer();
		LocalPlayer.name = "Player";
		LocalPlayer.money = 100000000;
		IsGameRunning = false;
		IsInLobby = false;
		IsTaggingPermitted = false;
	}

	public void Awake()
	{
		Ws = (WebsocketImpl)WebsocketImpl.Instance;
		LocalPlayer.name = "Player";
		LocalPlayer.clientID = SystemInfo.deviceUniqueIdentifier;
	}

	public void Connect()
	{
		StartCoroutine(Ws.Connect("ws://localhost:8090/user", GameId, LocalPlayer.clientID));
	}

	public void Clear()
	{
		ClientToken = null;
		GameId = null;
		PasswordUsed = null;
	}

	private IEnumerator SendPlayerLocation()
	{
		while (IsGameRunning)
		{
			if (LocalPlayer!=null && LocalPlayer.location!=null)
			{
				Ws.SendLocation(LocalPlayer.location);
			}
			yield return new WaitForSeconds(1); //todo tweak
		}
	}

	private IEnumerator SendHearthbeats()
	{
		while ((!IsGameRunning) && IsInLobby)
		{
			Ws.SendHearthbeat();
			yield return new WaitForSeconds(45);
		}
	}

	public void StartGame()
	{
		IsGameRunning = true;
		StartCoroutine(SendPlayerLocation());
		string serverGame = Rest.GetGameById(GameId);
		Game parsed = JsonUtility.FromJson<Game>(serverGame);
		gameDetail = parsed;
		PlayerTeam = FindPlayerTeam();
		KnownItems = new Dictionary<string, ServerTradePost.ServerItem>();
	}

	private ServerTeam FindPlayerTeam()
	{
		foreach (ServerTeam gameDetailTeam in gameDetail.teams)
		{
			if (gameDetailTeam.ContainsPlayer(LocalPlayer.clientID))
			{
				return gameDetailTeam;
			}
		}
		return null;
	}

	public void StopGame()
	{
		IsGameRunning = false;
	}

	public void EnterLobby()
	{
		IsInLobby = true;
		StartCoroutine(SendHearthbeats());
	}

	public void LeaveLobby()
	{
		IsInLobby = false;
	}

	public void setNewDistrict(string newDistrictName)
	{
		currentDistrict = newDistrictName;
		foreach (AreaLocation district in gameDetail.districts)
		{
			if (district.name.ToLower().Equals(newDistrictName.ToLower()))
			{
				currentDistrictID = district.id;
				return;
			}
		}
	}
}

