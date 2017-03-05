using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

  static public List<Item> ShopItems
  {
    get
    {
      List<Item> shopItems = new List<Item>();
      shopItems.Add(new Item("Ijs", 3, 4, true));
      shopItems.Add(new Item("Drugs", 30, 50, false));
      shopItems.Add(new Item("Koekjes", 6, 10, true));

      shopItems.Add(new Item("Diploma", 100, 120, false));
      shopItems.Add(new Item("Pizza", 10, 15, true));
      shopItems.Add(new Item("Orgaan", 50, 70, false));
      return shopItems;  
    }
  }

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
