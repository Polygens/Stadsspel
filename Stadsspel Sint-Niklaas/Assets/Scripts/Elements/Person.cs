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
  private DistrictManager DM;

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
    DM = GetComponent<DistrictManager>();
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

  public void PriorityUpdate(List<GameObject> allGameObjectsInRadius, Collider other)
  {
    if (allGameObjectsInRadius.Count > 0)
    {
      // In case there were no collidings before.
      MainPanel.gameObject.SetActive(true);
      ListPanel.gameObject.SetActive(true);
      priority tempPriority;

      // Set temp priority to lowest
      tempPriority = Enum.GetValues(typeof(priority)).Cast<priority>().First();
      int priorityNbr = 0;

      //Check with the tag of the gameObject, which priority it has in the enum,
      // If the priority is higher then the current, update the priority
      for (int i = 0; i < allGameObjectsInRadius.Count; i++)
      {
        priorityNbr = (int)Enum.Parse(typeof(priority), allGameObjectsInRadius[i].tag);
        if (priorityNbr > (int)tempPriority)
        {
          tempPriority = (priority)priorityNbr;
        }
      }

      // If the priority is still the same, keep the same button
      // But... We need to check for timers, for example: Robbing can only happen if you are not robbed or
      // your timer is 0 again
      // If timer is not 0, take next priority...
      if (tempPriority == Priority)
      {

      }
      else
      {
        Priority = tempPriority;
        Destroy(MainPanel.GetChild(0));
        Button prefab;
        //Filling in mainpanel, highest priority spawns in mainpanel
        switch (Priority)
        {
          case priority.Enemy:
            prefab = Instantiate(robButton, MainPanel.transform, false) as Button;
            prefab.transform.SetParent(MainPanel, false);
            enemiesInRadius.Add(other.gameObject);
            break;
          case priority.Schatkist:
            //Check if colliding with chest, check on district if it's yours
            DistrictStates state = DM.CheckDisctrictState();
            string stateName = state.ToString();
            int stateNumber = int.Parse(stateName.Substring(0, 1));
            if (stateNumber == mTeam)
            {
              prefab = Instantiate(taxInningButton, MainPanel.transform, false) as Button;
              prefab.transform.SetParent(MainPanel, false);
            }
            break;
          case priority.EnemyPlein:
            break;
          case priority.TradingPost:
            prefab = Instantiate(tradingPostButton, MainPanel.transform, false) as Button;
            prefab.transform.SetParent(MainPanel, false);
            break;
          case priority.Markt:
            prefab = Instantiate(marktButton, MainPanel.transform, false) as Button;
            prefab.transform.SetParent(MainPanel, false);
            break;
          case priority.Bank:
            prefab = Instantiate(bankButton, MainPanel.transform, false) as Button;
            prefab.transform.SetParent(MainPanel, false);
            break;
          case priority.Team:
            prefab = Instantiate(teamTradeButton, MainPanel.transform, false) as Button;
            prefab.transform.SetParent(MainPanel, false);
            break;
          default:
            break;
        }
      }

      // Update the listPanel... 
      // TempList of gameObjects in radius should delete the gameObject that uses mainpanel
      // All others should be added to the list (The buttons needed for those gameObjects) 
    }
    else
    {
      // No need to show the panels with buttons if there is nothing near
      MainPanel.gameObject.SetActive(false);
      ListPanel.gameObject.SetActive(false);
    }
  }

	public void OnTriggerEnter(Collider other)
  { 
		allGameObjectsInRadius.Add(other.gameObject);
    PriorityUpdate(allGameObjectsInRadius, other);

	}

	public void OnTriggerExit(Collider other)
	{
		enemiesInRadius.Remove(other.gameObject);
		allGameObjectsInRadius.Remove(other.gameObject);
    PriorityUpdate(allGameObjectsInRadius, other);
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
  EnemyPlein,
	Enemy
}

