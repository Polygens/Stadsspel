using System;
using UnityEngine;
using UnityEngine.Networking;

public class Treasure : Square
{
	private static int robThreshold = 2000; // Above this amount can be stolen
	private int moneyGainPerDistrict = 1000;

    [SyncVar]
	private int mAmountOfMoney = 5000;

    public int AmountOfMoney
    {
        get
        {
            return mAmountOfMoney;
        }
        set
        {
            mAmountOfMoney = value;
        }
    }

	private new void Start()
	{
		mDistrictType = DistrictType.square;
		base.Start();
		tag = "Treasure";
        GameManager.s_Singleton.Teams[(int)mTeamID - 1].AddOrRemoveMoney(mAmountOfMoney); //Update total team money
    }

    public int GetRobAmount()
    {
        if (mAmountOfMoney > robThreshold)
        {
            return AmountOfMoney - robThreshold;
        }
        return 0;
    }

	public int RobChest()
	{
		int robbedMoney = mAmountOfMoney * (robThreshold / 100);
		mAmountOfMoney -= robbedMoney;
		return robbedMoney;
	}

	public void GainMoneyOverTime()
	{
        int moneyGain = moneyGainPerDistrict * CheckAmountOfCapturedDistricts();
        mAmountOfMoney += moneyGain;
        GameManager.s_Singleton.Teams[(int)mTeamID - 1].AddOrRemoveMoney(moneyGain); //Update total team money
        Debug.Log("Treasure" + (int)mTeamID + " has " + mAmountOfMoney);
	}

	private int CheckAmountOfCapturedDistricts()
	{
		return GameManager.s_Singleton.Teams[(int)mTeamID - 1].AmountOfDistricts;
	}

    public bool IsMoneyTransferValid(int amount)
    {
        return amount <= mAmountOfMoney;
    }
    
}
