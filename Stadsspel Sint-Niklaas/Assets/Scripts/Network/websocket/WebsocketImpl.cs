using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Domain;
using Assets.Scripts.Network.websocket.messages;
using fastJSON;
using Stadsspel.Districts;
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
		CurrentGame.Instance.StartGame();

		StartCoroutine(NetworkManager.Singleton.RoomManager.ServerCountdownCoroutine(5));//todo set to 10
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
		string name = CurrentGame.Instance.DistrictNameFromId(dn.districtId);
		GameObject district = GameManager.s_Singleton.DistrictManager.GetDistrictByName(name);
		if (district != null)
		{
			CapturableDistrict capturableDistrict = district.GetComponent<CapturableDistrict>();
			if (capturableDistrict != null)
			{
				capturableDistrict.Team = CurrentGame.Instance.GetTeamByName(dn.teamName);
				capturableDistrict.OnTeamChanged();
				district.GetComponentInChildren<CapturePoint>().Team = capturableDistrict.Team;
			}
		}
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
		InfoNotification info = JsonUtility.FromJson<InfoNotification>(message.message);
		//notification containing extra info about a recently passed event (currently only robbery)
		//todo g: display message?
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
		st.districts = new List<AreaLocation>();
		foreach (AreaLocation areaLocation in tn.districts)
		{
			st.districts.Add(areaLocation);
		}
		st.tradePosts = tn.tradeposts;
	}

	protected override void HandleTagPermitted(MessageWrapper message)
	{
		//todo display a notification
		CurrentGame.Instance.IsTaggingPermitted = true;
	}

	protected override void HandleBulkLocation(MessageWrapper message)
	{
		BulkLocationMessage blm = JsonUtility.FromJson<BulkLocationMessage>(message.message);
		CurrentGame.Instance.TagablePlayers = new List<string>();

		IDictionary<string, GameObject> playerObjects = CurrentGame.Instance.PlayerObjects;
		foreach (GameObject playerObj in playerObjects.Values)
		{
			playerObj.SetActive(false);
		}

		foreach (KeyValuePair<string, Point> playerLocation in blm.Taggable)
		{
			//todo these players are taggable and need to be drawn on screen
			if (playerObjects.ContainsKey(playerLocation.Key))
			{
				GameObject go = playerObjects[playerLocation.Key];
				go.SetActive(true);
				Coordinates coordinates = new Coordinates(playerLocation.Value.latitude, playerLocation.Value.longitude, 0);
				go.transform.localPosition = coordinates.convertCoordinateToVector(0);
			}
		}

		foreach (KeyValuePair<string, Point> playerLocation in blm.Locations)
		{
			//todo these players only need to be drawn on screen
			if (playerObjects.ContainsKey(playerLocation.Key))
			{
				GameObject go = playerObjects[playerLocation.Key];
				go.SetActive(true);
				Coordinates coordinates = new Coordinates(playerLocation.Value.latitude, playerLocation.Value.longitude, 1);

				//move the object
				//StartCoroutine(MoveOverSeconds(go, coordinates.convertCoordinateToVector(0), 0.75f));
				go.transform.localPosition = coordinates.convertCoordinateToVector(0);
			}
		}


		if (blm.Taggable.Count > 0)
		{
			CurrentGame.Instance.TagablePlayers = new List<string>(blm.Taggable.Keys);
			CurrentGame.Instance.UIPlayer.OnTaggablePlayers();
			//todo enable tagging of players
		}
	}

	protected override void HandleLobbyUpdate(MessageWrapper message)
	{
		LobbyUpdate lu = JsonUtility.FromJson<LobbyUpdate>(message.message);
		CurrentGame.Instance.gameDetail.teams = lu.teams;
		CurrentGame.Instance.isHost = lu.isHost;
		NetworkManager.Singleton.RoomManager.OnLobbyLoad();
	}

	protected override void HandleConquerUpdate(MessageWrapper message)
	{
		ConqueringUpdate cu = JsonUtility.FromJson<ConqueringUpdate>(message.message);
		CurrentGame.Instance.lastConqueringUpdate = cu;
	}

	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
	{
		float elapsedTime = 0;
		Vector3 startingPos = objectToMove.transform.position;
		while (elapsedTime < seconds)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		objectToMove.transform.position = end;
	}
}