using UnityEngine.Networking;

public class Chest : Financial
{
    [SyncVar]
	private int mTeam;
    [SyncVar]
	private int mDistrict;

    private static int robThreshold = 20; // This percentage can be stolen
    private int moneyGainPerDistrict = 1000;
    private int startUpMoney = 5000;

    public int Team
    {
        get
        {
            return mTeam;
        }
    }
    public int District
    {
        get
        {
            return mDistrict;
        }
    }

	public Chest(int team, int district)
	{
        mTeam = team;
        mDistrict = district;
        mAmountOfMoney = startUpMoney;
	}

    public int RobChest()
    {
        int robbedMoney = mAmountOfMoney * (robThreshold / 100);
        mAmountOfMoney -= robbedMoney;
        return robbedMoney;
    }

    public override void GainMoneyOverTime()
    {
        CheckAmountOfCapturedDistricts();
        mAmountOfMoney += moneyGainPerDistrict /*  * amountOfDistricts  */;

        base.GainMoneyOverTime();
    }

    private void CheckAmountOfCapturedDistricts()
    {

    }
}