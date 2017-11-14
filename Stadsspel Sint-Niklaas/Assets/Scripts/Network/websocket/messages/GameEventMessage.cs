
using System;
using System.Collections.Generic;

[Serializable]
public class GameEventMessage
{
	//private GameEventType p_type;
	public List<string> players;
	public double moneyTransferred;
	public IDictionary<string, int> items;
	public string tradePostId;
	public string type;


	public GameEventMessage(GameEventType type, List<string> players, double moneyTransferred, IDictionary<string, int> items, string tradePostId)
	{
		//p_type = type;
		this.type = type.ToString();
		this.players = players;
		this.moneyTransferred = moneyTransferred;
		this.items = items;
		this.tradePostId = tradePostId;
	}
}
