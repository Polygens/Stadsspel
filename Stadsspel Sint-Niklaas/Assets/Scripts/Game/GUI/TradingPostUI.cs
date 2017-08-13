using Stadsspel.Elements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradingPostUI : MonoBehaviour
{
	private Text m_TotalPriceText;
	private int m_TotalPriceAmount;
	private List<InputField> m_Inputfields = new List<InputField>();
	private List<Text> m_TotalTextFields = new List<Text>();
	private GameObject m_MessagePanel;

	//private List<Item> items;
	//SyncListInt visitedTeams = new SyncListInt();
	private Item m_LegalShopItem;
	private Item m_IllegalShopItem;

	private bool m_EverythingIsInstantiated = false;
	private int type, legalNumberOfItems, illegalNumberOfItems;

	public InputField legalField, illegalField;
	private int[] m_NumberOfEachItem;

	public bool IsVisited {
		get {
			throw new System.NotImplementedException();
		}

		set {
		}
	}

	public GameObject MessagePanel {
		get {
			return m_MessagePanel;
		}
	}

	public Text MessagePanelText {
		get {
			return (Text)m_MessagePanel.GetComponentInChildren(typeof(Text), true);
		}
	}

	/// <summary>
	/// Initialises the class.
	/// </summary>
	private void Start()
	{
		//if (!isLocalPlayer)
		//{
		//  TradingPostPanel.gameObject.SetActive(false);
		//  return;
		//}

		m_MessagePanel = transform.Find("MessagePanel").gameObject;

		//RectTransform Grid = (RectTransform)transform.FindChild("MainPanel").transform.FindChild("Grid");
		m_TotalPriceText = transform.Find("MainPanel").transform.Find("InfoPanelTop").transform.Find("BuyPanel").transform.Find("AmountOfGoods").GetComponent<Text>();
		transform.Find("MainPanel").transform.Find("InfoPanelTop").transform.Find("MoneyPanel").transform.Find("AmountOfMoney").GetComponent<Text>().text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();

		LoadItems();
		//OnEnable();//todo extra call to instantiate player money?
		m_EverythingIsInstantiated = true;

		//TradingPostPanel.gameObject.SetActive(false);
	}

	private void LoadItems()
	{
		//items = new List<Item>();


		//load items
		ServerTradePost stp = CurrentGame.Instance.gameDetail.tradePosts.Find(tp => tp.id.Equals(CurrentGame.Instance.nearTP));
		//items = new List<Item>();
		foreach (ServerTradePost.ServerItem serverItem in stp.items)
		{
			if (!CurrentGame.Instance.KnownItems.ContainsKey(serverItem.name))
			{
				CurrentGame.Instance.KnownItems.Add(serverItem.name, serverItem);
			}
			m_LegalShopItem = new Item(serverItem.name, (int)serverItem.legalPurchase, (int)serverItem.legalSales, true);
			m_IllegalShopItem =new Item(serverItem.name, (int)serverItem.illegalPurchase, (int)serverItem.illegalSales, false);
		}
		//m_ShopItems = Item.ShopItems;
		//m_ShopItems = items;
		Transform wjd = transform.Find("WistJeDatPanel");
		if (wjd != null)
		{
			wjd.Find("PlaceHolder").Find("Message").gameObject.GetComponent<Text>().text = stp.flavorText;
		}


		RectTransform Grid = (RectTransform)transform.Find("MainPanel").transform.Find("Grid");
		transform.Find("MainPanel").transform.Find("NaamItem").GetComponent<Text>().text = m_LegalShopItem.ItemName;
		transform.Find("MainPanel").transform.Find("NaamTradingpost").GetComponent<Text>().text = stp.name;
		int index = 0;

		/*
		if (index >= items.Count)
		{
			index = 0;
		} else
		{
			Grid.GetChild(0).GetChild(j).transform.FindChild("ItemRow1").transform.FindChild("PrijsLabel").transform.FindChild("Prijs").GetComponent<Text>().text = m_IllegalShopItem.BuyPrice.ToString();
		}

		for (int j = 0; j < 2; j++)
		{
			//Grid.GetChild(0).GetChild(j).transform.Find("ItemRow1").Find("NaamItem").GetComponent<Text>().text = items[index].ItemName;
			Grid.GetChild(0).GetChild(j).transform.Find("ItemRow1").transform.Find("PrijsLabel").transform.Find("Prijs").GetComponent<Text>().text = items[index].BuyPrice.ToString();
			m_Inputfields.Add(Grid.GetChild(0).GetChild(j).transform.Find("InputField").GetComponent<InputField>());
			m_TotalTextFields.Add(Grid.GetChild(0).GetChild(j).transform.Find("ItemRow2").transform.Find("Totaal").GetComponent<Text>());
			index++;
		}
		*/

		Grid.GetChild(0).GetChild(0).transform.Find("ItemRow1").transform.Find("PrijsLabel").transform.Find("Prijs").GetComponent<Text>().text = m_LegalShopItem.BuyPrice.ToString();
		m_Inputfields.Add(Grid.GetChild(0).GetChild(0).transform.Find("InputField").GetComponent<InputField>());
		m_TotalTextFields.Add(Grid.GetChild(0).GetChild(0).transform.Find("ItemRow2").transform.Find("Totaal").GetComponent<Text>());

		Grid.GetChild(0).GetChild(1).transform.Find("ItemRow1").transform.Find("PrijsLabel").transform.Find("Prijs").GetComponent<Text>().text = m_IllegalShopItem.BuyPrice.ToString();
		m_Inputfields.Add(Grid.GetChild(0).GetChild(1).transform.Find("InputField").GetComponent<InputField>());
		m_TotalTextFields.Add(Grid.GetChild(0).GetChild(1).transform.Find("ItemRow2").transform.Find("Totaal").GetComponent<Text>());


		m_NumberOfEachItem = new int[2];
	}

	/// <summary>
	/// Checks and returns if the player's team has already visited.
	/// </summary>
	private bool CheckIfTeamAlreadyVisited()
	{
		/*moved to server
		bool teamAlreadyVisited = false;
		GameObject tempTradePost = GameManager.s_Singleton.Player.GetComponent<Player>().GetGameObjectInRadius("TradingPost");
#if (UNITY_EDITOR)
		Debug.Log(tempTradePost.name);
		Debug.Log(tempTradePost.GetComponent<TradingPost>().VisitedTeams.Count);
#endif
		List<int> visitedTeams = tempTradePost.GetComponent<TradingPost>().VisitedTeams;
		for (int i = 0; i < visitedTeams.Count; i++)
		{
			if (visitedTeams[i] == CurrentGame.Instance.gameDetail.IndexOfTeam(GameManager.s_Singleton.Player.Person.Team) + 1)
			{//todo this does not seem right, what is being compared?
				teamAlreadyVisited = true;
				break;
			}
		}
		return teamAlreadyVisited;
		*/
		return false;
	}

	/// <summary>
	/// Gets called when the GameObject becomes active.
	/// </summary>
	private void OnEnable()
	{
		legalNumberOfItems = 0;
		illegalNumberOfItems = 0;
		m_TotalPriceAmount = 0;
		legalField.text = "";
		illegalField.text = "";
		GameObject tempTradePost = GameManager.s_Singleton.Player.GetComponent<Player>().GetGameObjectInRadius("TradingPost");
		transform.Find("MainPanel").Find("NaamItem").GetComponent<Text>().text = "";
		transform.Find("MainPanel").Find("NaamTradingpost").GetComponent<Text>().text = "";
		if (CheckIfTeamAlreadyVisited())
		{
			//Start, + property
			m_MessagePanel.SetActive(true);
		} else
		{
			if (m_MessagePanel != null)
			{
				if (m_MessagePanel.activeSelf) m_MessagePanel.SetActive(false);
			}

		}
		if (m_EverythingIsInstantiated)
		{
			LoadItems();
			transform.Find("MainPanel").transform.Find("InfoPanelTop").transform.Find("MoneyPanel").transform.Find("AmountOfMoney").GetComponent<Text>().text = CurrentGame.Instance.LocalPlayer.money + "";
		}

	}

	public void MaxButton(int type)
	{
		if (m_TotalPriceAmount < (int)CurrentGame.Instance.LocalPlayer.money)
		{
			int remainingMoney = 0;
			if (type == 0) // LEGAL
			{
				if (illegalField.text != "")
				{
					remainingMoney = (int)CurrentGame.Instance.LocalPlayer.money - (int.Parse(illegalField.text) * m_IllegalShopItem.BuyPrice);
				} else
				{
					remainingMoney = (int)CurrentGame.Instance.LocalPlayer.money;
				}
				int numberOfItems = Mathf.FloorToInt(remainingMoney / m_LegalShopItem.BuyPrice);
				legalNumberOfItems = numberOfItems;
				m_NumberOfEachItem[0] = legalNumberOfItems;
				legalField.text = numberOfItems.ToString();
			} else
			{
				if (legalField.text != "")
				{
					remainingMoney = (int)CurrentGame.Instance.LocalPlayer.money - (int.Parse(legalField.text) * m_LegalShopItem.BuyPrice);
				} else
				{
					remainingMoney = GameManager.s_Singleton.Player.Person.AmountOfMoney;
				}
				int numberOfItems = Mathf.FloorToInt(remainingMoney / m_IllegalShopItem.BuyPrice);
				illegalField.text = numberOfItems.ToString();
				illegalNumberOfItems = numberOfItems;
				m_NumberOfEachItem[1] = illegalNumberOfItems;
			}
		}
	}

	/// <summary>
	/// todo message to server?
	/// </summary>
	public void Purchase()
	{
		//if (!isLocalPlayer)
		//{
		//  return;
		//}
		if (!CheckIfTeamAlreadyVisited())
		{
			//if (GameManager.s_Singleton.Player.Person.AmountOfMoney >= m_TotalPriceAmount)
			if (CurrentGame.Instance.LocalPlayer.money + 0.0000001 >= m_TotalPriceAmount)
			{
				AddGoodsToPlayer();
				gameObject.SetActive(false);
			} else
			{
#if (UNITY_EDITOR)
				Debug.Log("Not enough money"); //todo g: maybe change color of total/player money to red is they don't have enough?
#endif
			}
		} else
		{
			transform.Find("MessagePanel").gameObject.SetActive(true);
		}
	}

	//Execute when items are purchased (button holds this method)

	/// <summary>
	/// todo server should do dis
	/// </summary>
	public void AddGoodsToPlayer()
	{
		IDictionary<string, int> legalItems = new Dictionary<string, int>();
		IDictionary<string, int> illegalItems = new Dictionary<string, int>();


		for (int i = 0; i < m_NumberOfEachItem.Length; i++)
		{
			if (i==0)
			{//legal
				if (m_NumberOfEachItem[i] > 0)
				{
					legalItems.Add(m_LegalShopItem.ItemName, m_NumberOfEachItem[i]);
				}
			} else
			{
				if (m_NumberOfEachItem[i] > 0)
				{
					illegalItems.Add(m_IllegalShopItem.ItemName, m_NumberOfEachItem[i]);
				}
			}
		}

		for (int i = 0; i < m_Inputfields.Count; i++)
		{
			m_Inputfields[i].text = "";
			m_TotalTextFields[i].text = "Totaal: 0";
		}

		if (legalItems.Count > 0)
		{
			CurrentGame.Instance.Ws.SendTradepostLegalPurchase(legalItems, CurrentGame.Instance.nearTP);
		}
		if (illegalItems.Count > 0)
		{
			CurrentGame.Instance.Ws.SendTradepostIllegalPurchase(illegalItems, CurrentGame.Instance.nearTP);
		}

		m_TotalPriceText.text = "0";
		m_TotalPriceAmount = 0;

	}

	/// <summary>
	/// 
	/// </summary>
	public void OnClose()
	{
		transform.Find("MessagePanel").gameObject.SetActive(false);
	}

	//For drop down or inputField
	/// <summary>
	/// 
	/// </summary>
	public void UpdateNumberOfGoods(string number)
	{
		int focusedIndex = 0;
		int result = 0;
		int itemCount = 0;
		if (number == "")
		{
			number = "0";
		}
		for (int i = 0; i < m_Inputfields.Count; i++)
		{
			if (m_Inputfields[i].isFocused)
			{//todo clear memory drain at m_inputfields
				if (int.TryParse(number, out result))
				{
					Debug.Log(i);
					Debug.Log(i%m_NumberOfEachItem.Length);
					m_NumberOfEachItem[i%m_NumberOfEachItem.Length] = result;
					itemCount += m_NumberOfEachItem[i%m_NumberOfEachItem.Length];
				}
				focusedIndex = i;
			}
		}

		int tempTotal = 0;
		for (int i = 0; i < m_NumberOfEachItem.Length; i++)
		{
			if (i==0)
			{//legal
				tempTotal += (m_NumberOfEachItem[i] * m_LegalShopItem.BuyPrice);
				m_TotalTextFields[i].text = "Totaal: " + (m_NumberOfEachItem[i] * m_LegalShopItem.BuyPrice);
			}
			else
			{
				tempTotal += (m_NumberOfEachItem[i] * m_IllegalShopItem.BuyPrice);
				m_TotalTextFields[i].text = "Totaal: " + (m_NumberOfEachItem[i] * m_IllegalShopItem.BuyPrice);
			}
		}


		m_TotalPriceAmount = tempTotal;
		m_TotalPriceText.text = tempTotal.ToString();
		//transform.Find("MainPanel").transform.Find("InfoPanelTop").transform.Find("AmountOfGoods").GetComponent<Text>().text="";

		//m_TotalTextFields[0].text = "Totaal: " + legalItemTotal.ToString();
		//m_TotalTextFields[1].text = "Totaal: " + illegalItemTotal.ToString();
	}

}
