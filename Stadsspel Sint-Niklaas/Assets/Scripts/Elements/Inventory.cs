using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : Person
{
    
    RectTransform GoederenPanel;
    List<int> lItems = new List<int>();


    public new void Start () {
        lItems.Add(GameObject.FindWithTag("Player").GetComponent<Player>().LookUpLegalItems[(int)Items.ijs]);
        lItems.Add(GameObject.FindWithTag("Player").GetComponent<Player>().LookUpIllegalItems[(int)Items.drugs]);
        lItems.Add(GameObject.FindWithTag("Player").GetComponent<Player>().LookUpLegalItems[(int)Items.koekjes]);
        lItems.Add(GameObject.FindWithTag("Player").GetComponent<Player>().LookUpIllegalItems[(int)Items.diploma]);
        lItems.Add(GameObject.FindWithTag("Player").GetComponent<Player>().LookUpLegalItems[(int)Items.pizza]);
        lItems.Add(GameObject.FindWithTag("Player").GetComponent<Player>().LookUpIllegalItems[(int)Items.orgaan]);
        
        GoederenPanel = (RectTransform)GameObject.FindWithTag("Canvas").transform.FindChild("Panels").transform.FindChild("Goederen");
        RectTransform Grid = (RectTransform)GoederenPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
        int index = 0;
        for (int i = 1; i < Grid.childCount; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Grid.GetChild(i).GetChild(j).transform.FindChild("Aantal").GetComponent<Text>().text = lItems[index].ToString();
                index++;
            }
        }

    }
}
