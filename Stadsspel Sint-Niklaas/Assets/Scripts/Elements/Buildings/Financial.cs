public class Financial : Building
{
	protected int mAmountOfMoney;

	public Financial()
	{
		throw new System.NotImplementedException();
	}

	public void GetMoney()
	{
		throw new System.NotImplementedException();
	}

	public void StoreMoney()
	{
		throw new System.NotImplementedException();
	}

    public virtual void GainMoneyOverTime() // Gets called by GameManager
    {
        // Perhaps !! -> Update Team display TeamChestMoneyAmount
    }
}