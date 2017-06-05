using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GrandMarketUI : MonoBehaviour
{

	private RectTransform m_MarktPanel;
	[SerializeField]
	private Text m_TotalUI;
	private IDictionary<string, int> legalItems;
	private IDictionary<string, int> illegalItems;
	private int m_Total;

	/// <summary>
	/// Gets called when the GameObject becomes active.
	/// </summary>
	private void OnEnable()
	{
		UpdateUI();
	}

	/// <summary>
	/// Updates the grand market UI. Fills in all the player's goods.
	/// </summary>
	private void UpdateUI()
	{
		m_Total = 0;

		legalItems = CurrentGame.Instance.LocalPlayer.legalItems;
		List<string> legalKeys = legalItems.Keys.ToList();

		illegalItems = CurrentGame.Instance.LocalPlayer.illegalItems;
		List<string> illegalKeys = illegalItems.Keys.ToList();


		m_MarktPanel = (RectTransform)InGameUIManager.s_Singleton.GrandMarketUI.transform;
		RectTransform Grid = (RectTransform)m_MarktPanel.transform.Find("MainPanel").transform.Find("Grid");

		int legalIndex = 0;
		int illegalIndex = 0;
		//int indexLegal = 0;  
		//int indexIllegal = 0;

		for (int i = 1; i < Grid.childCount; i++)
		{ // i is which row 
			for (int j = 0; j < 2; j++)
			{
				// J is for legal or illegal 
				Grid.GetChild(i).GetChild(j).gameObject.SetActive(true);

				int subTotal = 0;
				if (j == 0)
				{
					if (legalIndex >= legalItems.Count)
					{
						Grid.GetChild(i).GetChild(j).gameObject.SetActive(false);
					} else
					{
						Grid.GetChild(i).GetChild(j).transform.Find("ItemRow1").transform.Find("NaamItem").GetComponent<Text>().text = legalKeys[legalIndex];
						Grid.GetChild(i).GetChild(j).transform.Find("ItemRow1").transform.Find("PrijsLabel").transform.Find("Prijs").GetComponent<Text>().text = CurrentGame.Instance.KnownItems[legalKeys[legalIndex]].legalSales.ToString();
						Grid.GetChild(i).GetChild(j).transform.Find("ItemRow2").transform.Find("Amount").GetComponent<Text>().text = "Amount: " + legalItems[legalKeys[legalIndex]];
						subTotal = CalculateSubtotal(legalKeys[legalIndex], true);
						legalIndex++;
					}
				} else
				{
					if (illegalIndex >= illegalItems.Count)
					{
						Grid.GetChild(i).GetChild(j).gameObject.SetActive(false);
					} else
					{
						Grid.GetChild(i).GetChild(j).transform.Find("ItemRow1").transform.Find("NaamItem").GetComponent<Text>().text = illegalKeys[illegalIndex];
						Grid.GetChild(i).GetChild(j).transform.Find("ItemRow1").transform.Find("PrijsLabel").transform.Find("Prijs").GetComponent<Text>().text = CurrentGame.Instance.KnownItems[illegalKeys[illegalIndex]].illegalSales.ToString();
						Grid.GetChild(i).GetChild(j).transform.Find("ItemRow2").transform.Find("Amount").GetComponent<Text>().text = "Amount: " + illegalItems[illegalKeys[illegalIndex]];
						subTotal = CalculateSubtotal(illegalKeys[illegalIndex], false);
						illegalIndex++;
					}
				}
				Grid.GetChild(i).GetChild(j).transform.Find("ItemRow2").transform.Find("Profit").GetComponent<Text>().text = "Winst: " + subTotal;
				m_Total += subTotal;
			}
			m_TotalUI.text = "Totaal: " + m_Total;
		}
	}


	/// <summary>
	/// Returns and calculates the sum of all the goods passed.
	/// </summary>
	private int CalculateSubtotal(string itemName, bool isLegal)
	{
		int amount = isLegal ? legalItems[itemName] : illegalItems[itemName];
		double price = isLegal ? CurrentGame.Instance.KnownItems[itemName].legalSales : CurrentGame.Instance.KnownItems[itemName].illegalSales;
		return (int)(amount * price);
	}

	/// <summary>
	/// Performs the operations to sell all the player's goods. Perform transaction, reset inventory and update UI.
	/// </summary>
	public void Sell()
	{
		CurrentGame.Instance.Ws.SendTradepostAllSale(new Dictionary<string, int>(),CurrentGame.Instance.currentDistrictID);
		m_Total = 0;
		UpdateUI();
		ResetUI();
	}

	/// <summary>
	/// Resets the UI to the default values.
	/// </summary>
	private void ResetUI()
	{
		RectTransform Grid = (RectTransform)m_MarktPanel.transform.Find("MainPanel").transform.Find("Grid");
		for (int i = 1; i < Grid.childCount; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				Grid.GetChild(i).GetChild(j).transform.Find("ItemRow2").transform.Find("Amount").GetComponent<Text>().text = "Amount: 0";
				Grid.GetChild(i).GetChild(j).transform.Find("ItemRow2").transform.Find("Profit").GetComponent<Text>().text = "Winst: 0";
			}
		}

		m_TotalUI.text = "Totaal: " + m_Total;
		m_MarktPanel.gameObject.SetActive(false);
	}

}

