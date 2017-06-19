using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts.dom;
using Assets.Scripts.Domain;
using Assets.Scripts.Network.websocket.messages;
using Stadsspel.Elements;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// A singleton that holds all information regarding the current (or last) game known.
/// </summary>
public class CurrentGame : Singleton<CurrentGame>
{
	public static long timeOffset = new DateTime(1970, 1, 1, 0, 0, 0).Ticks;
#if UNITY_EDITOR || UNITY_EDITOR_WIN
	private const string URL = "ws://localhost:8090/user";
#else
	private const string URL = "wss://stadspelapp-sintniklaas.herokuapp.com/user";
#endif
	//private const string URL = "ws://localhost:8090/user";
	//private const string URL = "wss://stadspelapp-sintniklaas.herokuapp.com/user";
	//private const string URL = "wss://stniklaas-stadsspel.herokuapp.com/user"; todo deprecated


	public WebsocketImpl Ws { get; private set; }
	public string HostingLoginToken { get; set; }
	public string HostedGameId { get; set; }
	public bool isHost { get; set; }
	public IDictionary<string, string> RegisteredGames { get; private set; }

	public string ClientToken { get; set; }
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
	public IDictionary<string, GameObject> PlayerObjects { get; private set; }
	public List<string> TagablePlayers { get; set; }
	public Player UIPlayer { get; set; }
	public ConqueringUpdate lastConqueringUpdate { get; set; }

	public IDictionary<string, ServerTradePost.ServerItem> KnownItems;
	public bool HalfwayPassed { get; set; }
	public bool TenMinuteMark { get; set; }
	public bool LastMinuteMark { get; set; }
	public List<WinningTeamMessage.TeamScore> TeamScores { get; set; }

	[Serializable]
	public class PersistentData{
	}


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
					bankAccount = 0,
					teamName = "TEAM" + (i + 1),
					treasury = 0,
					players = new List<ServerPlayer>(),
					customColor = "#FF" + (i + 1) + "000"
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
				if (team.teamName.Equals(teams[i].teamName))
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
		LocalPlayer.name = "Local Player";
		LocalPlayer.money = 0;
		IsGameRunning = false;
		IsInLobby = false;
		IsTaggingPermitted = false;
		TagablePlayers = new List<string>();
		HalfwayPassed = false;
		TenMinuteMark = false;
		LastMinuteMark = false;
	}

	public void Awake()
	{
		Ws = (WebsocketImpl)WebsocketImpl.Instance;
		LocalPlayer.name = "Speler" + DateTime.Now.Millisecond;
		if (!(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer))
		{
			LocalPlayer.clientID = ""+DateTime.Now.Ticks;
		}
		else
		{
			LocalPlayer.clientID = SystemInfo.deviceUniqueIdentifier;
		}
	}

	public void Connect()
	{
		StartCoroutine(Ws.Connect(URL, GameId, LocalPlayer.clientID));
	}

	public void Clear()
	{
		ClientToken = null;
		GameId = null;
		PasswordUsed = null;
		IsTaggingPermitted = false;
		TagablePlayers = new List<string>();
		HalfwayPassed = false;
		TenMinuteMark = false;
		LastMinuteMark = false;
	}

	private IEnumerator SendPlayerLocation()
	{
		while (IsGameRunning)
		{
			if (LocalPlayer != null && LocalPlayer.location != null)
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
		PlayerObjects = new Dictionary<string, GameObject>();
		IsGameRunning = true;
		StartCoroutine(SendPlayerLocation());
		string serverGame = Rest.GetGameById(GameId);
		Game parsed = JsonUtility.FromJson<Game>(serverGame);
		gameDetail = parsed;
		PlayerTeam = FindPlayerTeam();
		KnownItems = new Dictionary<string, ServerTradePost.ServerItem>();
		TeamScores = null;
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

	public string DistrictNameFromId(string districtId)
	{
		foreach (AreaLocation district in gameDetail.districts)
		{
			if (district.id.Equals(districtId))
			{
				return district.name;
			}
		}
		return null;
	}
	
	public List<ServerPlayer> PlayerList()
	{
		List<ServerPlayer> players = new List<ServerPlayer>();
		foreach (ServerTeam gameDetailTeam in gameDetail.teams)
		{
			foreach (ServerPlayer serverPlayer in gameDetailTeam.players)
			{
				if (serverPlayer.ClientId.Equals(LocalPlayer.ClientId))
				{
					LocalPlayer.name = serverPlayer.name;
				}
				players.Add(serverPlayer);
			}
		}
		return players;
	}

	public int isHeadDistrict(string districtName)
	{
		Dictionary<string, AreaLocation> districts = new Dictionary<string, AreaLocation>();
		foreach (AreaLocation district in gameDetail.districts)
		{
			districts.Add(district.id, district);
		}


		int count = gameDetail.teams.Count;


		for (int index = 0; index < gameDetail.teams.Count; index++)
		{
			if (gameDetail.teams[index].districts.Count >= 1)
			{
				string name = districts[gameDetail.teams[index].districts[0].id].name;
				if (districts[gameDetail.teams[index].districts[0].id].name.Equals(districtName, StringComparison.InvariantCultureIgnoreCase))
				{
					return index;
				}
			}
		}
		return -1;
	}

	public ServerTeam FindTeamByName(string teamName)
	{
		return gameDetail.teams.Find(t => t.teamName.Equals(teamName));
	}

	public ServerTeam getNextTeam(ServerTeam team)
	{
		int index = gameDetail.teams.FindIndex(t => t.teamName.Equals(team.teamName));
		int nextIndex = ++index;
		if (nextIndex >= gameDetail.teams.Count)
		{
			nextIndex = 0;
		}
		return gameDetail.teams[nextIndex];
	}

	public string FindPlayerById(string id)
	{
		ServerTeam team = gameDetail.teams.Find(st => st.ContainsPlayer(id));
		if (team == null) return null;
		ServerPlayer player = team.players.Find(pl => pl.ClientId.Equals(id));
		if (player == null) return null;
		return player.Name;
	}

	public static void FixZ(GameObject o)
	{
		Vector3 pos = o.transform.localPosition;
		pos.z = -4;
		o.transform.localPosition = pos;
	}

	/// <summary>
	/// Returns the name of the local player's main district 
	/// </summary>
	/// <returns>lowered main district name</returns>
	public string GetMainSquare()
	{
		if (PlayerTeam == null) return null;

		AreaLocation mainDistrict = PlayerTeam.districts.Find(d => d.type == AreaLocation.AreaType.DISTRICT_A);
		if (mainDistrict == null) return null;
		return mainDistrict.name.ToLower();
	}
}

