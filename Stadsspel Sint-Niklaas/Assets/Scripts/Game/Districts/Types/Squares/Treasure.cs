using UnityEngine;

namespace Stadsspel.Districts
{
	public class Treasure : Square
	{
		private const int m_RobThreshold = 2000;
		// Above this amount can be stolen
		private const int m_MoneyGainPerDistrict = 1000;

		//[SyncVar]
		private int m_AmountOfMoney = 5000;

		public int AmountOfMoney {
			get {
				return m_AmountOfMoney;
			}
		}

		private new void Start()
		{
			m_DistrictType = DistrictType.square;
			base.Start();
			tag = "Treasure";
		}

		public int GetRobAmount()
		{
			if(m_AmountOfMoney > m_RobThreshold) {
				return AmountOfMoney - m_RobThreshold;
			}
			return 0;
		}

		public int RobChest()
		{
			int robbedMoney = m_AmountOfMoney * (m_RobThreshold / 100);
			m_AmountOfMoney -= robbedMoney;
			return robbedMoney;
		}

		public void GainMoneyOverTime()
		{
			m_AmountOfMoney += m_MoneyGainPerDistrict * CheckAmountOfCapturedDistricts();
			GameManager.s_Singleton.Teams[(int)m_TeamID - 1].AddOrRemoveMoney(m_MoneyGainPerDistrict * CheckAmountOfCapturedDistricts()); //Update total team money
			Debug.Log("Treasure" + (int)m_TeamID + " has " + m_AmountOfMoney);
		}

		private int CheckAmountOfCapturedDistricts()
		{
			return GameManager.s_Singleton.Teams[(int)m_TeamID - 1].AmountOfDistricts;
		}

		//[Command]
		public void CmdTransaction(int amount)
		{
			if(amount <= m_AmountOfMoney) {
				m_AmountOfMoney -= amount;
				GameManager.s_Singleton.Player.Person.MoneyTransaction(amount);
				Debug.Log("ChestMoney: " + m_AmountOfMoney);
			}
		}
	}
}