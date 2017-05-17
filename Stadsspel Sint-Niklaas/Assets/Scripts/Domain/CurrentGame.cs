using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Domain;
using Stadsspel.Elements;
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

	[Serializable]
	public class Game
	{
		private string id;
		private string roomName;
		private List<AreaLocation> districts;
		private List<AreaLocation> markets;
		private List<PointLocation> tradePosts;
		private List<PointLocation> banks;
		private List<ServerTeam> teams;
		private int maxPlayersPerTeam;
		private int maxTeams;
	}

	private CurrentGame()
	{
		LocalPlayer = new LocalPlayer();
	}

	public void Awake()
	{
		Ws = (WebsocketImpl)WebsocketImpl.Instance;
		LocalPlayer = new LocalPlayer();
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
}
