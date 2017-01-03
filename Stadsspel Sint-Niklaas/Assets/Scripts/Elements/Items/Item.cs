using UnityEngine;
using System.Collections;

public class Item {

  public Item(string ItemName, int BuyPrice, int SellPrice, bool IsLegal)
  {
    itemName = ItemName;
    buyPrice = BuyPrice;
    sellPrice = SellPrice;
    isLegal = IsLegal;
  }

  private int buyPrice;
  private int sellPrice;
  private bool isLegal;
  private string itemName;

  public string ItemName
  {
    get { return itemName; }
    set { itemName = value; }
  }

  public int BuyPrice
  {
    get { return buyPrice; }
    set { buyPrice = value; }
  }

  public bool IsLegal
  {
    get { return isLegal; }
    set { isLegal = value; }
  }

  public int SellPrice
  {
    get { return sellPrice; }
    set { sellPrice = value; }
  }
}

public enum Items : byte
{
  pizza,
  ijs,
  koekjes,
  orgaan,
  diploma,
  drugs
}
