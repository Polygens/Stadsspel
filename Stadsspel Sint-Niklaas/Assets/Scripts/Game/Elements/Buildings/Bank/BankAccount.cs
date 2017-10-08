using Photon;
using UnityEngine;

namespace Stadsspel.Elements
{
	public class BankAccount : MonoBehaviour
	{
		[SerializeField]
		private int m_Balance;

		private ServerTeam m_Team;

		private float m_InterestMultiplier = 0.02f;

		public int Balance {
			get { return m_Balance; }
		}

		public ServerTeam Team {
			get { return m_Team; }
			set { m_Team = value; }
		}
		
		/*
		public void Transaction(int amount, PhotonMessageInfo info)
		{

			if(amount != 0) {
				m_Balance += amount;
				if(GameManager.s_Singleton.Player.Person.Team == m_Team) {
					if(amount < 0) {
						InGameUIManager.s_Singleton.LogUI.AddToLog(LogUI.m_HasWithdrawnMoneyInBank, new object[] {
						info.sender.NickName,
						Mathf.Abs(amount)
						});
					}
					else {
						InGameUIManager.s_Singleton.LogUI.AddToLog(LogUI.m_HasDepositedMoneyInBank, new object[] {
						info.sender.NickName,
						amount
						});
					}
				}
			}

		}
		*/
		/// <summary>
		///  Performs a bank transaction of the passed amount on the account money.
		/// todo this is handled by server i guess need to confirm
		/// </summary>
		public bool PlayerTransaction(int amount)
		{
			/*
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
			*/
			return false;
		}
	}
}