using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GoodsUI : MonoBehaviour
{
	private IDictionary<string, int> legalItems;
	private IDictionary<string, int> illegalItems;
	private string[] indexStrings = new[] { "TOP", "Dozijn bloemen", "Vat bier", "Kg ijs", "Rol textiel", "Koets bakstenen", "Art deco" };

	/// <summary>
	/// Gets called when the GameObject becomes active.
	/// </summary>
	private void OnEnable()
	{
		//copy from grand market ui
		legalItems = CurrentGame.Instance.LocalPlayer.legalItems;
		illegalItems = CurrentGame.Instance.LocalPlayer.illegalItems;

		RectTransform Grid = (RectTransform)transform.Find("MainPanel").transform.Find("Grid");

		int legalIndex = 0;
		int illegalIndex = 0;
		for (int i = 1; i < Grid.childCount; i++)
		{ // i is which row 
			string rowItem = indexStrings[i];
			for (int j = 0; j < 2; j++)
			{
				// J is for legal or illegal
				Grid.GetChild(i).GetChild(j).gameObject.SetActive(true);

				if (j == 0)
				{
					if (legalItems.ContainsKey(rowItem))
					{
						Debug.Log(legalItems[rowItem]);
						Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = legalItems[rowItem] + "";
						legalIndex++;
					}
				} else
				{
					if (illegalItems.ContainsKey(rowItem))
					{
						Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = illegalItems[rowItem] + "";
						illegalIndex++;
					}
				}
			}
		}
	}



	/// <summary>
	/// Resets the UI to the default values.
	/// </summary>
	public void ResetUI()
	{
		RectTransform Grid = (RectTransform)transform.Find("MainPanel").transform.Find("Grid");
		for (int i = 1; i < Grid.childCount; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = "0";
				Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = "0";
			}
		}
	}
}
