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
			set {
				m_AmountOfMoney = value;
			}
		}

		private new void Awake()
		{
			m_DistrictType = DistrictType.square;
			base.Awake();
			tag = "Treasure";
		}

		private void Start()
		{
			GameManager.s_Singleton.Teams[(int)m_Team - 1].AddOrRemoveMoney(m_AmountOfMoney);//Update total team money
			GameManager.s_Singleton.AddTreasure(this);
		}

		public int GetRobAmount()
		{
			if(m_AmountOfMoney > m_RobThreshold) {
				return AmountOfMoney - m_RobThreshold;
			}
			return 0;
		}

		[PunRPC]
		public void EmptyChest(int amount)
		{
			m_AmountOfMoney -= amount;
		}

		[PunRPC]
		public int RobChest()
		{
			int robbedMoney = m_AmountOfMoney * (m_RobThreshold / 100);
			m_AmountOfMoney -= robbedMoney;
			return robbedMoney;
		}

		[PunRPC]
		public void GainMoneyOverTime()
		{
			m_AmountOfMoney += moneyGain;
			GameManager.s_Singleton.Teams[(int)m_Team - 1].AddOrRemoveMoney(moneyGain);//Update total team money
			#if (UNITY_EDITOR)
			Debug.Log("Treasure" + (int)m_Team + " has " + m_AmountOfMoney);
			#endif
		}

		private int CheckAmountOfCapturedDistricts()
		{
			return GameManager.s_Singleton.Teams[(int)m_Team - 1].AmountOfDistricts;
				int moneyGain = m_MoneyGainPerDistrict * GameManager.s_Singleton.Teams[(int)m_Team - 1].AmountOfDistricts;
		}

		public bool IsMoneyTranferValid(int amount)
		{
			return amount <= m_AmountOfMoney;
		}


	}
}