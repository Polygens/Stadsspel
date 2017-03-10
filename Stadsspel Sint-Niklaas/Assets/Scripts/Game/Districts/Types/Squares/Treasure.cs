using UnityEngine;

namespace Stadsspel.Districts
{
	public class Treasure : Square
	{
		private static int robThreshold = 2000;
		// Above this amount can be stolen
		private int moneyGainPerDistrict = 1000;

		//[SyncVar]
		private int mAmountOfMoney = 5000;

		public int AmountOfMoney {
			get {
				return mAmountOfMoney;
			}
		}

		private new void Start()
		{
			mDistrictType = DistrictType.square;
			base.Start();
			tag = "Treasure";
		}

		public int GetRobAmount()
		{
			if(mAmountOfMoney > robThreshold) {
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
			mAmountOfMoney += moneyGainPerDistrict * CheckAmountOfCapturedDistricts();
			GameManager.s_Singleton.Teams[(int)mTeamID - 1].AddOrRemoveMoney(moneyGainPerDistrict * CheckAmountOfCapturedDistricts()); //Update total team money
			Debug.Log("Treasure" + (int)mTeamID + " has " + mAmountOfMoney);
		}

		private int CheckAmountOfCapturedDistricts()
		{
			return GameManager.s_Singleton.Teams[(int)mTeamID - 1].AmountOfDistricts;
		}

		//[Command]
		public void CmdTransaction(int amount)
		{
			if(amount <= mAmountOfMoney) {
				mAmountOfMoney -= amount;
				GameManager.s_Singleton.Player.Person.MoneyTransaction(amount);
				Debug.Log("ChestMoney: " + mAmountOfMoney);
			}
		}
	}
}