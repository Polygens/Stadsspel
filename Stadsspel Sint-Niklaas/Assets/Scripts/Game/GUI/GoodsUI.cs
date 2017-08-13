using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GoodsUI : MonoBehaviour
{
	private List<int> m_LegalItems = new List<int>();
	private List<int> m_IllegalItems = new List<int>();

	private IDictionary<string, int> legalItems;
	private IDictionary<string, int> illegalItems;
	private string[] indexStrings = new[] { "TOP", "Dozijn bloemen", "Vat bier", "Kg ijs", "Rol textiel", "Koets bakstenen", "Art deco" };

	/// <summary>
	/// Gets called when the GameObject becomes active.
	/// </summary>
	private void OnEnable()
	{
		m_LegalItems = GameManager.s_Singleton.Player.Person.LookUpLegalItems;

		m_IllegalItems = GameManager.s_Singleton.Player.Person.LookUpIllegalItems;

		//copy from grand market ui
		legalItems = CurrentGame.Instance.LocalPlayer.legalItems;
		List<string> legalKeys = legalItems.Keys.ToList();
		illegalItems = CurrentGame.Instance.LocalPlayer.illegalItems;
		List<string> illegalKeys = illegalItems.Keys.ToList();
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

				int subTotal = 0;
				if (j == 0)
				{
					if (legalItems.ContainsKey(rowItem))
					{
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





		/*todo legacy
		for(int i = 1; i < Grid.childCount; i++) {
			for(int j = 0; j < 2; j++) {
				if(j == 0) {
					Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = m_LegalItems[i - 1].ToString();
					Debug.Log("legal item: " + m_LegalItems[i - 1]);
				}
				else {
					Grid.GetChild(i).GetChild(j).transform.Find("Aantal").GetComponent<Text>().text = m_IllegalItems[i - 1].ToString();
				}
			}
		}
		*/
	}
}
