using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TreasureUI : MonoBehaviour
{
    public Text amountOfOwnMoney;
    public Text amountOfChestMoney;
    public InputField input;

    private Treasure currentTreasure;

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    private void OnEnable()
    {
        input.text = "";
        FindCurrentTreasure();
        UpdateUI();
    }

    private void UpdateUI()
    {
        amountOfOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
        amountOfChestMoney.text = currentTreasure.AmountOfMoney.ToString();
    }

    private void FindCurrentTreasure()
    {
        currentTreasure = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Treasure>();
    }

    public void TransferMoney()
    {
        int amount;
        int.TryParse(input.text, out amount);
        Debug.Log("amount = " + amount);
        currentTreasure.CmdTransaction(amount);
        UpdateUI();
    }
}
