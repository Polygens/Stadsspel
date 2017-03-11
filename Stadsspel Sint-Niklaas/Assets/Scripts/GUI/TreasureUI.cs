using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TreasureUI : MonoBehaviour
{
    public Text amountOfOwnMoney;
    public Text amountOfChestMoney;
    public InputField input;
    private float timer;

    private Treasure currentTreasure;

	public Treasure CurrentTreasure
    {
        get
        {
            return currentTreasure;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1) // Refreshing for better feedback, can be adjusted if necessary
        {
            UpdateUI();
            timer = 0;
        }
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
        if (currentTreasure.IsMoneyTransferValid(amount))
        {
            Debug.Log("Valid Transaction");
            GameManager.s_Singleton.Player.Person.CmdTreasureTransaction(amount);
        }
        
        UpdateUI();
    }
}
