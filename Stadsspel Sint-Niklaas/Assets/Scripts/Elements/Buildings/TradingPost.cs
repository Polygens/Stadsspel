using UnityEngine.Networking;
using System.Collections.Generic;

public class TradingPost : Building
{
  SyncListInt visitedTeams;
  List<Item> shopItems = new List<Item>();

  public void Start()
  {
    shopItems.Add(new Item("Pizza", 10, 15, true));
    shopItems.Add(new Item("Ijs", 2.5f, 4f, true));
    shopItems.Add(new Item("Koekjes", 6f, 10f, true));
    shopItems.Add(new Item("Orgaan", 50f, 70f, true));
    shopItems.Add(new Item("Diploma", 100, 120, true));
    shopItems.Add(new Item("Drugs", 30, 50, true));
  }

  public void AddTeamToList()
  {
    throw new System.NotImplementedException();
  }


  public void AddGoodsToPlayer()
  {
    throw new System.NotImplementedException();
  }

  //For drop down or inputField
  public void UpdateNumberOfGoods(int number)
  {
    throw new System.NotImplementedException();
  }

  public TradingPost()
	{
		throw new System.NotImplementedException();
	}

	public bool IsVisited {
		get {
			throw new System.NotImplementedException();
		}

		set {
		}
	}
}