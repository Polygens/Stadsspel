using UnityEngine;
using Photon;

namespace Stadsspel.Elements
{
	public class BankAccount : PunBehaviour
	{
		[SerializeField]
		//[SyncVar]
		private int m_Balance;

		private float m_InterestMultiplier = 0.02f;

		public int Balance {
			get { return m_Balance; }
		}

		private void Update()
		{
			//mBalance += Mathf.RoundToInt(mBalance * interestMultiplier);//Add 2% from current balance to the total balance
		}

    [PunRPC]
		public void Transaction(int amount)
		{
			m_Balance += amount;
		}

		public bool PlayerTransaction(int amount)
		{
			if(amount > 0) { //Add money to bank, subtract from player
				if(amount <= GameManager.s_Singleton.Player.Person.AmountOfMoney) {
          GameManager.s_Singleton.Player.Person.BankTransaction(-amount);
					//m_Balance += amount;
					return true;
				}
			} else { //Subtract money from bank, add to player
				if(-amount <= m_Balance) {
					GameManager.s_Singleton.Player.Person.BankTransaction(-amount);
					//m_Balance += amount;
					return true;
				}
			}
			return false;
		}
	}
}