using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

  public Item(string ItemName, float BuyPrice, float SellPrice, bool IsLegal)
  {
    itemName = ItemName;
    buyPrice = BuyPrice;
    sellPrice = SellPrice;
    isLegal = IsLegal;
  }

  private float buyPrice;
  private float sellPrice;
  private bool isLegal;
  private string itemName;

  public string ItemName
  {
    get { return itemName; }
    set { itemName = value; }
  }

  public float BuyPrice
  {
    get { return buyPrice; }
    set { buyPrice = value; }
  }

  public bool IsLegal
  {
    get { return isLegal; }
    set { isLegal = value; }
  }

  public float SellPrice
  {
    get { return sellPrice; }
    set { sellPrice = value; }
  }
}
