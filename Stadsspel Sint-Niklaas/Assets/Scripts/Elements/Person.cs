using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;

public class Person : Element
{
	List<int> illegalItems = new List<int>();
  //legalItems[(int)Items.diploma] = 10; Bijvoorbeeld
  List<int> legalItems = new List<int>();
	private int mAmountOfMoney;
	private int mTeam;
	private int mDetectionRadius;
	private List<GameObject> enemiesInRadius = new List<GameObject>();
	private List<GameObject> allGameObjectsInRadius = new List<GameObject>();
	private priority Priority = priority.Team;

	public Button robButton;
	public Button marktButton;
	public Button tradingPostButton;
	public Button teamTradeButton;
	public Button bankButton;
	public Button claimButton;
	public Button taxInningButton;
  public RectTransform MainPanel;
  public RectTransform ListPanel;


	private void Start()
	{
    MainPanel.gameObject.SetActive(false);
    ListPanel.gameObject.SetActive(false);
  }

	public Person()
	{
		throw new System.NotImplementedException();
	}

	public void UpdatePosition()
	{
		throw new System.NotImplementedException();
	}

	public void Rob()
	{
		foreach (GameObject enemy in enemiesInRadius)
		{
			AddGoods(enemy.GetComponent<Person>().AmountOfMoney, enemy.GetComponent<Person>().LookUpLegalItems, enemy.GetComponent<Person>().LookUpIllegalItems);
			enemy.GetComponent<Person>().GetRobbed();
		}
		throw new System.NotImplementedException();
	}

	public void ResetLegalItems()
	{
		legalItems.Clear();
	}

	public void ResetIllegalItems()
	{
		illegalItems.Clear();
	}

	public void GetRobbed()
	{
		mAmountOfMoney = 0;
		ResetLegalItems();
		ResetIllegalItems();
		throw new System.NotImplementedException();
	}

	public List<int> LookUpLegalItems
	{
		get { return legalItems; }
	}

	public List<int> LookUpIllegalItems
	{
		get { return illegalItems; }
	}

	public void AddLegalItems(List<int> items)
	{
		for (int i = 0; i < items.Count; i++)
		{
			legalItems[i] = items[i];
		}
	}

	public void AddIllegalItems(List<int> items)
	{
		for (int i = 0; i < items.Count; i++)
		{
			illegalItems[i] = items[i];
		}
	}

	public void RemoveMoney(int money)
	{
		mAmountOfMoney -= money;
	}


	public void AddItems(int money)
	{
		mAmountOfMoney += money;
		throw new System.NotImplementedException();
	}

	public void AddGoods(int money, List<int> legalItems, List<int> illegalItems)
	{
		AddItems(money);
		AddLegalItems(legalItems);
		AddIllegalItems(illegalItems);
		throw new System.NotImplementedException();
	}

	public void OnTriggerEnter(Collider other)
  { 
		allGameObjectsInRadius.Add(other.gameObject);
    priority tempPriority;
    tempPriority = Enum.GetValues(typeof(priority)).Cast<priority>().First();
    int priorityNbr = 0;
		for (int i = 0; i < allGameObjectsInRadius.Count; i++)
		{
			priorityNbr = (int)Enum.Parse(typeof(priority), allGameObjectsInRadius[i].tag);
      if (priorityNbr > (int)tempPriority)
      {
        tempPriority = (priority)priorityNbr;
      }
		}

    if (tempPriority == Priority)
    {

    }
    else
    {
      Priority = tempPriority;
      Destroy(MainPanel.GetChild(0));
      switch (Priority)
      {
        case priority.Enemy:
          robButton.transform.SetParent(MainPanel, false);
          enemiesInRadius.Add(other.gameObject);
          break;
        case priority.Schatkist:
          break;
        case priority.TradingPost:
          break;
        case priority.Markt:
          break;
        case priority.Bank:
          break;
        case priority.Team:
          break;
        default:
          break;
      }
    }

	}

	public void OnTriggerExit(Collider other)
	{
		enemiesInRadius.Remove(other.gameObject);
		allGameObjectsInRadius.Remove(other.gameObject);
	}

	public int AmountOfMoney
	{
		get { return mAmountOfMoney; }
	}
}

public enum priority : byte
{
	Team,
	Bank,
	TradingPost,
	Markt,
	Schatkist,
	Enemy
}

