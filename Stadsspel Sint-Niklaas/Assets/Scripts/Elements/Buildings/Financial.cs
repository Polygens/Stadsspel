using UnityEngine.Networking;

public class Financial : Building
{
    [SyncVar]
	protected int mAmountOfMoney;

	public Financial()
	{
		throw new System.NotImplementedException();
	}

	public void GetMoney()
	{
		// Open Money Transfer UI
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