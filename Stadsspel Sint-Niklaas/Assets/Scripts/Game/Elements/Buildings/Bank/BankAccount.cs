using Photon;
using UnityEngine;

namespace Stadsspel.Elements
{
	public class BankAccount : MonoBehaviour
	{
		[SerializeField]
		private int m_Balance;

		private ServerTeam m_Team;

		public int Balance {
			get { return m_Balance; }
		}

		public ServerTeam Team {
			get { return m_Team; }
			set { m_Team = value; }
		}
	}
}
