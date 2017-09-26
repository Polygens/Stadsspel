using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using Assets.scripts.dom;
using Assets.Scripts.Domain;
using Assets.Scripts.Network.websocket.messages;
using Stadsspel.Elements;
using Stadsspel.Networking;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A singleton that holds all information regarding the current (or last) game known.
/// </summary>
public class CurrentGame : Singleton<CurrentGame>
{
	public static long timeOffset = new DateTime(1970, 1, 1, 0, 0, 0).Ticks;
	public static string dataPath;
	public static string previousGamePath;


	//private const string URL = "ws://localhost:8090/user";							//LOCAL	server
	//private const string URL = "wss://stadsspelapp.herokuapp.com/user";				//LIVE	server
	private const string URL = "wss://stadspelapp-sintniklaas.herokuapp.com/user";		//DEV	server
	

	private static PersistentData persistentData = null;

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
	public GameObject ReconnectPanel;

	[Serializable]
	public class PersistentData
	{
		public string ClientToken;
		public string GameId;
		public string PasswordUsed;

		public void Clear()
		{
			ClientToken = "";
			GameId = "";
			PasswordUsed = "";
		}
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
		dataPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "stadspelapp";
		previousGamePath = dataPath + Path.DirectorySeparatorChar + "previousGame";

		Ws = (WebsocketImpl)WebsocketImpl.Instance;
		LocalPlayer.name = "Speler" + DateTime.Now.Millisecond;


		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			LocalPlayer.clientID = SystemInfo.deviceUniqueIdentifier;
		} else
		{
			LocalPlayer.clientID = "" + DateTime.Now.Ticks;
			//LocalPlayer.clientID = SystemInfo.deviceUniqueIdentifier; //todo this is for rejoin debug, disable pls
		}

		LoadPersistentData();
		//CheckPersistentData(); too early, not all objects loaded?
	}

	public void Start()
	{
		CheckPersistentData();
	}

	private void CheckPersistentData()
	{
		Debug.Log("CHECKING PERSISTENT DATA");
		Debug.Log(dataPath);
		Debug.Log(previousGamePath);
		Debug.Log(persistentData.GameId);
		if (persistentData.GameId == null || persistentData.GameId.Equals(""))
		{
			persistentData.Clear();
			SavePersistentData();
			return;
		}
		string state = Rest.GetGameState(persistentData.GameId);
		if (state.Equals("STAGED") || state.Equals("RUNNING"))
		{
			//todo show "trying to reconnect" popup

			if (ReconnectPanel != null)
			{
				ReconnectPanel.SetActive(true);
			}

			//hot join game now
			ClientToken = persistentData.ClientToken;
			GameId = persistentData.GameId;
			PasswordUsed = persistentData.PasswordUsed;

			StartCoroutine(HotJoin(state));
		}
		else
		{
			persistentData.Clear();
			SavePersistentData();
		}
	}

	private void SavePersistentData()
	{
		Debug.Log("SAVING PERSISTENT DATA");
		Debug.Log(GameId);
		Debug.Log(persistentData.GameId);
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		//directory exists at this point

		if (File.Exists(previousGamePath))
		{
			File.Delete(previousGamePath);
		}

		using (StreamWriter sw = File.CreateText(previousGamePath))
		{
			sw.WriteLine(JsonUtility.ToJson(persistentData));
		}
		//File.Encrypt(previousGamePath);
	}

	private void LoadPersistentData()
	{
		Debug.Log("LOADING PERSISTENT DATA");
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		//directory exists at this point

		if (!File.Exists(previousGamePath))
		{
			persistentData = new PersistentData();
		} else
		{
			//File.Decrypt(previousGamePath);
			using (StreamReader fs = File.OpenText(previousGamePath))
			{
				while (!fs.EndOfStream)
				{
					try
					{
						String line = fs.ReadLine();
						PersistentData pd = JsonUtility.FromJson<PersistentData>(line);
						persistentData = pd;
					} catch (Exception e)
					{
						Console.WriteLine(e);
					}
				}
			}
			//File.Encrypt(previousGamePath);
		}
	}

	public void Connect()
	{
		StartCoroutine(Ws.Connect(URL, GameId, LocalPlayer.clientID));
	}

	public void StopReconnect()
	{
		ReconnectPanel.SetActive(false);
		StopGame();
		//
		//
		//
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
		Ws.Clear();
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

	private IEnumerator HotJoin(string state)
	{
		Debug.Log("RECONNECTING");
		yield return new WaitForSeconds(10);
		NetworkManager.Singleton.CreateJoinRoomManager.ShowLobby();
		//connect websocket and open required screens for staged game
		Connect();
		NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(true);
		NetworkManager.Singleton.LobbyManager.EnableDisableMenu(false);
		//NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(false);
		NetworkManager.Singleton.RoomManager.EnableDisableMenu(true);

		if (state.Equals("RUNNING"))
		{
			StartGame();

			StartCoroutine(NetworkManager.Singleton.RoomManager.ServerCountdownCoroutine(10));
			Debug.Log("GAME HOT JOINED");
			if(ReconnectPanel != null) if (ReconnectPanel.activeSelf) ReconnectPanel.SetActive(false);
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
				if (districts[gameDetail.teams[index].districts[0].id].name
					.Equals(districtName, StringComparison.InvariantCultureIgnoreCase))
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

	public void UpdatePersistentData()
	{
		persistentData.GameId = GameId;
		persistentData.ClientToken = ClientToken;
		persistentData.PasswordUsed = PasswordUsed;
		SavePersistentData();
	}

	public void ClearPersistentData()
	{
		persistentData.Clear();
		SavePersistentData();
	}
}
