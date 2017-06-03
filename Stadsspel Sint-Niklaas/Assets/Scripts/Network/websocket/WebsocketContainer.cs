using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading;
using fastJSON;
using Stadsspel.Networking;
using UnityEngine;
using WebSocketSharp;

public abstract class WebsocketContainer : Singleton<WebsocketContainer>
{
	//private static UTF8Encoding encoder = new UTF8Encoding();

	private WebSocket ws;
	private Thread listeningThread;
	private bool stopThread = true;
	private readonly Queue<string> messageBuffer;
	private string gameID, clientID;
	private ConcurrentQueue<MessageWrapper> _inbox;

	private void Update()
	{
		while (!_inbox.IsEmpty)
		{
			MessageWrapper messageWrapper;
			while (!_inbox.TryDequeue(out messageWrapper)) ;
			HandleMessage(messageWrapper);
		}

		//send a message from the buffer
		Send();
	}

	protected WebsocketContainer()
	{
		_inbox = new ConcurrentQueue<MessageWrapper>();
		//private constructor for singleton
		messageBuffer = new Queue<string>();
	}

	public IEnumerator Connect(string url, string gameID, string clientID)
	{
		if (ws == null)
		{
			ws = new WebSocket(new Uri(url));
		}
		this.clientID = clientID;
		this.gameID = gameID;
		_inbox = new ConcurrentQueue<MessageWrapper>();

		yield return StartCoroutine(ws.Connect());
		listeningThread = new Thread(ListeningRun);
		listeningThread.Start();

		//send hearthbeat to provide server with player info
		SendHearthbeat();
	}

	private void ListeningRun()
	{
		stopThread = false;
		while (!stopThread)
		{
			string reply = ws.RecvString();
			if (reply != null)
			{
				Debug.Log("Received: " + reply);
				MessageWrapper mw = JsonUtility.FromJson<MessageWrapper>(reply);
				_inbox.Enqueue(mw);
			}
			if (ws.error != null)
			{
				Debug.LogError("Error: " + ws.error);
				break;
			}
		}
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
			case GameMessageType.TREASURIES_CLOSE:
				HandleTreasuriesClose(message);
				break;
			case GameMessageType.TREASURIES_OPEN:
				HandleTreasuriesOpen(message);
				break;
			case GameMessageType.WINNING_TEAM:
				HandleWinningTeam(message);
				break;
			case GameMessageType.ERROR_EXCEPTION:
				HandleErrorException(message);
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
		IDictionary<string, int> items = null, string locationID = null)
	{
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

	protected abstract void HandleTreasuriesOpen(MessageWrapper message);

	protected abstract void HandleTreasuriesClose(MessageWrapper message);

	protected abstract void HandleTeamNotification(MessageWrapper message);

	protected abstract void HandleTagPermitted(MessageWrapper message);

	protected abstract void HandleBulkLocation(MessageWrapper message);
}
