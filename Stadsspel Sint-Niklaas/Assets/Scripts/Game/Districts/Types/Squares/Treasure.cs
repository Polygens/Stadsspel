using UnityEngine;

namespace Stadsspel.Districts
{
	public class Treasure : Square
	{
		private const int m_RobThreshold = 200;
		// Above this amount can be stolen
		private const int m_MoneyGainPerDistrict = 50;

		private int m_AmountOfMoney = 2000;

		public int AmountOfMoney {
			get {
				return m_AmountOfMoney;
			}
			set {
				m_AmountOfMoney = value;
			}
		}

		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
		private new void Awake()
		{
			m_DistrictType = DistrictType.square;
			base.Awake();
			tag = "Treasure";
		}

		/// <summary>
		/// Initialises the class.
		/// </summary>
		private void Start()
		{
			//GameManager.s_Singleton.Teams[(int)m_Team - 1].AddOrRemoveMoney(m_AmountOfMoney);//Update total team money todo fix cuz i lazy
			GameManager.s_Singleton.AddTreasure(this);
		}

		/// <summary>
		/// Returns the allowed amount of money that can be robbed of this treasure.
		/// </summary>
		public int GetRobAmount()
		{
			if(m_AmountOfMoney > m_RobThreshold) {
				return AmountOfMoney - m_RobThreshold;
			}
			return 0;
		}

		/// <summary>
		/// [PunRPC] Reduces the money in the chest with a given amount.
		/// </summary>
		[PunRPC]
		public void ReduceChestMoney(int amount)
		{
			m_AmountOfMoney -= amount;
		}

		/// <summary>
		/// [PunRPC] Reduces the amount of money of the treasure caused by rob.
		/// </summary>
		[PunRPC]
		public int RobChest()
		{
			int robbedMoney = m_AmountOfMoney * (m_RobThreshold / 100);
			m_AmountOfMoney -= robbedMoney;
			return robbedMoney;
		}

		/// <summary>
		/// [PunRPC] Calculates and adds the tax money of the owned districts to the treasure.
		/// todo this is done by server
		/// </summary>
		[PunRPC]
		public void RetrieveTaxes()
		{
			/*
			int moneyGain = m_MoneyGainPerDistrict * (GameManager.s_Singleton.Teams[(int)m_Team - 1].AmountOfDistricts + 1);
			m_AmountOfMoney += moneyGain;
			GameManager.s_Singleton.Teams[(int)m_Team - 1].AddOrRemoveMoney(moneyGain);//Update total team money

#if(UNITY_EDITOR)
			Debug.Log("Treasure" + (int)m_Team + " has " + m_AmountOfMoney);
#endif

			if(m_Team == GameManager.s_Singleton.Player.Person.Team) // GameLog
			{
				InGameUIManager.s_Singleton.LogUI.AddToLog(LogUI.m_TaxesIncome, new object[] { moneyGain });
			}
			*/
		}

		/// <summary>
		/// Verifies if the transaction is valid.
		/// </summary>
		public bool IsMoneyTranferValid(int amount)
		{
			return amount <= m_AmountOfMoney;
		}


	}
}