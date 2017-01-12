using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    
    public RectTransform GoederenPanel;
  List<int> legalItems = new List<int>();
  List<int> illegalItems = new List<int>();

  public void OnEnable () {
    legalItems = GameObject.FindWithTag("Player").GetComponent<Player>().LookUpLegalItems;
    illegalItems = GameObject.FindWithTag("Player").GetComponent<Player>().LookUpIllegalItems;


    RectTransform Grid = (RectTransform)GoederenPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
        for (int i = 1; i < Grid.childCount; i++)
        {
            for (int j = 0; j < 2; j++)
            {
              if (j == 0)
              {
                Grid.GetChild(i).GetChild(j).transform.FindChild("Aantal").GetComponent<Text>().text = legalItems[i - 1].ToString();
              }
              else
              {
                Grid.GetChild(i).GetChild(j).transform.FindChild("Aantal").GetComponent<Text>().text = illegalItems[i - 1].ToString();
              }
            }
        }

    }
}
