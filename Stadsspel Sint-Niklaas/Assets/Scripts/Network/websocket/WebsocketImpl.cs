using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Scripts.Domain;
using Assets.Scripts.Network.websocket.messages;
using fastJSON;
using Stadsspel.Districts;
using UnityEngine;
using Stadsspel.Networking;

public class WebsocketImpl : Singleton<WebsocketImpl>
{
	private WebSocket ws;
	private Thread listeningThread;
	private bool stopThread = true;
	private readonly Queue<string> messageBuffer;
	private string gameID, clientID;
	//private string url;
	//private ConcurrentQueue<MessageWrapper> _inbox;
	private Queue<MessageWrapper> _inbox;

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}


	private void Update()
	{
		while (_inbox.Count > 0)
		{
			/*todo concurrency
			MessageWrapper messageWrapper;
			while (!_inbox.TryDequeue(out messageWrapper));
			HandleMessage(messageWrapper);
			*/
			HandleMessage(_inbox.Dequeue());
		}

		//send a message from the buffer
		Send();
	}

	public WebsocketImpl()
	{
		//_inbox = new ConcurrentQueue<MessageWrapper>();todo concurrency
		_inbox = new Queue<MessageWrapper>();
		//private constructor for singleton
		messageBuffer = new Queue<string>();
	}

	public IEnumerator Connect(string url, string gameID, string clientID)
	{
		Debug.Log("DEBUG");
		if (listeningThread != null && listeningThread.ThreadState != ThreadState.Stopped)
		{
			Debug.Log("Stop previous");
			stopThread = true;
			yield return new WaitUntil(() => listeningThread.ThreadState == ThreadState.Stopped);
		}

		if (ws == null)
		{
			ws = new WebSocket(new Uri(url));
		} else
		{
			//ws.Close();
		}

		this.clientID = clientID;
		this.gameID = gameID;
		//this.url = url;
		//_inbox = new ConcurrentQueue<MessageWrapper>(); todo concurrency
		_inbox = new Queue<MessageWrapper>();
		Debug.Log("CONNECT");
		bool connected = false;
		while (!connected)
		{
			yield return StartCoroutine(ws.Connect());
			if (ws.error != null)
			{//todo limit the tries
				Debug.Log("ERROR: " + ws.error);
			} else
			{
				Debug.Log("CONNECTED");
				connected = true;
			}
		}

		listeningThread = new Thread(ListeningRun);
		listeningThread.Start();

		Debug.Log("SEND");
		//send hearthbeat to provide server with player info
		SendHearthbeat();
	}

	public IEnumerator ReConnectCurrent()
	{
		Debug.Log("DEBUG");
		if (listeningThread != null && listeningThread.ThreadState != ThreadState.Stopped)
		{
			Debug.Log("Stop previous");
			stopThread = true;
			yield return new WaitUntil(() => listeningThread.ThreadState == ThreadState.Stopped);
		}
		if (ws.IsConnected)
		{
			ws.Close();
		}

		//_inbox = new ConcurrentQueue<MessageWrapper>(); todo concurrency
		_inbox = new Queue<MessageWrapper>();
		Debug.Log("RECONNECT");
		//bool connected = false;
		while (ws.IsConnected)
		{
			yield return StartCoroutine(ws.Connect());
			if (ws.error != null)
			{//todo limit the tries
				Debug.Log("ERROR: " + ws.error);
			} else
			{
				Debug.Log("CONNECTED");
				//connected = true;
			}
		}

		listeningThread = new Thread(ListeningRun);
		listeningThread.Start();

		Debug.Log("SEND");
		//send hearthbeat to provide server with player info
		SendHearthbeat();
	}

	private void ListeningRun()
	{
		stopThread = false;
		int consecutiveErrors = 0;
		while (!stopThread)
		{
			if (ws.IsConnected)
			{
				string reply = ws.RecvString();
				if (reply != null)
				{
					consecutiveErrors = 0;
					MessageWrapper mw = JsonUtility.FromJson<MessageWrapper>(reply);
					_inbox.Enqueue(mw);
				}
				if (ws.error != null)
				{
					consecutiveErrors++;
					if (consecutiveErrors >= 5)
					{
						Debug.LogError("G Error: " + ws.error);
						stopThread = true;
					}
				}
			}
		}
	}

	public void Clear()
	{
		stopThread = true;
		if (ws != null && ws.IsConnected)
		{
			ws.Close();
		}
	}

	private void HandleMessage(MessageWrapper message)
	{
		if (!message.gameID.Equals(CurrentGame.Instance.GameId)) return; //if not right game do nothing todo throw error or something also check if no error will occur with this line here
		if (NetworkManager.Singleton.ConnectingManager != null)
		{
			NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(false); //todo move this line somewhere better
		}
		switch (message.MessageType)
		{
			case GameMessageType.BULK_LOCATION:
				HandleBulkLocation(message);
				break;
			case GameMessageType.DISTRICT_NOTIFICATION:
				HandleDistrictNotification(message);
				break;
			case GameMessageType.EVENT:
				HandleEvent(message);
				break;
			case GameMessageType.GAME_START:
				HandleGameStart(message);
				break;
			case GameMessageType.GAME_STOP:
				HandleGameStop(message);
				break;
			case GameMessageType.INFO_NOTIFICATION:
				HandleInfoNotification(message);
				break;
			case GameMessageType.PLAYER_NOTIFICATION:
				HandlePlayerNotification(message);
				break;
			case GameMessageType.TAG_NOTIFICATION:
				HandleTagNotification(message);
				break;
			case GameMessageType.TAG_PERMITTED:
				HandleTagPermitted(message);
				break;
			case GameMessageType.TEAM_NOTIFICATION:
				HandleTeamNotification(message);
				break;
			case GameMessageType.WINNING_TEAM:
				HandleWinningTeam(message);
				break;
			case GameMessageType.ERROR_EXCEPTION:
				HandleErrorException(message);
				break;
			case GameMessageType.LOBBY_UPDATE:
				HandleLobbyUpdate(message);
				break;
			case GameMessageType.CONQUERING_UPDATE:
				HandleConquerUpdate(message);
				break;
			case GameMessageType.PLAYER_KICKED:
				HandlePlayerKicked();
				break;
			default:
				Debug.Log("Message is not of a type we should catch: " + message.MessageType.ToString());
				break;
		}
	}

	/// <summary>
	/// Sends a message from the buffer.
	/// If the buffer is empty or no connection is established it will not send.
	/// </summary>
	private void Send()
	{
		if (messageBuffer.Count <= 0) return;
		if (ws == null) return;
		try
		{
			if (!ws.IsConnected)
			{
				StartCoroutine(ReConnectCurrent());
			} else
			{
				var message = messageBuffer.Peek();
				//Debug.Log(message);
				//todo compress message
				ws.SendString(message);
				messageBuffer.Dequeue();
			}
		} catch (Exception e)
		{
			Debug.Log("Error during sening of message, buffering");
			Debug.Log(e);
		}
	}

	private void Send(GameMessageType type, string innerMessage)
	{
		string message = JsonUtility.ToJson(new MessageWrapper(type, innerMessage, gameID, clientId: clientID, token: CurrentGame.Instance.ClientToken));
		messageBuffer.Enqueue(message);
		Send();
	}

	public void SendHearthbeat()
	{
		Send(GameMessageType.HEARTBEAT, "");
	}

	public void SendLocation(Point location)
	{
		LocationMessage lm = new LocationMessage(location.latitude, location.longitude);
		string innerMessage = JsonUtility.ToJson(lm);
		Send(GameMessageType.LOCATION, innerMessage);
	}

	/// <summary>
	/// Sends an event to the server constructed from the given parameters
	/// </summary>
	/// <param name="type">Event type</param>
	/// <param name="players">All involved players excluding this player</param>
	/// <param name="moneyTransferred">The amount of money transferred to or from the location</param>
	/// <param name="items">The items bought or sold</param>
	/// <param name="locationID">The location or district of the event</param>
	private void SendEvent(GameEventType type, List<string> players = null, double moneyTransferred = 0,
		IDictionary<string, int> items = null, string locationID = "")
	{
		if (players == null)
		{
			players = new List<string>();
		}
		if (items == null)
		{
			items = new Dictionary<string, int>();
		}
		GameEventMessage gem = new GameEventMessage(type, players, moneyTransferred, items, locationID);
		//string innerMessage = JsonUtility.ToJson(gem);
		JSONParameters jsonParameters = new JSONParameters();
		jsonParameters.UsingGlobalTypes = false;
		jsonParameters.UseExtensions = false;
		string innerMessage = JSON.ToJSON(gem, jsonParameters);
		Send(GameMessageType.EVENT, innerMessage);
	}

	public void SendTag(List<string> taggedPlayers, string locationId)
	{
		SendEvent(GameEventType.PLAYER_TAGGED, players: taggedPlayers, locationID: locationId);
	}

	public void SendBankDeposit(double moneyTransferred, string locationID)
	{
		SendEvent(GameEventType.BANK_DEPOSIT, moneyTransferred: moneyTransferred, locationID: locationID);
	}

	public void SendBankWithdrawal(double moneyTransferred, string locationID)
	{
		SendEvent(GameEventType.BANK_WITHDRAWAL, moneyTransferred: moneyTransferred, locationID: locationID);
	}

	public void SendTreasuryWithdrawal(double moneyTransferred, string locationID)
	{
		SendEvent(GameEventType.TREASURY_WITHDRAWAL, moneyTransferred: moneyTransferred, locationID: locationID);
	}

	public void SendTreasuryRobbery(string locationID)
	{
		SendEvent(GameEventType.TREASURY_ROBBERY, locationID: locationID);
	}

	public void SendTradepostLegalPurchase(IDictionary<string, int> items, string locationID)
	{
		SendEvent(GameEventType.TRADEPOST_LEGAL_PURCHASE, items: items, locationID: locationID);
	}

	public void SendTradepostLegalSale(IDictionary<string, int> items, string locationID)
	{
		SendEvent(GameEventType.TRADEPOST_LEGAL_SALE, items: items, locationID: locationID);
	}

	public void SendTradepostIllegalPurchase(IDictionary<string, int> items, string locationID)
	{
		SendEvent(GameEventType.TRADEPOST_ILLEGAL_PURCHASE, items: items, locationID: locationID);
	}

	public void SendTradepostIllegalSale(IDictionary<string, int> items, string locationID)
	{
		SendEvent(GameEventType.TRADEPOST_ILLEGAL_SALE, items: items, locationID: locationID);
	}

	/// <summary>
	/// Sells all items to the market
	/// todo neither of the parameters get used by the code in server
	/// </summary>
	/// <param name="items"></param>
	/// <param name="locationID"></param>
	public void SendTradepostAllSale(IDictionary<string, int> items, string locationID)
	{
		SendEvent(GameEventType.TRADEPOST_ALL_SALE, items: items, locationID: locationID);
	}

	public void SendDistrictConquered(string locationId)
	{
		SendEvent(GameEventType.DISTRICT_CONQUERED, locationID: locationId);
	}

	public void SendConquerStart(string locationId)
	{
		ConquerMessage cm = new ConquerMessage();
		cm.LocationID = locationId;
		Send(GameMessageType.CONQUERING_START, JsonUtility.ToJson(cm));
	}

	public void SendConquerEnd(string locationId)
	{
		ConquerMessage cm = new ConquerMessage();
		cm.LocationID = locationId;
		Send(GameMessageType.CONQUERING_END, JsonUtility.ToJson(cm));
	}

	public void SendPlayerNameUpdate(string newName)
	{
		Send(GameMessageType.PLAYER_UPDATE_NAME, newName);
	}

	public void SendPlayerTeamUpdate(string newTeam)
	{
		Send(GameMessageType.PLAYER_UPDATE_TEAM, newTeam);
	}


	public new void OnDestroy()
	{
		base.OnDestroy();
		stopThread = true;
	}

	protected void HandleGameStart(MessageWrapper message)
	{
		CurrentGame.Instance.StartGame();

		StartCoroutine(NetworkManager.Singleton.RoomManager.ServerCountdownCoroutine(10));
		Debug.Log("GAME STARTED");
	}

	protected void HandleEvent(MessageWrapper message)
	{
		Debug.Log("#############################################");//I think a player should only send events not receive
		Debug.Log("#############################################");
		Debug.Log("################### EVENT ###################");
		Debug.Log("#############################################");
		Debug.Log("#############################################");
		throw new System.NotImplementedException();
	}

	protected void HandleDistrictNotification(MessageWrapper message)
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

	protected void HandleGameStop(MessageWrapper message)
	{
		//todo display correct sceens
		CurrentGame.Instance.StopGame();
		Debug.Log("GAME STOPPED");
	}

	protected void HandleInfoNotification(MessageWrapper message)
	{
		InfoNotification info = JsonUtility.FromJson<InfoNotification>(message.message);
		Debug.Log(message.message);
		string text = info.GameEventType.ToString();
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
				ServerTeam st = CurrentGame.Instance.FindTeamByName(info.by);
				string name = "team " + CurrentGame.Instance.ColorNames[st.customColor];
				//text = "Je schatkist is bestolen door " + info.by;
				text = "Je schatkist is bestolen door " + name;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		InGameUIManager.s_Singleton.LogUI.AddToLog(text, new object[] { });
	}

	protected void HandlePlayerNotification(MessageWrapper message)
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
		lp.visitedTradeposts = pn.VisitedTradeposts;


		Debug.Log(pn.IllegalItems.Count);
		Debug.Log(pn.LegalItems.Count);
	}

	protected void HandleTagNotification(MessageWrapper message)
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

	protected void HandleErrorException(MessageWrapper message)
	{
		ErrorExceptionMessage eem = JsonUtility.FromJson<ErrorExceptionMessage>(message.message);
		Debug.Log(eem.message);
		Debug.Log(eem.exceptionClass);
		Debug.Log(eem.cause);
		InGameUIManager.s_Singleton.LogUI.AddToLog("ERROR: " + eem.message, new object[] { }, silent: true);
		InGameUIManager.s_Singleton.LogUI.AddToLog("ERROR: " + eem.exceptionClass, new object[] { }, silent: true);
		InGameUIManager.s_Singleton.LogUI.AddToLog("ERROR: " + eem.cause, new object[] { }, silent: true);
	}

	protected void HandleWinningTeam(MessageWrapper message)
	{
		WinningTeamMessage winningTeamMessage = JsonUtility.FromJson<WinningTeamMessage>(message.message);
		CurrentGame.Instance.TeamScores = winningTeamMessage.scoreList;
	}

	protected void HandleTeamNotification(MessageWrapper message)
	{
		bool treasuryTax = false, treasuryRob = false, bankUpdateDep = false, bankUpdateWith = false, districtUpdate = false, tradepostUpdate = false;
		TeamNotification tn = JsonUtility.FromJson<TeamNotification>(message.message);
		ServerTeam st = CurrentGame.Instance.PlayerTeam;
		st.TotalPlayerMoney = tn.totalPlayerMoney;
		st.visitedTadeposts = tn.VisitedTradeposts;//todo validate

		if (st.bankAccount < (tn.bankAccount - 0.0001))
		{
			st.bankAccount = tn.bankAccount;
			bankUpdateDep = true;
		} else if (st.bankAccount > (tn.bankAccount + 0.0001))
		{
			st.bankAccount = tn.bankAccount;
			bankUpdateWith = true;
		}


		if (st.treasury > (tn.treasury + 0.0001))
		{
			st.treasury = tn.treasury;
			treasuryRob = true;
		} else if (st.treasury < (tn.treasury - 0.0001))
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

	protected void HandleTagPermitted(MessageWrapper message)
	{
		CurrentGame.Instance.IsTaggingPermitted = true;
		InGameUIManager.s_Singleton.LogUI.AddToLog("Tikken is nu toegestaan", new object[] { });
	}

	protected void HandleBulkLocation(MessageWrapper message)
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

	protected void HandleLobbyUpdate(MessageWrapper message)
	{
		LobbyUpdate lu = JsonUtility.FromJson<LobbyUpdate>(message.message);
		CurrentGame.Instance.gameDetail.teams = lu.teams;
		CurrentGame.Instance.isHost = lu.isHost;
		NetworkManager.Singleton.RoomManager.OnLobbyLoad();
	}

	protected void HandleConquerUpdate(MessageWrapper message)
	{
		ConqueringUpdate cu = JsonUtility.FromJson<ConqueringUpdate>(message.message);
		CurrentGame.Instance.lastConqueringUpdate = cu;
		if (cu.isConqueringTeam && !cu.isDraw && cu.progress >= 1.0)
		{
			InGameUIManager.s_Singleton.LogUI.AddToLog("Wijk overgenomen", new object[] { });
		}
	}

	protected void HandlePlayerKicked()
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
