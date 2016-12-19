public class Chest : Financial
{
	private int mTeam;
	private int mDistrict;
    private static int robThreshold; // This percentage can be stolen
    private int moneyGainPerDistrict;
    private int startUpMoney = 5000;

	public Chest()
	{
        mAmountOfMoney = startUpMoney;
	}

    public void TakeMoney()
    {
        // Open money transfer UI
    }

    public int RobChest()
    {
        int robbedMoney = mAmountOfMoney * (robThreshold / 100);
        mAmountOfMoney -= robbedMoney;
        return robbedMoney;
    }

    public override void GainMoneyOverTime()
    {
        mAmountOfMoney += moneyGainPerDistrict /*  * amountOfDistricts  */;

        base.GainMoneyOverTime();
    }
}