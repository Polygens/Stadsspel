using System;
using UnityEngine.Networking;

public class Treasure : Square
{
	private static int robThreshold = 20; // This percentage can be stolen
	private int moneyGainPerDistrict = 1000;
	private int mAmountOfMoney = 5000;

	private new void Start()
	{
		mDistrictType = DistrictType.square;
		base.Start();
		tag = "Treasure";
	}

	public int RobChest()
	{
		int robbedMoney = mAmountOfMoney * (robThreshold / 100);
		mAmountOfMoney -= robbedMoney;
		return robbedMoney;
	}

	public void GainMoneyOverTime()
	{
		mAmountOfMoney += moneyGainPerDistrict * CheckAmountOfCapturedDistricts();
	}

	private int CheckAmountOfCapturedDistricts()
	{
		return GameManager.s_Singleton.Teams[(int)mTeamID].AmountOfDistricts;
	}

	public bool Transaction(int amount)
	{
		return false; // TO DO
	}
}
