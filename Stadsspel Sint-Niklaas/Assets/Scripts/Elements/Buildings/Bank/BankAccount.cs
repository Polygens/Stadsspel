public class BankAccount
{
    public TeamID teamID;
    private int balance;

    public BankAccount(TeamID id)
    {
        teamID = id;
        balance = 0;
    }

    public int Balance
    {
        get { return balance; }
    }

    public void Transaction(int amount)
    {
        balance += amount;
    }

}
