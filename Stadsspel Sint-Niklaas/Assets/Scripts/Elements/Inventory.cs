using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : Person
{
    
    RectTransform GoederenPanel;
    
    int mDiploma = GameObject.FindWithTag("Player").GetComponent<Player>().LookUpIllegalItems[(int)Items.diploma ];
    int mOrganen = GameObject.FindWithTag("Player").GetComponent<Player>().LookUpIllegalItems[(int)Items.orgaan];
    int mDrugs = GameObject.FindWithTag("Player").GetComponent<Player>().LookUpIllegalItems[(int)Items.drugs];
    int mIjs = GameObject.FindWithTag("Player").GetComponent<Player>().LookUpLegalItems[(int)Items.ijs];
    int mKoekjes = GameObject.FindWithTag("Player").GetComponent<Player>().LookUpLegalItems[(int)Items.koekjes];
    int mPizza = GameObject.FindWithTag("Player").GetComponent<Player>().LookUpLegalItems[(int)Items.pizza];
    List<int> lItems = new List<int>();


    public new void Start () {
        lItems.Add(mIjs);
        lItems.Add(mDrugs);
        lItems.Add(mKoekjes);
        lItems.Add(mDiploma);
        lItems.Add(mPizza);
        lItems.Add(mOrganen);
        
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
