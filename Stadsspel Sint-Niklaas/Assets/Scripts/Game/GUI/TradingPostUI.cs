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

	//SyncListInt visitedTeams = new SyncListInt();
	private Item m_LegalShopItem;
	private Item m_IllegalShopItem;

	private bool m_EverythingIsInstantiated = false;
	private int type, legalNumberOfItems, illegalNumberOfItems;

	public InputField legalField, illegalField;

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

		m_MessagePanel = transform.FindChild("MessagePanel").gameObject;


		InitializeUI();
		
		m_EverythingIsInstantiated = true;

		//TradingPostPanel.gameObject.SetActive(false);
	}

	private void InitializeUI()
	{
		type = (int)GameManager.s_Singleton.Player.GetComponent<Player>().GetGameObjectInRadius("TradingPost").GetComponent<TradingPost>().tradingpostType;

		m_LegalShopItem = Item.LegalShopItems[type];
		m_IllegalShopItem = Item.IllegalShopItems[type];

		RectTransform Grid = (RectTransform)transform.FindChild("MainPanel").transform.FindChild("Grid");
		m_TotalPriceText = transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("BuyPanel").transform.FindChild("AmountOfGoods").GetComponent<Text>();
		transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("MoneyPanel").transform.FindChild("AmountOfMoney").GetComponent<Text>().text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();

		for (int j = 0; j < 2; j++)
		{
			if (j == 0)
			{
				Grid.GetChild(0).GetChild(j).transform.FindChild("ItemRow1").transform.FindChild("PrijsLabel").transform.FindChild("Prijs").GetComponent<Text>().text = m_LegalShopItem.BuyPrice.ToString();
			}
			else
			{
				Grid.GetChild(0).GetChild(j).transform.FindChild("ItemRow1").transform.FindChild("PrijsLabel").transform.FindChild("Prijs").GetComponent<Text>().text = m_IllegalShopItem.BuyPrice.ToString();
			}

			m_Inputfields.Add(Grid.GetChild(0).GetChild(j).transform.FindChild("InputField").GetComponent<InputField>());
			m_TotalTextFields.Add(Grid.GetChild(0).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Totaal").GetComponent<Text>());
		}
	}

	/// <summary>
	/// Checks and returns if the player's team has already visited.
	/// </summary>
	private bool CheckIfTeamAlreadyVisited()
	{
		bool teamAlreadyVisited = false;
		GameObject tempTradePost = GameManager.s_Singleton.Player.GetComponent<Player>().GetGameObjectInRadius("TradingPost");
#if(UNITY_EDITOR)
		Debug.Log(tempTradePost.name);
		Debug.Log(tempTradePost.GetComponent<TradingPost>().VisitedTeams.Count);
#endif
		List<int> visitedTeams = tempTradePost.GetComponent<TradingPost>().VisitedTeams;
		for(int i = 0; i < visitedTeams.Count; i++) {
			if(visitedTeams[i] == (int)GameManager.s_Singleton.Player.Person.Team) {
				teamAlreadyVisited = true;
				break;
			}
		}
		return teamAlreadyVisited;
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
		transform.GetChild(0).FindChild("NaamItem").GetComponent<Text>().text = tempTradePost.GetComponent<TradingPost>().tradingpostType.ToString();
		transform.GetChild(0).FindChild("NaamTradingpost").GetComponent<Text>().text = tempTradePost.GetComponent<TradingPost>().naamTradingpost;
		if (CheckIfTeamAlreadyVisited()) {
			//Start, + property
			m_MessagePanel.SetActive(true);
		}
		else {
			if(m_MessagePanel != null) {
				if(m_MessagePanel.activeSelf)
					m_MessagePanel.SetActive(false);
			}

		}
		if(m_EverythingIsInstantiated) {
			InitializeUI();
			transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("MoneyPanel").transform.FindChild("AmountOfMoney").GetComponent<Text>().text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		}
		
	}

	public void MaxButton(int type)
	{
		if (m_TotalPriceAmount < GameManager.s_Singleton.Player.Person.AmountOfMoney)
		{
			int remainingMoney = GameManager.s_Singleton.Player.Person.AmountOfMoney - m_TotalPriceAmount;
			if (type == 0) // LEGAL
			{
				int numberOfItems = Mathf.FloorToInt(remainingMoney / m_LegalShopItem.BuyPrice);
				legalNumberOfItems = numberOfItems;
				legalField.text = numberOfItems.ToString();
			}
			else
			{
				int numberOfItems = Mathf.FloorToInt(remainingMoney / m_IllegalShopItem.BuyPrice);
				illegalField.text = numberOfItems.ToString();
				illegalNumberOfItems = numberOfItems;
			}
		}
	}

	/// <summary>
	/// TODO
	/// </summary>
	public void Purchase()
	{
		//if (!isLocalPlayer)
		//{
		//  return;
		//}
		if(!CheckIfTeamAlreadyVisited()) {
			if(GameManager.s_Singleton.Player.Person.AmountOfMoney >= m_TotalPriceAmount) {
				AddGoodsToPlayer();
				gameObject.SetActive(false);
			}
			else {
#if(UNITY_EDITOR)
				Debug.Log("Not enough money");
#endif
			}
		}
		else {
			transform.FindChild("MessagePanel").gameObject.SetActive(true);
		}
	}

	//Execute when items are purchased (button holds this method)

	/// <summary>
	/// TODO
	/// </summary>
	public void AddGoodsToPlayer()
	{
		GameManager.s_Singleton.Player.Person.GetComponent<PhotonView>().RPC("AddLegalItem", PhotonTargets.All, type, legalNumberOfItems);
		GameManager.s_Singleton.Player.Person.GetComponent<PhotonView>().RPC("AddIllegalItem", PhotonTargets.All, type, illegalNumberOfItems);

		GameManager.s_Singleton.Player.GetGameObjectInRadius("TradingPost").GetComponent<TradingPost>().GetComponent<PhotonView>().RPC("AddTeamToList", PhotonTargets.All, (int)GameManager.s_Singleton.Player.Person.Team);
		GameManager.s_Singleton.Player.Person.photonView.RPC("MoneyTransaction", PhotonTargets.AllViaServer, -m_TotalPriceAmount);

		for(int i = 0; i < m_Inputfields.Count; i++) {
			m_Inputfields[i].text = "";
			m_TotalTextFields[i].text = "Totaal: 0";
		}

		m_TotalPriceText.text = "0";
		m_TotalPriceAmount = 0;
	}

	/// <summary>
	/// TODO
	/// </summary>
	public void OnClose()
	{
		transform.FindChild("MessagePanel").gameObject.SetActive(false);
	}

	//For drop down or inputField
	/// <summary>
	/// TODO
	/// </summary>
	public void UpdateNumberOfGoods(string number)
	{
		string numberLegal = legalField.text;
		int resultLegal;
		int.TryParse(numberLegal,out resultLegal);
		legalNumberOfItems = resultLegal;
		int legalItemTotal = (resultLegal * m_LegalShopItem.BuyPrice);

		string numberIllegal = illegalField.text;
		int resultIllegal;
		int.TryParse(numberIllegal,out resultIllegal);
		illegalNumberOfItems = resultIllegal;
		int illegalItemTotal = (resultIllegal * m_IllegalShopItem.BuyPrice);


		int tempTotal = 0;
		tempTotal = legalItemTotal + illegalItemTotal; 


		m_TotalPriceAmount = tempTotal;
		m_TotalPriceText.text = tempTotal.ToString();
		
		m_TotalTextFields[0].text = "Totaal: " + legalItemTotal.ToString();
		m_TotalTextFields[1].text = "Totaal: " + illegalItemTotal.ToString();
	}

}
