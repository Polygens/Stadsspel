using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsUI : MonoBehaviour
{
	private List<int> m_LegalItems = new List<int>();
	private List<int> m_IllegalItems = new List<int>();

	private void OnEnable()
	{
		m_LegalItems = GameManager.s_Singleton.Player.Person.LookUpLegalItems;

		m_IllegalItems = GameManager.s_Singleton.Player.Person.LookUpIllegalItems;

		RectTransform Grid = (RectTransform)transform.FindChild("MainPanel").transform.FindChild("Grid");
		for(int i = 1; i < Grid.childCount; i++) {
			for(int j = 0; j < 2; j++) {
				if(j == 0) {
					Grid.GetChild(i).GetChild(j).transform.FindChild("Aantal").GetComponent<Text>().text = m_LegalItems[i - 1].ToString();
					Debug.Log("legal item: " + m_LegalItems[i - 1]);
				}
				else {
					Grid.GetChild(i).GetChild(j).transform.FindChild("Aantal").GetComponent<Text>().text = m_IllegalItems[i - 1].ToString();
				}
			}
		}
	}
}
