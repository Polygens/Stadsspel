using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class TradingPost : Building
{
<<<<<<< Updated upstream
<<<<<<< Updated upstream
  RectTransform TradingPostPanel;
  Text totalPriceText;
  int totalPriceAmount;
  List<InputField> inputfields = new List<InputField>();
  List<Text> totalTextFields = new List<Text>();
  //SyncListInt visitedTeams = new SyncListInt();
  List<Item> shopItems = new List<Item>();
  int[] numberOfEachItem;
  bool everythingIsInstantiated = false;

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

    shopItems = Item.ShopItems;

   
    RectTransform Grid = (RectTransform)TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
    totalPriceText = TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("BuyPanel").transform.FindChild("AmountOfGoods").GetComponent<Text>();
    TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("MoneyPanel").transform.FindChild("AmountOfMoney").GetComponent<Text>().text = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AmountOfMoney.ToString();
    int childsInGrid = Grid.childCount;
    int index = 0;
=======

  List<InputField> inputfields = new List<InputField>();
  SyncListInt visitedTeams = new SyncListInt();
  List<Item> shopItems = new List<Item>();
  int[] numberOfEachItem;

  public void Start()
  {
=======

  List<InputField> inputfields = new List<InputField>();
  SyncListInt visitedTeams = new SyncListInt();
  List<Item> shopItems = new List<Item>();
  int[] numberOfEachItem;

  public void Start()
  {
>>>>>>> Stashed changes
    // Like the grid
    shopItems.Add(new Item("Ijs", 2.5f, 4f, true));
    shopItems.Add(new Item("Drugs", 30, 50, false));
    shopItems.Add(new Item("Pizza", 10, 15, true));
    shopItems.Add(new Item("Diploma", 100, 120, false));
    shopItems.Add(new Item("Koekjes", 6f, 10f, true));
    shopItems.Add(new Item("Orgaan", 50f, 70f, false));



    RectTransform TradingPostPanel = (RectTransform)GameObject.FindWithTag("Canvas").transform.FindChild("Panels").transform.FindChild("TradingPost");
    RectTransform Grid = (RectTransform)TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
    int childsInGrid = Grid.childCount;
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    for (int i = 1; i < childsInGrid; i++)
    {
      for (int j = 0; j < 2; j++)
      {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow1").transform.FindChild("PrijsLabel").transform.FindChild("Prijs").GetComponent<Text>().text = shopItems[index].BuyPrice.ToString();
        inputfields.Add(Grid.GetChild(i).GetChild(j).transform.FindChild("InputField").GetComponent<InputField>());
        totalTextFields.Add(Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Totaal").GetComponent<Text>());
        index++;
      }
    }
    numberOfEachItem = new int[shopItems.Count];
    everythingIsInstantiated = true;
    //TradingPostPanel.gameObject.SetActive(false);
  }

  private bool CheckIfTeamAlreadyVisited()
  {
    bool teamAlreadyVisited = false;
    GameObject tempTradePost = GameObject.FindWithTag("Player").GetComponent<Player>().GetTradingPost();
    List<int> visitedTeams = tempTradePost.GetComponent<TradingPostSyncing>().VisitedTeams;
    for (int i = 0; i < visitedTeams.Count; i++)
    {
      if (visitedTeams[i] == (int)GameObject.FindWithTag("Player").GetComponent<Player>().Team)
      {
        teamAlreadyVisited = true;
      }
    }
    return teamAlreadyVisited;
=======
        inputfields.Add(Grid.GetChild(i).GetChild(j).transform.FindChild("InputField").GetComponent<InputField>());
      }
    }
    numberOfEachItem = new int[shopItems.Count];
  }

  //Execute when items are purchased (button holds this method)
  public void AddTeamToList()
  {   
    if (!isLocalPlayer)
    {
      return;
    }
    visitedTeams.Add((int)GameObject.FindWithTag("Player").GetComponent<Player>().Team);
    AddGoodsToPlayer();
>>>>>>> Stashed changes
  }

  public void OnEnable()
  {
    if (CheckIfTeamAlreadyVisited())
    {
      TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(true);
      return;
    }
    if (everythingIsInstantiated)
    {
      TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("MoneyPanel").transform.FindChild("AmountOfMoney").GetComponent<Text>().text = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AmountOfMoney.ToString();

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
      if (GameObject.FindWithTag("Player").GetComponent<Player>().AmountOfMoney > totalPriceAmount)
      {
        AddGoodsToPlayer();
        TradingPostPanel.gameObject.SetActive(false);
      }
      else
      {
        Debug.Log("Not enough money");
      }
    }
    else
    {
      TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(true);
    }

=======
        inputfields.Add(Grid.GetChild(i).GetChild(j).transform.FindChild("InputField").GetComponent<InputField>());
      }
    }
    numberOfEachItem = new int[shopItems.Count];
  }

  //Execute when items are purchased (button holds this method)
  public void AddTeamToList()
  {   
    if (!isLocalPlayer)
    {
      return;
    }
    visitedTeams.Add((int)GameObject.FindWithTag("Player").GetComponent<Player>().Team);
    AddGoodsToPlayer();
>>>>>>> Stashed changes
  }

  //Execute when items are purchased (button holds this method)


  public void AddGoodsToPlayer()
  {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
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
    GameObject.FindWithTag("Player").GetComponent<Player>().GetTradingPost().GetComponent<TradingPostSyncing>().CmdAddTeamToList();
    GameObject.FindWithTag("Player").GetComponent<Player>().RemoveMoney(totalPriceAmount);

    for (int i = 0; i < inputfields.Count; i++)
    {
      inputfields[i].text = "";
      totalTextFields[i].text = "Totaal: 0";
    }
    totalPriceText.text = "0";
    totalPriceAmount = 0;
  }

  public void OnClose()
  {
    TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(false);
=======
=======
>>>>>>> Stashed changes
    //GameObject.FindWithTag("Player").GetComponent<Player>().AddIllegalItems();
    throw new System.NotImplementedException();
>>>>>>> Stashed changes
  }

  //For drop down or inputField
  public void UpdateNumberOfGoods(string number)
  {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    int focusedIndex = 0;
    int result = 0;
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    if (number == "")
    {
      number = "0";
    }
    for (int i = 0; i < inputfields.Count; i++)
    {
      if (inputfields[i].isFocused)
      {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        
=======
        int result = 0;
>>>>>>> Stashed changes
=======
        int result = 0;
>>>>>>> Stashed changes
        bool isNumber = int.TryParse(number,out result);
        if (isNumber)
        {
          numberOfEachItem[i] = result;
        }
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        focusedIndex = i;
      }
    }
    int tempTotal = 0;
    for (int i = 0; i < numberOfEachItem.Length; i++)
    {
      tempTotal += (numberOfEachItem[i] * shopItems[i].BuyPrice);
    }

    totalPriceAmount = tempTotal;
    totalPriceText.text = tempTotal.ToString();
    int itemTotal = (result * shopItems[focusedIndex].BuyPrice);
    totalTextFields[focusedIndex].text = "Totaal: " + itemTotal.ToString();
=======
=======
>>>>>>> Stashed changes

        Debug.Log(shopItems[i].ItemName);
      }
    }
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
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