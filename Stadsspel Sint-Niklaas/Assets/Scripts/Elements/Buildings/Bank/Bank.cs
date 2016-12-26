using UnityEngine;

public class Bank : Financial
{
    BankAccountManager manager = new BankAccountManager();

	public Bank()
	{
		throw new System.NotImplementedException();
	}

    public override void GainMoneyOverTime()
    {
        // Interest
        for (byte i = 0; i < BankAccountManager.BankAccounts.Count; i++)
        {
            BankAccount account = BankAccountManager.BankAccounts[i];
            manager.CmdTransaction(i, Mathf.RoundToInt(account.Balance * interestMultiplier));//Add 2% from current balance to the total balance
        }
    }

    public void Transaction(byte teamID, int amount) // Amount can be negative
    {
        manager.CmdTransaction(teamID, amount);
    }
}