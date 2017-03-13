using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Stadsspel.Elements;

public class TradingPostUI : MonoBehaviour
{
	private RectTransform m_TradingPostPanel;
	private Text m_TotalPriceText;
	private int m_TotalPriceAmount;
	private List<InputField> m_Inputfields = new List<InputField>();
	private List<Text> m_TotalTextFields = new List<Text>();
  private GameObject messagePanel;

	//SyncListInt visitedTeams = new SyncListInt();
	private List<Item> m_ShopItems = new List<Item>();

	private int[] m_NumberOfEachItem;
	private bool m_EverythingIsInstantiated = false;

	public bool IsVisited {
		get {
			throw new System.NotImplementedException();
		}

		set {
		}
	}

  public GameObject MessagePanel
  {
    get
    {
      return messagePanel;
    }
  }

  public Text MessagePanelText
  {
    get
    {
      return (Text)messagePanel.GetComponentInChildren(typeof(Text),true);
    }
  }

  private void Start()
	{
		m_TradingPostPanel = (RectTransform)GameObject.FindWithTag("Canvas").transform.FindChild("Panels").transform.FindChild("TradingPost");

    //if (!isLocalPlayer)
    //{
    //  TradingPostPanel.gameObject.SetActive(false);
    //  return;
    //}

    messagePanel = transform.FindChild("MessagePanel").gameObject;
    

    m_ShopItems = Item.ShopItems;

		RectTransform Grid = (RectTransform)m_TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
		m_TotalPriceText = m_TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("BuyPanel").transform.FindChild("AmountOfGoods").GetComponent<Text>();
		m_TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("MoneyPanel").transform.FindChild("AmountOfMoney").GetComponent<Text>().text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		int childsInGrid = Grid.childCount;
		int index = 0;
		for(int i = 1; i < childsInGrid; i++) {
			for(int j = 0; j < 2; j++) {
				Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow1").transform.FindChild("PrijsLabel").transform.FindChild("Prijs").GetComponent<Text>().text = m_ShopItems[index].BuyPrice.ToString();
				m_Inputfields.Add(Grid.GetChild(i).GetChild(j).transform.FindChild("InputField").GetComponent<InputField>());
				m_TotalTextFields.Add(Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Totaal").GetComponent<Text>());
				index++;
			}
		}
		m_NumberOfEachItem = new int[m_ShopItems.Count];
		m_EverythingIsInstantiated = true;

		//TradingPostPanel.gameObject.SetActive(false);
	}

	private bool CheckIfTeamAlreadyVisited()
	{
		bool teamAlreadyVisited = false;
		GameObject tempTradePost = GameManager.s_Singleton.Player.GetComponent<Player>().GetGameObjectInRadius("TradingPost");

		Debug.Log(tempTradePost.name);
		Debug.Log(tempTradePost.GetComponent<TradingPost>().VisitedTeams.Count);

		List<int> visitedTeams = tempTradePost.GetComponent<TradingPost>().VisitedTeams;
		for(int i = 0; i < visitedTeams.Count; i++)
        {
			if(visitedTeams[i] == (int)GameManager.s_Singleton.Player.Person.Team)
            {
				teamAlreadyVisited = true;
                break;
			}
		}
		return teamAlreadyVisited;
	}

	private void OnEnable()
	{
        if (CheckIfTeamAlreadyVisited())
        {
            //Start, + property
            messagePanel.SetActive(true);
        }
        else
        {
            if (messagePanel.activeSelf)
                messagePanel.SetActive(false);
        }
        if (m_EverythingIsInstantiated)
        {
            m_TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("MoneyPanel").transform.FindChild("AmountOfMoney").GetComponent<Text>().text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
        }
    }

	public void Purchase()
	{
		//if (!isLocalPlayer)
		//{
		//  return;
		//}
		if(!CheckIfTeamAlreadyVisited()) {
			if(GameManager.s_Singleton.Player.Person.AmountOfMoney >= m_TotalPriceAmount) {
				AddGoodsToPlayer();
				m_TradingPostPanel.gameObject.SetActive(false);
			} else {
				Debug.Log("Not enough money");
			}
		} else {
			m_TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(true);
		}
	}

	//Execute when items are purchased (button holds this method)

	public void AddGoodsToPlayer()
	{
		List<int> legalItems = new List<int>();
		List<int> illegalItems = new List<int>();
		for(int i = 0; i < m_NumberOfEachItem.Length; i++) {
			if(m_ShopItems[i].IsLegal) {
				legalItems.Add(m_NumberOfEachItem[i]);
			} else {
				illegalItems.Add(m_NumberOfEachItem[i]);
			}
		}
		GameManager.s_Singleton.Player.Person.AddLegalItems(legalItems);
		GameManager.s_Singleton.Player.Person.AddIllegalItems(illegalItems);
        GameManager.s_Singleton.Player.GetGameObjectInRadius("TradingPost").GetComponent<TradingPost>().GetComponent<PhotonView>().RPC("AddTeamToList", PhotonTargets.All, (int)GameManager.s_Singleton.Player.Person.Team);
		GameManager.s_Singleton.Player.Person.MoneyTransaction(-m_TotalPriceAmount);

		for(int i = 0; i < m_Inputfields.Count; i++) {
			m_Inputfields[i].text = "";
			m_TotalTextFields[i].text = "Totaal: 0";
		}
		m_TotalPriceText.text = "0";
		m_TotalPriceAmount = 0;
	}

	public void OnClose()
	{
		m_TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(false);
	}

	//For drop down or inputField
	public void UpdateNumberOfGoods(string number)
	{
		int focusedIndex = 0;
		int result = 0;
		if(number == "") {
			number = "0";
		}
		for(int i = 0; i < m_Inputfields.Count; i++) {
			if(m_Inputfields[i].isFocused) {
				bool isNumber = int.TryParse(number, out result);
				if(isNumber) {
					m_NumberOfEachItem[i] = result;
				}
				focusedIndex = i;
			}
		}
		int tempTotal = 0;
		for(int i = 0; i < m_NumberOfEachItem.Length; i++) {
			tempTotal += (m_NumberOfEachItem[i] * m_ShopItems[i].BuyPrice);
		}

		m_TotalPriceAmount = tempTotal;
		m_TotalPriceText.text = tempTotal.ToString();
		int itemTotal = (result * m_ShopItems[focusedIndex].BuyPrice);
		m_TotalTextFields[focusedIndex].text = "Totaal: " + itemTotal.ToString();
	}

}
