using System;
using System.Collections.Generic;
using Assets.Scripts.Domain;
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
		Debug.Log("#############################################");
		Debug.Log("#############################################");
		Debug.Log("################### EVENT ###################");
		Debug.Log("#############################################");
		Debug.Log("#############################################");
		throw new System.NotImplementedException();
	}

	protected override void HandleDistrictNotification(MessageWrapper message)
	{
		DistrictNotification dn = JsonUtility.FromJson<DistrictNotification>(message.message);
		//Contains a district ID and team name
		//todo map id to a district and change it's team
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
		LocalPlayer lp = CurrentGame.Instance.LocalPlayer;
		lp.money = pn.money;
		lp.legalItems = pn.LegalItems;
		lp.illegalItems = pn.IllegalItems;
	}

	protected override void HandleTagNotification(MessageWrapper message)
	{
		TagNotification tn = JsonUtility.FromJson<TagNotification>(message.message);
		if (tn.taggedBy.Equals(CurrentGame.Instance.LocalPlayer.ClientId))
		{
			//todo Player tagged someone
		}else{
			//todo Player got tagged
		}
	}

	protected override void HandleErrorException(MessageWrapper message)
	{
		ErrorExceptionMessage eem = JsonUtility.FromJson<ErrorExceptionMessage>(message.message);
		Debug.Log(eem.message);
		Debug.Log(eem.exceptionClass);
		Debug.Log(eem.cause);
	}

	protected override void HandleWinningTeam(MessageWrapper message)
	{
		//todo display winning team, will be expanded to include statistics
	}

	protected override void HandleTreasuriesOpen(MessageWrapper message)
	{
		//todo display notification of treasuries opening
	}

	protected override void HandleTreasuriesClose(MessageWrapper message)
	{
		//todo display notification of treasuries closing
	}

	protected override void HandleTeamNotification(MessageWrapper message)
	{
		TeamNotification tn = JsonUtility.FromJson<TeamNotification>(message.message);
		ServerTeam st = CurrentGame.Instance.PlayerTeam;
		st.bankAccount = tn.bankAccount;
		st.treasury = tn.treasury;
		st.districts = new List<string>();
		foreach (AreaLocation areaLocation in tn.districts)
		{
			st.districts.Add(areaLocation.name);
		}
		st.tradeposts = tn.tradeposts;
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