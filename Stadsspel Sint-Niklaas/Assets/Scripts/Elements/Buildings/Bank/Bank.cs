using System.Collections.Generic;

public class Bank : Financial
{
    private List<BankAccount> bankAccounts = new List<BankAccount>();

	public Bank()
	{
		throw new System.NotImplementedException();
        
	}

    public override void GainMoneyOverTime()
    {
        // Interest

        base.GainMoneyOverTime();
    }

    public BankAccount CreateBankAccount(TeamID id)
    {
        return new BankAccount(id);
    }
}