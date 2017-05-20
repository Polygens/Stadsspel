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


		m_ShopItems = Item.ShopItems;

		RectTransform Grid = (RectTransform)transform.FindChild("MainPanel").transform.FindChild("Grid");
		m_TotalPriceText = transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("BuyPanel").transform.FindChild("AmountOfGoods").GetComponent<Text>();
		transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("MoneyPanel").transform.FindChild("AmountOfMoney").GetComponent<Text>().text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
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
			if(visitedTeams[i] == CurrentGame.Instance.gameDetail.IndexOfTeam(GameManager.s_Singleton.Player.Person.Team)+1) {//todo this does not seem right, what is being compared?
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
		if(CheckIfTeamAlreadyVisited()) {
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
			transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("MoneyPanel").transform.FindChild("AmountOfMoney").GetComponent<Text>().text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
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
	/// TODO server should do dis
	/// </summary>
	public void AddGoodsToPlayer()
	{
		/*
		List<int> legalItems = new List<int>();
		List<int> illegalItems = new List<int>();

		for(int i = 0; i < m_NumberOfEachItem.Length; i++) {
			if(m_ShopItems[i].IsLegal) {
				legalItems.Add(m_NumberOfEachItem[i]);

			}
			else {
				illegalItems.Add(m_NumberOfEachItem[i]);

			}
		}

		for(int i = 0; i < legalItems.Count; i++) {
			GameManager.s_Singleton.Player.Person.GetComponent<PhotonView>().RPC("AddLegalItem", PhotonTargets.All, i, legalItems[i]);
			Debug.Log("Add legal item at index: " + i);
		}
		for(int i = 0; i < illegalItems.Count; i++) {
			GameManager.s_Singleton.Player.Person.GetComponent<PhotonView>().RPC("AddIllegalItem", PhotonTargets.All, i, illegalItems[i]);
			Debug.Log("Add illegal item at index: " + i);
		}

		GameManager.s_Singleton.Player.GetGameObjectInRadius("TradingPost").GetComponent<TradingPost>().GetComponent<PhotonView>().RPC("AddTeamToList", PhotonTargets.All, (int)GameManager.s_Singleton.Player.Person.Team);
		GameManager.s_Singleton.Player.Person.photonView.RPC("MoneyTransaction", PhotonTargets.AllViaServer, -m_TotalPriceAmount);

		for(int i = 0; i < m_Inputfields.Count; i++) {
			m_Inputfields[i].text = "";
			m_TotalTextFields[i].text = "Totaal: 0";
		}

		m_TotalPriceText.text = "0";
		m_TotalPriceAmount = 0;
		*/
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
