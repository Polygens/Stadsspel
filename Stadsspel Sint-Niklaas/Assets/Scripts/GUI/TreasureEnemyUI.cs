using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TreasureEnemyUI : MonoBehaviour
{
    public Text amountOfOwnMoney;
    public Text amountOfChestMoney;
    public InputField label;

    private Treasure currentTreasure;
    
	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    private void OnEnable()
    {
        FindCurrentTreasure();
        UpdateUI();
    }

    private void UpdateUI()
    {
        amountOfOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
        amountOfChestMoney.text = currentTreasure.AmountOfMoney.ToString();
        label.text = currentTreasure.GetRobAmount().ToString();
    }

    private void FindCurrentTreasure()
    {
        currentTreasure = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Treasure>();
    }

    public void TransferMoney()
    {
        int amount = int.Parse(currentTreasure.GetRobAmount().ToString());
        GameManager.s_Singleton.Teams[(int)currentTreasure.TeamID - 1].AddOrRemoveMoney(-amount); //Remove robbed money from enemy team's total money
        GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].AddOrRemoveMoney(amount); //Add robbed money to your team's total money
        //currentTreasure.Transaction(amount);
        UpdateUI();
    }
}
