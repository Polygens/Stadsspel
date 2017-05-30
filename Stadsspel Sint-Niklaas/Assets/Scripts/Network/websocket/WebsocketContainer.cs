using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading;
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

		SendHearthbeat();
	}

	public void SendHearthbeat()
	{
		Send(GameMessageType.HEARTBEAT, "");
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
		//todo move this line somewhere else
		NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(false);
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

	public void Send()
	{
		if (messageBuffer.Count <= 0) return;
		if (ws == null) return;
		try
		{
			var message = messageBuffer.Peek();
			//todo compress message
			Debug.Log("SENDING");
			Debug.Log(message);
			ws.SendString(message);
			messageBuffer.Dequeue();
		}
		catch (Exception e)
		{
			Debug.Log("Error during sening of message, buffering");
			Debug.Log(e);
		}
	}

	public void Send(GameMessageType type, string innerMessage)
	{
		string message = JsonUtility.ToJson(new MessageWrapper(type, innerMessage, gameID, clientId: clientID, token: CurrentGame.Instance.ClientToken));
		messageBuffer.Enqueue(message);
		Send();
	}

	public void SendLocation(Point location)
	{
		LocationMessage lm = new LocationMessage(location.latitude,location.longitude);
		string innerMessage = JsonUtility.ToJson(lm);
		string message = JsonUtility.ToJson(new MessageWrapper(GameMessageType.LOCATION, innerMessage, gameID, clientId: clientID, token: CurrentGame.Instance.ClientToken));
		messageBuffer.Enqueue(message);
		Send();
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
