using UnityEngine;
using System;

public class Bank : Financial
{
	//BankAccountManager manager = new BankAccountManager();

	public override void GainMoneyOverTime()
	{
		// Interest
		for (byte i = 0; i < BankAccountManager.BankAccounts.Count; i++) {
			BankAccount account = BankAccountManager.BankAccounts[i];
			GetComponent<BankAccountManager>().CmdTransaction((TeamID)i, Mathf.RoundToInt(account.Balance * interestMultiplier));//Add 2% from current balance to the total balance
		}
	}

	public void Transaction(bool isDeposit, int amount)
	{
		TeamID teamID = GameManager.s_Singleton.Player.Team;

		if (isDeposit) //Add money to bank, subtract from player
		{
			if (amount <= GameManager.s_Singleton.Player.AmountOfMoney) {
				GameManager.s_Singleton.Player.RemoveMoney(amount);
				GetComponent<BankAccountManager>().CmdTransaction(teamID, amount);
			}
		}
		else //Subtract money from bank, add to player
		{
			if (amount <= BankAccountManager.BankAccounts[Convert.ToInt32(teamID)].Balance) {
				GameManager.s_Singleton.Player.AddItems(amount);
				GetComponent<BankAccountManager>().CmdTransaction(teamID, -amount);
			}
		}
	}
}
