using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GrandMarketUI : MonoBehaviour
{

  RectTransform MarktPanel;
  public Text TotalUI;
  List<int> illegalItems = new List<int>();
  List<int> legalItems = new List<int>();
  private Player player;
  private int total;


  public void OnEnable()
  {
    player = GameObject.FindWithTag("Player").GetComponent<Player>();
    UpdateUI();
  }

  public void UpdateUI()
  {
    legalItems = GameObject.FindWithTag("Player").GetComponent<Player>().Person.LookUpLegalItems;
    illegalItems = GameObject.FindWithTag("Player").GetComponent<Player>().Person.LookUpIllegalItems;

    MarktPanel = (RectTransform)GameObject.FindWithTag("Canvas").transform.FindChild("Panels").transform.FindChild("GrandMarket");
    RectTransform Grid = (RectTransform)MarktPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
    int index = 0;
    //int indexLegal = 0;  
    //int indexIllegal = 0;  

    for (int i = 1; i < Grid.childCount; i++) // i is which row 
    {
      for (int j = 0; j < 2; j++) // J is for legal or illegal 
      {
        int subTotal = 0;
        Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow1").transform.FindChild("PrijsLabel").transform.FindChild("Prijs").GetComponent<Text>().text = Item.ShopItems[index].SellPrice.ToString();
        if (j == 0)
        {
          Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Amount").GetComponent<Text>().text = "Amount: " + legalItems[i - 1].ToString();
          subTotal = CalculateSubtotal(i - 1, index, legalItems);
          //indexLegal++;  
        }
        else
        {
          Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Amount").GetComponent<Text>().text = "Amount: " + illegalItems[i - 1].ToString();
          subTotal = CalculateSubtotal(i - 1, index, illegalItems);
          //indexIllegal++;  
        }


        Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Profit").GetComponent<Text>().text = "Winst: " + subTotal;
        total += subTotal;
        index++;

        TotalUI.text = "Totaal: " + total;


      }
    }
  }

  private int CalculateSubtotal(int Listindex, int index, List<int> items)
  {
    int sellPrice = Item.ShopItems[index].SellPrice;
    int subTotal = sellPrice * items[Listindex];
    return subTotal;
  }

  public void Sell()
  {

    player.Person.AddGoods(total);
    player.Person.ResetIllegalItems();
    player.Person.ResetLegalItems();
    //GameObject.FindWithTag("Player").GetComponent<Player>().Person.MoneyTransaction(total); 
    //GameObject.FindWithTag("Player").GetComponent<Player>().Person.ResetIllegalItems(); 
    //GameObject.FindWithTag("Player").GetComponent<Player>().Person.ResetLegalItems(); 
    total = 0;
    UpdateUI();
    ResetUI();
  }

  private void ResetUI()
  {
    RectTransform Grid = (RectTransform)MarktPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
    for (int i = 1; i < Grid.childCount; i++)
    {
      for (int j = 0; j < 2; j++)
      {
        Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Amount").GetComponent<Text>().text = "Amount: 0";
        Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Profit").GetComponent<Text>().text = "Winst: 0";
      }
    }

    TotalUI.text = "Totaal: " + total;
    MarktPanel.gameObject.SetActive(false);
  }

}

