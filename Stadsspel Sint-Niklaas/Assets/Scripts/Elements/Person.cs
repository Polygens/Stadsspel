using UnityEngine;
using System.Collections.Generic;

public class Person : Element
{
	private int mNumberOfPizzas = 0;
	private int mNumberOfIceCreams = 0;
	private int mNumberOfCookies = 0;
	private int mNumberOfOrgans = 0;
	private int mNumberOfDiplomas = 0;
	private int mNumberOfDrugs = 0;
	private int mAmountOfMoney;
	//private int[] mAmountOfGoods;
	private int mTeam;
	private int mDetectionRadius;
  private List<GameObject> enemiesInRadius = new List<GameObject>();

  public GameObject robButton;

  private void Start()
  {
    robButton.SetActive(false);
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
      AddGoods(enemy.GetComponent<Person>().AmountOfMoney, enemy.GetComponent<Person>().LookUpGoods());
      enemy.GetComponent<Person>().GetRobbed();
    }
    throw new System.NotImplementedException();
  }

	public void GetRobbed()
	{
    mNumberOfPizzas = 0;
    mNumberOfIceCreams = 0;
    mNumberOfCookies = 0;
    mNumberOfOrgans = 0;
    mNumberOfDiplomas = 0;
    mNumberOfDrugs = 0;
    mAmountOfMoney = 0;
		throw new System.NotImplementedException();
	}

  public int[] LookUpGoods()
  {
    int[] goods = new int[5];
    goods[0] = mNumberOfPizzas;
    goods[1] = mNumberOfIceCreams;
    goods[2] = mNumberOfCookies;
    goods[3] = mNumberOfOrgans;
    goods[4] = mNumberOfDiplomas;
    goods[5] = mNumberOfDrugs;
    return goods;
  }

  // When you rob a person
  public void AddGoods(int money, int[] goods)
  {
    AddGoods(money);
    AddGoods(goods);
  }

  public void AddGoods(int[] goods)
  {
    mNumberOfPizzas += goods[0];
    mNumberOfIceCreams += goods[1];
    mNumberOfCookies += goods[2];
    mNumberOfOrgans += goods[3];
    mNumberOfDiplomas += goods[4];
    mNumberOfDrugs += goods[5];
  }

  // when you buy from a shop
	public void AddGoods(Item[] goods)
	{
		foreach (Item good in goods)
		{
			switch (good.ItemName)
			{
				case "Pizza":
					mNumberOfPizzas++;
					break;
				case "Ijs":
					mNumberOfIceCreams++;
					break;
				case "Koekjes":
					mNumberOfCookies++;
					break;
				case "Diploma":
					mNumberOfDiplomas++;
					break;
				case "Organen":
					mNumberOfOrgans++;
					break;
				case "Drugs":
					mNumberOfDrugs++;
					break;
				default:
					break;
			}
		}
		throw new System.NotImplementedException();
	}

  public void RemoveGoods(Item[] goods)
  {
    foreach (Item good in goods)
    {
      switch (good.ItemName)
      {
        case "Pizza":
          mNumberOfPizzas--;
          break;
        case "Ijs":
          mNumberOfIceCreams--;
          break;
        case "Koekjes":
          mNumberOfCookies--;
          break;
        case "Diploma":
          mNumberOfDiplomas--;
          break;
        case "Organen":
          mNumberOfOrgans--;
          break;
        case "Drugs":
          mNumberOfDrugs--;
          break;
        default:
          break;
      }
    }
  }

  public void RemoveGoods(int money)
  {
    mAmountOfMoney -= money;
  }


	public void AddGoods(int money)
	{
    mAmountOfMoney += money;
		throw new System.NotImplementedException();
	}

  public void RemoveGoods(int money, Item[] goods)
  {
    RemoveGoods(money);
    RemoveGoods(goods);
  }

	public void AddGoods(int money, Item[] goods)
	{
    AddGoods(money);
    AddGoods(goods);
		throw new System.NotImplementedException();
	}

  public void OnCollisionEnter(UnityEngine.Collision collision)
  {
    if (collision.gameObject.tag == "Enemy")
    {
      robButton.SetActive(true);
      enemiesInRadius.Add(collision.gameObject);
    }
  }

  public void OnCollisionExit(Collision collision)
  {
    enemiesInRadius.Remove(collision.gameObject);
  }

  public int AmountOfMoney
  {
    get { return mAmountOfMoney; }
  }
}