using System;
using System.Collections;
//using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Assets.Scripts.Network.websocket.messages;
using fastJSON;
using Stadsspel.Networking;
using UnityEngine;

public abstract class WebsocketContainer : Singleton<WebsocketContainer>
{
	//private static UTF8Encoding encoder = new UTF8Encoding();

	private WebSocket ws;
	private Thread listeningThread;
	private bool stopThread = true;
	private readonly Queue<string> messageBuffer;
	private string gameID, clientID;
	//private ConcurrentQueue<MessageWrapper> _inbox;
	private Queue<MessageWrapper> _inbox;

	private void Update()
	{
		while (_inbox.Count >0)
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

	protected WebsocketContainer()
	{
		//_inbox = new ConcurrentQueue<MessageWrapper>();todo concurrency
		_inbox = new Queue<MessageWrapper>();
		//private constructor for singleton
		messageBuffer = new Queue<string>();
	}

	public IEnumerator Connect(string url, string gameID, string clientID)
	{
		Debug.Log("DEBUG");
		if (listeningThread.ThreadState != ThreadState.Stopped)
		{
			Debug.Log("Stop previous");
			stopThread = true;
			yield return new WaitUntil(() => listeningThread.ThreadState == ThreadState.Stopped);
		}

		if (ws == null)
		{
			ws = new WebSocket(new Uri(url));
		}
		else
		{
			ws.Close();
		}

		this.clientID = clientID;
		this.gameID = gameID;
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
			}
			else
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

	private void ListeningRun()
	{
		stopThread = false;
		int consecutiveErrors =0;
		while (!stopThread)
		{
			string reply = ws.RecvString();
			if (reply != null)
			{
				consecutiveErrors = 0;
				MessageWrapper mw = JsonUtility.FromJson<MessageWrapper>(reply);
				Debug.Log(mw.messageType);
				_inbox.Enqueue(mw);
			}
			if (ws.error != null)
			{
				consecutiveErrors++;
				if (consecutiveErrors >= 5)
				{
					Debug.LogError("Error: " + ws.error);
					stopThread = true;
				}
			}
		}
	}

	public void Clear()
	{
		stopThread = true;
	}

	private void HandleMessage(MessageWrapper message)
	{
		if (!message.gameID.Equals(CurrentGame.Instance.GameId)) return; //if not right game do nothing todo throw error or something also check if no error will occur with this line here
		if (NetworkManager.Singleton.ConnectingManager!=null)
		{
			NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(false); //todo move this line somewhere better
		}
		switch (message.getMessageType())
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
				Debug.Log("Message is not of a type we should catch");
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
			var message = messageBuffer.Peek();
			//Debug.Log(message);
			//todo compress message
			ws.SendString(message);
			messageBuffer.Dequeue();
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
		string innerMessage = JSON.ToJSON(gem,jsonParameters);
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
		Send(GameMessageType.CONQUERING_START,JsonUtility.ToJson(cm));
	}

	public void SendConquerEnd(string locationId)
	{
		ConquerMessage cm = new ConquerMessage();
		cm.LocationID = locationId;
		Send(GameMessageType.CONQUERING_END,JsonUtility.ToJson(cm));
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

	protected abstract void HandleGameStart(MessageWrapper message);

	protected abstract void HandleEvent(MessageWrapper message);

	protected abstract void HandleDistrictNotification(MessageWrapper message);

	protected abstract void HandleGameStop(MessageWrapper message);

	protected abstract void HandleInfoNotification(MessageWrapper message);

	protected abstract void HandlePlayerNotification(MessageWrapper message);

	protected abstract void HandleTagNotification(MessageWrapper message);

	protected abstract void HandleErrorException(MessageWrapper message);

	protected abstract void HandleWinningTeam(MessageWrapper message);

	protected abstract void HandleTeamNotification(MessageWrapper message);

	protected abstract void HandleTagPermitted(MessageWrapper message);

	protected abstract void HandleBulkLocation(MessageWrapper message);

	protected abstract void HandleLobbyUpdate(MessageWrapper message);

	protected abstract void HandleConquerUpdate(MessageWrapper message);

	protected abstract void HandlePlayerKicked();

}
