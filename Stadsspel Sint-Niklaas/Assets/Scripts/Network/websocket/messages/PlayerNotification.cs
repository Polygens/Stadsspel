using System;
using System.Collections.Generic;
[Serializable]
public class PlayerNotification {
  public IDictionary<string, int> legalItems;
  public IDictionary<string, int> illegalItems;
  public double money;
}