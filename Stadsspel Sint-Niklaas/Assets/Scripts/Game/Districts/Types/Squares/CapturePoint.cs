using UnityEngine;

namespace Stadsspel.Districts
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class CapturePoint : Square
	{
		private byte[] m_PlayersOnPoint;
		private float m_CapturingAmount = 0;
		private ServerTeam m_CapturingTeam = null;
		private bool IsPlayerOnPoint { get; set; }

		private const float m_CaptureMultiplier = 10;

		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
		private new void Awake()
		{
			Team = null;
			base.Awake();
			Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
			rigidbody.isKinematic = true;
			//m_PlayersOnPoint = new byte[TeamData.GetMaxTeams(PhotonNetwork.room.MaxPlayers)]; todo replace
			m_PlayersOnPoint = new byte[TeamData.GetMaxTeams(12)];
		}

		/// <summary>
		/// Gets called every frame. Handles the capturing of the square.
		/// </summary>
		private void Update()
		{
			int totalPlayers = 0;
			int mostPlayers = 0;
			ServerTeam mostPlayersTeam = null;

			

			// Update the capturing notification progress.
			if (GameManager.s_Singleton.DistrictManager.CurrentCapturePoint == this)
			{
				if (CurrentGame.Instance.lastConqueringUpdate != null)
				{
					m_CapturingAmount = (float)CurrentGame.Instance.lastConqueringUpdate.progress;
				}

				GameManager.s_Singleton.DistrictManager.GenerateCapturingNotification();

				Color c1 = new Color();
				Color c2 = new Color();
				if (Team == null)
				{
					c1 = Color.gray;
				}
				else
				{
					ColorUtility.TryParseHtmlString(Team.customColor, out c1);
				}
				ColorUtility.TryParseHtmlString(CurrentGame.Instance.PlayerTeam.customColor, out c2);
				GameManager.s_Singleton.DistrictManager.CapturingNotification.SetColor(c1, c2);

				GameManager.s_Singleton.DistrictManager.CapturingNotification.SetProgress(m_CapturingAmount);
			}


			// Destroy the capturing notification when no longer on capture point.
			if (GameManager.s_Singleton.DistrictManager.CurrentCapturePoint == null)
			{
				GameManager.s_Singleton.DistrictManager.DestroyCapturingNotification();
			}
		}

		/// <summary>
		/// Adds a player of the specified team to the player list of m_PlayersOnPoint.
		/// </summary>
		public void AddPlayerOnPoint(string team)
		{
			CurrentGame.Instance.Ws.SendConquerStart(CurrentGame.Instance.currentDistrictID);
			IsPlayerOnPoint = true;
#if (UNITY_EDITOR)
			for (int i = 0; i < m_PlayersOnPoint.Length; i++)
			{
				Debug.Log("TeamID " + i + ": " + m_PlayersOnPoint[i]);
			}
#endif
		}


		/// <summary>
		/// Removes a player of the specified team from the player list of m_PlayersOnPoint.
		/// </summary>
		public void RemovePlayerOnPoint(string team)
		{
			CurrentGame.Instance.Ws.SendConquerEnd(CurrentGame.Instance.currentDistrictID);
			IsPlayerOnPoint = false;
#if (UNITY_EDITOR)
			for (int i = 0; i < m_PlayersOnPoint.Length; i++)
			{
				Debug.Log("TeamID " + i + ": " + m_PlayersOnPoint[i]);
			}
#endif
		}
	}
}