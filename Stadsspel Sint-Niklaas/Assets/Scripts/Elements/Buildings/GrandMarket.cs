using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class GrandMarket : Building
{
    RectTransform MarktPanel;
    public Text TotalUI;
    List<int> lItems = new List<int>();
    private Player player;
    
    private int total;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        UpdateUI();
       
    }

    public void UpdateUI()
    {
        lItems.Add(player.LookUpLegalItems[(int)Items.ijs]);
        lItems.Add(player.LookUpIllegalItems[(int)Items.drugs]);
        lItems.Add(player.LookUpLegalItems[(int)Items.koekjes]);
        lItems.Add(player.LookUpIllegalItems[(int)Items.diploma]);
        lItems.Add(player.LookUpLegalItems[(int)Items.pizza]);
        lItems.Add(player.LookUpIllegalItems[(int)Items.orgaan]);

        MarktPanel = (RectTransform)GameObject.FindWithTag("Canvas").transform.FindChild("Panels").transform.FindChild("Markt");
        RectTransform Grid = (RectTransform)MarktPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
        int index = 0;

        for (int i = 1; i < Grid.childCount; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Amount").GetComponent<Text>().text = "Amount: " + lItems[index].ToString();
                int selPrice = int.Parse(Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow1").transform.FindChild("GameObject").transform.FindChild("Prijs").GetComponent<Text>().text);
                int subTotal = selPrice * lItems[index];
                Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Profit").GetComponent<Text>().text = "Winst: " + subTotal;
                total += subTotal;
                index++;
            }
        }
        TotalUI.text  = "Totaal: " + total;

    }
    public void Kopen()
    {

        player.AddItems(total);
        player.ResetIllegalItems();
        player.ResetLegalItems();
        lItems.Clear();
        total = 0;
        UpdateUI();

    }
   
}