public class BankAccount
{
    public byte teamID;
    private int balance;

    public BankAccount(byte id)
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
