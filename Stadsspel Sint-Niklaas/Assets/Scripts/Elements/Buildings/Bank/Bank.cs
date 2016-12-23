using UnityEngine;

public class Bank : Financial
{
	public Bank()
	{
		throw new System.NotImplementedException();
        
	}

    public override void GainMoneyOverTime()
    {
        // Interest
        for (int i = 0; i < BankAccountManager.BankAccounts.Count; i++)
        {
            BankAccount account = BankAccountManager.BankAccounts[i];
            account.Transaction(Mathf.RoundToInt(account.Balance * interestMultiplier));//Add 2% from current balance to the total balance
        }
        //base.GainMoneyOverTime();
    }
}