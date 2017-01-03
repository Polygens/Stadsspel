using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class TradingPost : Building
{
  RectTransform TradingPostPanel;
  Text totalPrice;
  List<InputField> inputfields = new List<InputField>();
  List<Text> totalTextFields = new List<Text>();
  SyncListInt visitedTeams = new SyncListInt();
  List<Item> shopItems = new List<Item>();
  int[] numberOfEachItem;

  public void Start()
  {
    TradingPostPanel = (RectTransform)GameObject.FindWithTag("Canvas").transform.FindChild("Panels").transform.FindChild("TradingPost");
    //if (!isLocalPlayer)
    //{
    //  TradingPostPanel.gameObject.SetActive(false);
    //  return;
    //}

    if (CheckIfTeamAlreadyVisited())
    {
      TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(true);
      return;
    }

    // Like the grid in hierarchy, each 2 is a row
    shopItems.Add(new Item("Ijs", 3, 4, true));
    shopItems.Add(new Item("Drugs", 30, 50, false));
    shopItems.Add(new Item("Koekjes", 6, 10, true));

    shopItems.Add(new Item("Diploma", 100, 120, false));
    shopItems.Add(new Item("Pizza", 10, 15, true));
    shopItems.Add(new Item("Orgaan", 50, 70, false));

   
    RectTransform Grid = (RectTransform)TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
    totalPrice = TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("BuyPanel").transform.FindChild("AmountOfGoods").GetComponent<Text>();
    int childsInGrid = Grid.childCount;
    int index = 0;
    for (int i = 1; i < childsInGrid; i++)
    {
      for (int j = 0; j < 2; j++)
      {
        Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow1").transform.FindChild("PrijsLabel").transform.FindChild("Prijs").GetComponent<Text>().text = shopItems[index].BuyPrice.ToString();
        inputfields.Add(Grid.GetChild(i).GetChild(j).transform.FindChild("InputField").GetComponent<InputField>());
        totalTextFields.Add(Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Totaal").GetComponent<Text>());
        index++;
      }
    }
    numberOfEachItem = new int[shopItems.Count];
    Debug.Log(numberOfEachItem.Length);
    TradingPostPanel.gameObject.SetActive(false);
  }

  private bool CheckIfTeamAlreadyVisited()
  {
    bool teamAlreadyVisited = false;
    for (int i = 0; i < visitedTeams.Count; i++)
    {
      if (visitedTeams[i] == (int)GameObject.FindWithTag("Player").GetComponent<Player>().Team)
      {
        teamAlreadyVisited = true;
      }
    }
    return teamAlreadyVisited;
  }

  public void OnEnable()
  {
    if (CheckIfTeamAlreadyVisited())
    {
      TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(true);
      return;
    }
  }

  public void Purchase()
  { 
    //if (!isLocalPlayer)
    //{
    //  return;
    //}
    if (!CheckIfTeamAlreadyVisited())
    {
      AddGoodsToPlayer();
      TradingPostPanel.gameObject.SetActive(false);
    }
    else
    {
      TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(true);
    }

  }

  //Execute when items are purchased (button holds this method)
  public void AddTeamToList()
  {   
    visitedTeams.Add((int)GameObject.FindWithTag("Player").GetComponent<Player>().Team);
  }


  public void AddGoodsToPlayer()
  {
    List<int> legalItems = new List<int>();
    List<int> illegalItems = new List<int>();
    for (int i = 0; i < numberOfEachItem.Length; i++)
    {
      if (shopItems[i].IsLegal)
      {
        legalItems.Add(numberOfEachItem[i]);
      }
      else
      {
        illegalItems.Add(numberOfEachItem[i]);
      }
    }
    GameObject.FindWithTag("Player").GetComponent<Player>().AddLegalItems(legalItems);
    GameObject.FindWithTag("Player").GetComponent<Player>().AddIllegalItems(illegalItems);
    AddTeamToList();
  }

  //For drop down or inputField
  public void UpdateNumberOfGoods(string number)
  {
    int focusedIndex = 0;
    int result = 0;
    if (number == "")
    {
      number = "0";
    }
    for (int i = 0; i < inputfields.Count; i++)
    {
      if (inputfields[i].isFocused)
      {
        
        bool isNumber = int.TryParse(number,out result);
        if (isNumber)
        {
          numberOfEachItem[i] = result;
        }
        focusedIndex = i;
      }
    }
    int tempTotal = 0;
    for (int i = 0; i < numberOfEachItem.Length; i++)
    {
      tempTotal += (numberOfEachItem[i] * shopItems[i].BuyPrice);
    }

    totalPrice.text = tempTotal.ToString();
    int itemTotal = (result * shopItems[focusedIndex].BuyPrice);
    totalTextFields[focusedIndex].text = "Totaal: " + itemTotal.ToString();
  }

  public TradingPost()
	{
		//throw new System.NotImplementedException();
	}

	public bool IsVisited {
		get {
			throw new System.NotImplementedException();
		}

		set {
		}
	}
}