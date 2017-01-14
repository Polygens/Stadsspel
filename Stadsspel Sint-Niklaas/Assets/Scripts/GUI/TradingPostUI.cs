using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class TradingPostUI : MonoBehaviour
{
	private RectTransform TradingPostPanel;
	private Text totalPriceText;
	private int totalPriceAmount;
	private List<InputField> inputfields = new List<InputField>();
	private List<Text> totalTextFields = new List<Text>();

	//SyncListInt visitedTeams = new SyncListInt();
	private List<Item> shopItems = new List<Item>();

	private int[] numberOfEachItem;
	private bool everythingIsInstantiated = false;

	public void Start()
	{
		TradingPostPanel = (RectTransform)GameObject.FindWithTag("Canvas").transform.FindChild("Panels").transform.FindChild("TradingPost");

		//if (!isLocalPlayer)
		//{
		//  TradingPostPanel.gameObject.SetActive(false);
		//  return;
		//}

		if (CheckIfTeamAlreadyVisited()) {
			TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(true);
			return;
		}

		shopItems = Item.ShopItems;

		RectTransform Grid = (RectTransform)TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
		totalPriceText = TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("BuyPanel").transform.FindChild("AmountOfGoods").GetComponent<Text>();
		TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("MoneyPanel").transform.FindChild("AmountOfMoney").GetComponent<Text>().text = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AmountOfMoney.ToString();
		int childsInGrid = Grid.childCount;
		int index = 0;
		for (int i = 1; i < childsInGrid; i++) {
			for (int j = 0; j < 2; j++) {
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
		List<int> visitedTeams = tempTradePost.GetComponent<TradingPost>().VisitedTeams;
		for (int i = 0; i < visitedTeams.Count; i++) {
			if (visitedTeams[i] == (int)GameObject.FindWithTag("Player").GetComponent<Player>().Team) {
				teamAlreadyVisited = true;
			}
		}
		return teamAlreadyVisited;
	}

	public void OnEnable()
	{
		if (CheckIfTeamAlreadyVisited()) {
			TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(true);
			return;
		}
		if (everythingIsInstantiated) {
			TradingPostPanel.transform.FindChild("MainPanel").transform.FindChild("InfoPanelTop").transform.FindChild("MoneyPanel").transform.FindChild("AmountOfMoney").GetComponent<Text>().text = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AmountOfMoney.ToString();
		}
	}

	public void Purchase()
	{
		//if (!isLocalPlayer)
		//{
		//  return;
		//}
		if (!CheckIfTeamAlreadyVisited()) {
			if (GameObject.FindWithTag("Player").GetComponent<Player>().AmountOfMoney > totalPriceAmount) {
				AddGoodsToPlayer();
				TradingPostPanel.gameObject.SetActive(false);
			}
			else {
				Debug.Log("Not enough money");
			}
		}
		else {
			TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(true);
		}
	}

	//Execute when items are purchased (button holds this method)

	public void AddGoodsToPlayer()
	{
		List<int> legalItems = new List<int>();
		List<int> illegalItems = new List<int>();
		for (int i = 0; i < numberOfEachItem.Length; i++) {
			if (shopItems[i].IsLegal) {
				legalItems.Add(numberOfEachItem[i]);
			}
			else {
				illegalItems.Add(numberOfEachItem[i]);
			}
		}
		GameObject.FindWithTag("Player").GetComponent<Player>().AddLegalItems(legalItems);
		GameObject.FindWithTag("Player").GetComponent<Player>().AddIllegalItems(illegalItems);
		GameObject.FindWithTag("Player").GetComponent<Player>().GetTradingPost().GetComponent<TradingPost>().CmdAddTeamToList();
		GameObject.FindWithTag("Player").GetComponent<Player>().RemoveMoney(totalPriceAmount);

		for (int i = 0; i < inputfields.Count; i++) {
			inputfields[i].text = "";
			totalTextFields[i].text = "Totaal: 0";
		}
		totalPriceText.text = "0";
		totalPriceAmount = 0;
	}

	public void OnClose()
	{
		TradingPostPanel.transform.FindChild("MessagePanel").gameObject.SetActive(false);
	}

	//For drop down or inputField
	public void UpdateNumberOfGoods(string number)
	{
		int focusedIndex = 0;
		int result = 0;
		if (number == "") {
			number = "0";
		}
		for (int i = 0; i < inputfields.Count; i++) {
			if (inputfields[i].isFocused) {
				bool isNumber = int.TryParse(number, out result);
				if (isNumber) {
					numberOfEachItem[i] = result;
				}
				focusedIndex = i;
			}
		}
		int tempTotal = 0;
		for (int i = 0; i < numberOfEachItem.Length; i++) {
			tempTotal += (numberOfEachItem[i] * shopItems[i].BuyPrice);
		}

		totalPriceAmount = tempTotal;
		totalPriceText.text = tempTotal.ToString();
		int itemTotal = (result * shopItems[focusedIndex].BuyPrice);
		totalTextFields[focusedIndex].text = "Totaal: " + itemTotal.ToString();
	}

	public bool IsVisited {
		get {
			throw new System.NotImplementedException();
		}

		set {
		}
	}
}
