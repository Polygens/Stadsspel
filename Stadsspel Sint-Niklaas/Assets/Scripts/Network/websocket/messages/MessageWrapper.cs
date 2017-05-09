using System;

[Serializable]
public class MessageWrapper {
  private GameMessageType eMessageType;
  public string messageType;
  public string token;
  public string message;
  public string gameID;
  public string clientID;

  public MessageWrapper(GameMessageType messageType, string message, string gameId, string token = null, string clientId = null) {
    this.eMessageType = messageType;
    this.message = message;
    this.gameID = gameId;
    this.token = token;
    this.clientID = clientId;
    this.messageType = eMessageType.ToString();
  }

  public GameMessageType getMessageType() {
    return (GameMessageType)Enum.Parse(typeof(GameMessageType), messageType);
  }
}