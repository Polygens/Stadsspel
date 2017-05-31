
using System;
using System.Collections.Generic;

[Serializable]
public class GameEventMessage {
  public GameEventType type;
  public List<string> players;
  public double moneyTransferred;
  public IDictionary<string, int> items;
  public string tradePostId;


	public GameEventMessage(GameEventType type, List<string> players, double moneyTransferred, IDictionary<string, int> items, string tradePostId)
	{
		this.type = type;
		this.players = players;
		this.moneyTransferred = moneyTransferred;
		this.items = items;
		this.tradePostId = tradePostId;
	}

	public GameEventMessage()
	{
	}
}