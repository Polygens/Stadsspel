using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton that holds all information regarding the current (or last) game known.
/// </summary>
public class CurrentGame
{
	private static CurrentGame _instance;

	public static CurrentGame GetInstance()
	{
		return _instance ?? (_instance = new CurrentGame());
	}

	public static void NewInstance()
	{
		_instance = new CurrentGame();
	}



	public WebsocketImpl Ws { get; private set; }
	public string ClientToken { get; set; }
	public string GameId { get; set; }
	public string PasswordUsed { get; set; }

	private  CurrentGame()
	{
		Ws = new WebsocketImpl();
	}

}
