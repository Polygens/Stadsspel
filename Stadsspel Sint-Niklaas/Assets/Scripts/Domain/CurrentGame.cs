using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton that holds all information regarding the current (or last) game known.
/// </summary>
public class CurrentGame : Singleton<CurrentGame>
{
	public WebsocketImpl Ws { get; private set; }
	public string ClientToken { get; set; }
	public string GameId { get; set; }
	public string PasswordUsed { get; set; }

	private  CurrentGame(){}

	public void Awake()
	{
		Ws = (WebsocketImpl)WebsocketImpl.Instance;
	}

	public void Connect()
	{
		StartCoroutine(Ws.Connect("ws://localhost:8090/user", GameId, SystemInfo.deviceUniqueIdentifier));
	}

	public void Clear()
	{
		ClientToken = null;
		GameId = null;
		PasswordUsed = null;
	}
}
