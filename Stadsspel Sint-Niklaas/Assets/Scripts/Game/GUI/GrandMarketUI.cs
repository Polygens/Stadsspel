using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrandMarketUI : MonoBehaviour
{

	private RectTransform m_MarktPanel;
	[SerializeField]
	private Text m_TotalUI;
	private List<int> m_IllegalItems = new List<int>();
	private List<int> m_LegalItems = new List<int>();
	private int m_Total;

	/// <summary>
	/// TODO
	/// </summary>
	private void OnEnable()
	{
		UpdateUI();
	}

	/// <summary>
	/// TODO
	/// </summary>
	private void UpdateUI()
	{
		m_Total = 0;

		m_LegalItems = GameManager.s_Singleton.Player.Person.LookUpLegalItems;
		m_IllegalItems = GameManager.s_Singleton.Player.Person.LookUpIllegalItems;

		m_MarktPanel = (RectTransform)InGameUIManager.s_Singleton.GrandMarketUI.transform;
		RectTransform Grid = (RectTransform)m_MarktPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
		int index = 0;
		//int indexLegal = 0;  
		//int indexIllegal = 0;  

		for(int i = 1; i < Grid.childCount; i++) { // i is which row 
			for(int j = 0; j < 2; j++) { // J is for legal or illegal 
				int subTotal = 0;
				Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow1").transform.FindChild("PrijsLabel").transform.FindChild("Prijs").GetComponent<Text>().text = Item.ShopItems[index].SellPrice.ToString();
				if(j == 0) {
					Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Amount").GetComponent<Text>().text = "Amount: " + m_LegalItems[i - 1].ToString();
					subTotal = CalculateSubtotal(i - 1, index, m_LegalItems);
					//indexLegal++;  
				}
				else {
					Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Amount").GetComponent<Text>().text = "Amount: " + m_IllegalItems[i - 1].ToString();
					subTotal = CalculateSubtotal(i - 1, index, m_IllegalItems);
					//indexIllegal++;  
				}


				Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Profit").GetComponent<Text>().text = "Winst: " + subTotal;
				m_Total += subTotal;
				index++;

				m_TotalUI.text = "Totaal: " + m_Total;


			}
		}
	}

	/// <summary>
	/// TODO
	/// </summary>
	private int CalculateSubtotal(int Listindex, int index, List<int> items)
	{
		int sellPrice = Item.ShopItems[index].SellPrice;
		int subTotal = sellPrice * items[Listindex];
		return subTotal;
	}

	/// <summary>
	/// TODO
	/// </summary>
	public void Sell()
	{

		GameManager.s_Singleton.Player.Person.photonView.RPC("MoneyTransaction", PhotonTargets.AllViaServer, m_Total);
		GameManager.s_Singleton.Player.Person.ResetIllegalItems();
		GameManager.s_Singleton.Player.Person.ResetLegalItems();
		m_Total = 0;
		UpdateUI();
		ResetUI();
	}

	/// <summary>
	/// TODO
	/// </summary>
	private void ResetUI()
	{
		RectTransform Grid = (RectTransform)m_MarktPanel.transform.FindChild("MainPanel").transform.FindChild("Grid");
		for(int i = 1; i < Grid.childCount; i++) {
			for(int j = 0; j < 2; j++) {
				Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Amount").GetComponent<Text>().text = "Amount: 0";
				Grid.GetChild(i).GetChild(j).transform.FindChild("ItemRow2").transform.FindChild("Profit").GetComponent<Text>().text = "Winst: 0";
			}
		}

		m_TotalUI.text = "Totaal: " + m_Total;
		m_MarktPanel.gameObject.SetActive(false);
	}

}

