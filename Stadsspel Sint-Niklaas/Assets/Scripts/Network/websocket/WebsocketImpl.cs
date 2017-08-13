using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Domain;
using Assets.Scripts.Network.websocket.messages;
using fastJSON;
using Stadsspel.Districts;
using UnityEngine;
using Stadsspel.Networking;

public class WebsocketImpl : WebsocketContainer
{

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	protected override void HandleGameStart(MessageWrapper message)
	{
		CurrentGame.Instance.StartGame();

		StartCoroutine(NetworkManager.Singleton.RoomManager.ServerCountdownCoroutine(10));
		Debug.Log("GAME STARTED");
	}

	protected override void HandleEvent(MessageWrapper message)
	{
		Debug.Log("#############################################");//I think a player should only send events not receive
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
				capturableDistrict.Team = CurrentGame.Instance.FindTeamByName(dn.teamName);
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
		Debug.Log(message.message);
		string text=info.GameEventType.ToString();
		//notification containing extra info about a recently passed event (currently only robbery)
		switch (info.GameEventType)
		{
			case GameEventType.BANK_DEPOSIT:
				break;
			case GameEventType.BANK_WITHDRAWAL:
				break;
			case GameEventType.PLAYER_TAGGED:
				break;
			case GameEventType.DISTRICT_CONQUERED:
				break;
			case GameEventType.TRADEPOST_LEGAL_SALE:
				break;
			case GameEventType.TRADEPOST_LEGAL_PURCHASE:
				break;
			case GameEventType.TRADEPOST_ILLEGAL_SALE:
				break;
			case GameEventType.TRADEPOST_ILLEGAL_PURCHASE:
				break;
			case GameEventType.TREASURY_WITHDRAWAL:
				break;
			case GameEventType.TRADEPOST_ALL_SALE:
				break;
			case GameEventType.TREASURY_ROBBERY:
				text = "Je schatkist is bestolen door " + info.by;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		InGameUIManager.s_Singleton.LogUI.AddToLog(text, new object[] { });
	}

	protected override void HandlePlayerNotification(MessageWrapper message)
	{
		/*
		JSONParameters jsonParameters = new JSONParameters();
		jsonParameters.UsingGlobalTypes = false;
		jsonParameters.UseExtensions = false;
		*/
		PlayerNotification pn = JsonUtility.FromJson<PlayerNotification>(message.message);
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
			InGameUIManager.s_Singleton.LogUI.AddToLog("Je hebt iemand getikt", new object[] { });
		} else
		{
			InGameUIManager.s_Singleton.LogUI.AddToLog("Je bent getikt", new object[] { });
		}
	}

	protected override void HandleErrorException(MessageWrapper message)
	{
		ErrorExceptionMessage eem = JsonUtility.FromJson<ErrorExceptionMessage>(message.message);
		Debug.Log(eem.message);
		Debug.Log(eem.exceptionClass);
		Debug.Log(eem.cause);
		InGameUIManager.s_Singleton.LogUI.AddToLog("ERROR: "+eem.message, new object[] { },silent:true);
		InGameUIManager.s_Singleton.LogUI.AddToLog("ERROR: "+eem.exceptionClass, new object[] { }, silent: true);
		InGameUIManager.s_Singleton.LogUI.AddToLog("ERROR: "+eem.cause, new object[] { }, silent: true);
	}

	protected override void HandleWinningTeam(MessageWrapper message)
	{
		WinningTeamMessage winningTeamMessage = JsonUtility.FromJson<WinningTeamMessage>(message.message);
		CurrentGame.Instance.TeamScores = winningTeamMessage.scoreList;
	}

	protected override void HandleTeamNotification(MessageWrapper message)
	{
		bool treasuryTax = false,treasuryRob = false, bankUpdateDep = false,bankUpdateWith = false, districtUpdate = false, tradepostUpdate = false;
		TeamNotification tn = JsonUtility.FromJson<TeamNotification>(message.message);
		ServerTeam st = CurrentGame.Instance.PlayerTeam;
		st.TotalPlayerMoney = tn.totalPlayerMoney;

		if (st.bankAccount < (tn.bankAccount - 0.0001))
		{
			st.bankAccount = tn.bankAccount;
			bankUpdateDep = true;
		} else if(st.bankAccount > (tn.bankAccount + 0.0001))
		{
			st.bankAccount = tn.bankAccount;
			bankUpdateWith = true;
		}


		if (st.treasury > (tn.treasury + 0.0001))
		{
			st.treasury = tn.treasury;
			treasuryRob = true;
		}
		else if(st.treasury < (tn.treasury - 0.0001))
		{
			st.treasury = tn.treasury;
			treasuryTax = true;

		}

		//todo check for differences
		st.districts = new List<AreaLocation>();
		foreach (AreaLocation areaLocation in tn.districts)
		{
			st.districts.Add(areaLocation);
		}

		//todo check for differences
		st.tradePosts = tn.tradeposts;
		

		if (treasuryTax)
		{
			InGameUIManager.s_Singleton.LogUI.AddToLog("Er zijn belastingen binnen gekomen", new object[] { });
		}

		if (treasuryRob)
		{
			InGameUIManager.s_Singleton.LogUI.AddToLog("Er is geld uit je schatkist genomen", new object[] { });
		}
		if (bankUpdateDep)
		{
			InGameUIManager.s_Singleton.LogUI.AddToLog("Er is geld op de bank gezet", new object[] { });
		}
		if (bankUpdateWith)
		{
			InGameUIManager.s_Singleton.LogUI.AddToLog("Er is geld van de bank afgehaald", new object[] { });
		}
		if (districtUpdate)
		{
			InGameUIManager.s_Singleton.LogUI.AddToLog("WIJK UPDATE", new object[] { });
		}
		if (tradepostUpdate)
		{
			InGameUIManager.s_Singleton.LogUI.AddToLog("HANDELSPOST UPDATE", new object[] { });
		}
	}

	protected override void HandleTagPermitted(MessageWrapper message)
	{
		CurrentGame.Instance.IsTaggingPermitted = true;
		InGameUIManager.s_Singleton.LogUI.AddToLog("Tikken is nu toegestaan", new object[] { });
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
				CurrentGame.FixZ(go);
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
				CurrentGame.FixZ(go);
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
		if (cu.isConqueringTeam && !cu.isDraw && cu.progress >= 1.0)
		{
			InGameUIManager.s_Singleton.LogUI.AddToLog("Wijk overgenomen", new object[] { });
		}
	}

	protected override void HandlePlayerKicked()
	{
		NetworkManager.Singleton.RoomManager.EnableDisableMenu(false);
		NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
		CurrentGame.Instance.Clear();
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