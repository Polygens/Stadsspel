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

	private string[] indexStrings = new[] {"TOP", "Dozijn bloemen", "Vat bier", "Kg ijs", "Rol textiel", "Koets bakstenen", "Art deco" };

	private void Start()
	{
		m_MarktPanel = (RectTransform)InGameUIManager.s_Singleton.GrandMarketUI.transform; 
	}

	/// <summary>
	/// Gets called when the GameObject becomes active.
	/// </summary>
	private void OnEnable()
	{
		
		m_Total = 0;
		//m_LegalItems = GameManager.s_Singleton.Player.Person.LookUpLegalItems;
		//m_IllegalItems = GameManager.s_Singleton.Player.Person.LookUpIllegalItems;

		RectTransform Grid = (RectTransform)transform.Find("MainPanel").transform.Find("Grid");
		for (int i = 1; i < Grid.childCount; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				if (j == 0)
				{
					Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = "0";
				}
				else
				{
					Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = "0";
				}
			}
		}

		m_TotalUI.text = "Totaal: 0";
		UpdateUI();
	}

	/// <summary>
	/// Updates the grand market UI. Fills in all the player's goods.
	/// </summary>
	private void UpdateUI()
	{
		m_Total = 0;

		legalItems = CurrentGame.Instance.LocalPlayer.legalItems;
		illegalItems = CurrentGame.Instance.LocalPlayer.illegalItems;

		m_MarktPanel = (RectTransform)InGameUIManager.s_Singleton.GrandMarketUI.transform;
		RectTransform Grid = (RectTransform)m_MarktPanel.transform.Find("MainPanel").transform.Find("Grid");
		

		for (int i = 1; i < Grid.childCount; i++)
		{ // i is which row 
			string rowItem = indexStrings[i];
			for (int j = 0; j < 2; j++)
			{
				// J is for legal or illegal
				Grid.GetChild(i).GetChild(j).gameObject.SetActive(true);

				int subTotal = 0;
				if (j == 0)
				{
					if (legalItems.ContainsKey(rowItem))
					{
						Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = legalItems[rowItem]+"";
						subTotal = CalculateSubtotal(rowItem, true);
					}
				} else
				{
					if (illegalItems.ContainsKey(rowItem))
					{
						Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = illegalItems[rowItem]+"";
						subTotal = CalculateSubtotal(rowItem, false);
					}
				}
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
				Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = "0";
				Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = "0";
			}
		}
		InGameUIManager.s_Singleton.GoodsUI.ResetUI();
		m_TotalUI.text = "Totaal: " + m_Total;
		m_MarktPanel.gameObject.SetActive(false);
	}

}

