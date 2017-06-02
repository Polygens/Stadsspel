using System;
using System.Collections.Generic;
using fastJSON;
using UnityEngine;
using Stadsspel.Networking;
using Random = UnityEngine.Random;

public class WebsocketImpl : WebsocketContainer
{

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	protected override void HandleGameStart(MessageWrapper message)
	{
		StartCoroutine(NetworkManager.Singleton.RoomManager.ServerCountdownCoroutine(5));//todo set to 10
		CurrentGame.Instance.StartGame();
		Debug.Log("GAME STARTED");
	}

	protected override void HandleEvent(MessageWrapper message)
	{
		throw new System.NotImplementedException();
	}

	protected override void HandleDistrictNotification(MessageWrapper message)
	{
		throw new System.NotImplementedException();
	}

	protected override void HandleGameStop(MessageWrapper message)
	{
		//todo display correct sceens
		CurrentGame.Instance.StopGame();
		Debug.Log("GAME STOPPED");
	}

	protected override void HandleInfoNotification(MessageWrapper message)
	{
		throw new System.NotImplementedException();
	}

	protected override void HandlePlayerNotification(MessageWrapper message)
	{
		JSONParameters jsonParameters = new JSONParameters();
		jsonParameters.UsingGlobalTypes = false;
		jsonParameters.UseExtensions = false;
		PlayerNotification pn = JSON.ToObject<PlayerNotification>(message.message, jsonParameters);

		//todo implment further
	}

	protected override void HandleTagNotification(MessageWrapper message)
	{
		throw new System.NotImplementedException();
	}

	protected override void HandleErrorException(MessageWrapper message)
	{
		throw new System.NotImplementedException();
	}

	protected override void HandleWinningTeam(MessageWrapper message)
	{
		throw new System.NotImplementedException();
	}

	protected override void HandleTreasuriesOpen(MessageWrapper message)
	{
		throw new System.NotImplementedException();
	}

	protected override void HandleTreasuriesClose(MessageWrapper message)
	{
		throw new System.NotImplementedException();
	}

	protected override void HandleTeamNotification(MessageWrapper message)
	{
		throw new System.NotImplementedException();
	}

	protected override void HandleTagPermitted(MessageWrapper message)
	{
		//todo display a notification
		CurrentGame.Instance.IsTaggingPermitted = true;
	}

	protected override void HandleBulkLocation(MessageWrapper message)
	{
		BulkLocationMessage blm = JsonUtility.FromJson<BulkLocationMessage>(message.message);

		foreach (KeyValuePair<string, Point> playerLocation in blm.Taggable)
		{
			//todo these players are taggable and need to be drawn on screen
		}

		foreach (KeyValuePair<string, Point> playerLocation in blm.Locations)
		{
			//todo these players only need to be drawn on screen
		}

		if (blm.Taggable.Count > 0)
		{
			//todo enable tagging of players
		}
	}
}