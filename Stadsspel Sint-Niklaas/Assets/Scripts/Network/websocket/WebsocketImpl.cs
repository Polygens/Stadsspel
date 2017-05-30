using UnityEngine;
using System.Collections;
using Stadsspel.Networking;

public class WebsocketImpl : WebsocketContainer
{

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	protected override void HandleGameStart(MessageWrapper message)
	{
		StartCoroutine(NetworkManager.Singleton.RoomManager.ServerCountdownCoroutine(0));
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
		throw new System.NotImplementedException();
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
		throw new System.NotImplementedException();
	}

	protected override void HandleBulkLocation(MessageWrapper message)
	{
		throw new System.NotImplementedException();
	}
}
