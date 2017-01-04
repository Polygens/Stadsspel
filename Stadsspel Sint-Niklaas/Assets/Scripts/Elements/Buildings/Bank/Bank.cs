using UnityEngine;
using UnityEngine.UI;
using System;

public class Bank : Financial
{
    //BankAccountManager manager = new BankAccountManager();
    public Text amountField;
    public Text amountOwnMoney;
    public Text amountBankMoney;
    public Toggle selectAllToggle;

    private Player player;

    public Bank()
	{
        //throw new System.NotImplementedException();
        
    }

    public void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void Update()
    {
        amountOwnMoney.text = player.AmountOfMoney.ToString();
    }

    public override void GainMoneyOverTime()
    {
        // Interest
        for (byte i = 0; i < BankAccountManager.BankAccounts.Count; i++)
        {
            BankAccount account = BankAccountManager.BankAccounts[i];
            GetComponent<BankAccountManager>().CmdTransaction((TeamID)i, Mathf.RoundToInt(account.Balance * interestMultiplier));//Add 2% from current balance to the total balance
        }
    }

    public void Transaction(bool isDeposit)
    {
        TeamID teamID = player.Team;
        int amount = int.Parse(amountField.text);
        
        if(isDeposit) //Add money to bank, subtract from player
        {
            if (amount <= player.AmountOfMoney)
            {
                player.RemoveMoney(amount);
                GetComponent<BankAccountManager>().CmdTransaction(teamID, amount);
            }         
        }
        else //Subtract money from bank, add to player
        {
            if(amount <= BankAccountManager.BankAccounts[Convert.ToInt32(teamID)].Balance)
            {
                player.AddItems(amount);
                GetComponent<BankAccountManager>().CmdTransaction(teamID, -amount);
            }     
        }
        //Update UI
        amountBankMoney.text = BankAccountManager.BankAccounts[Convert.ToInt32(teamID)].Balance.ToString();
        amountOwnMoney.text = player.AmountOfMoney.ToString();
    }

    public void SelectAll()
    {
        if(selectAllToggle.isOn)
        {
            amountField.text = player.AmountOfMoney.ToString();
        }
    }
}