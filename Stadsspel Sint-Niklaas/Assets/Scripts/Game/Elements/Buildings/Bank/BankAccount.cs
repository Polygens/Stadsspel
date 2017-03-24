using Photon;
using UnityEngine;

namespace Stadsspel.Elements
{
	public class BankAccount : PunBehaviour
	{
		[SerializeField]
		private int m_Balance;

		private TeamID m_Team;

		private float m_InterestMultiplier = 0.02f;

		public int Balance {
			get { return m_Balance; }
		}

		public TeamID Team {
			get { return m_Team; }
			set { m_Team = value; }
		}

		/// <summary>
		/// [PunRPC] Performs a bank transaction of the passed amount on the account money.
		/// </summary>
		[PunRPC]
		public void Transaction(int amount, PhotonMessageInfo info)
		{
			m_Balance += amount;
			if(GameManager.s_Singleton.Player.Person.Team == m_Team) {
				InGameUIManager.s_Singleton.LogUI.AddToLog(LogUI.m_HasDepositedMoneyInBank, new object[] {
						info.sender.NickName,
						amount
						});
			}
		}

		/// <summary>
		/// [PunRPC] Performs a bank transaction of the passed amount on the account money.
		/// </summary>
		[PunRPC]
		public bool PlayerTransaction(int amount)
		{

			if(amount > 0) { //Add money to bank, subtract from player
				if(amount <= GameManager.s_Singleton.Player.Person.AmountOfMoney) {
					GameManager.s_Singleton.Player.Person.photonView.RPC("TransactionMoney", PhotonTargets.AllViaServer, -amount);
					//m_Balance += amount;
					return true;
				}
			}
			else { //Subtract money from bank, add to player
				if(-amount <= m_Balance) {
					GameManager.s_Singleton.Player.Person.photonView.RPC("TransactionMoney", PhotonTargets.AllViaServer, -amount);
					//m_Balance += amount;
					return true;
				}
			}
			return false;
		}
	}
}