
using System;
using System.Collections.Generic;

public class GameEventMessage {
  public GameEventType type;
  public List<string> players;
  public double moneyTransferred;
  public IDictionary<string, int> items;
  public string tradePostId;
}