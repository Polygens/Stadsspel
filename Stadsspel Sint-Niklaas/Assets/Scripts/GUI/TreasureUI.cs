using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TreasureUI : MonoBehaviour
{
    public Text amountOfOwnMoney;
    public Text amountOfChestMoney;

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    private void OnEnable()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        amountOfOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
        
    }
}
