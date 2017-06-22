using System;
using UnityEngine;

[Serializable]
public class MessageWrapper
{
	private GameMessageType eMessageType;
	private bool typeInit = false;
	[SerializeField]
	private string messageType;
	public string token;
	public string message;
	public string gameID;
	public string clientID;



	public GameMessageType MessageType {
		get {
			if (!typeInit)
			{
				eMessageType = (GameMessageType)Enum.Parse(typeof(GameMessageType), messageType);
				typeInit = true;
			}
			return eMessageType;
		}
	}


	public MessageWrapper(GameMessageType messageType, string message, string gameId, string token = null, string clientId = null)
	{
		this.eMessageType = messageType;
		this.message = message;
		this.gameID = gameId;
		this.token = token;
		this.clientID = clientId;
		this.messageType = eMessageType.ToString();
	}
}