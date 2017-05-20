using UnityEngine;

namespace Stadsspel.Districts
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class CapturePoint : Square
	{
		private byte[] m_PlayersOnPoint;
		private float m_CapturingAmount = 0;
		private ServerTeam m_CapturingTeam = null;

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

			// Check which team has the most players
			for(int i = 0; i < m_PlayersOnPoint.Length; i++) {
				totalPlayers += m_PlayersOnPoint[i];
				if(m_PlayersOnPoint[i] > mostPlayers) {
					mostPlayers = m_PlayersOnPoint[i];
					mostPlayersTeam = CurrentGame.Instance.gameDetail.GetTeamByIndex(i);
				}
			}

			// If there are players calculate the difference between the next team with the most players.
			if(mostPlayers > 0) {
				int competingPlayers = totalPlayers - mostPlayers;
				int mostPlayersTeamDiff = mostPlayers - competingPlayers;

				// If the capturing team has a majority capturing begins
				if(mostPlayersTeamDiff > 0) {
					// Reverse capturing progress competing team
					if(!mostPlayersTeam.Equals(m_CapturingTeam)) {
						m_CapturingAmount -= Time.deltaTime * mostPlayersTeamDiff * m_CaptureMultiplier;
						if(m_CapturingAmount <= 0) {
							m_CapturingTeam = mostPlayersTeam;
						}
					}
					// Progress capturing of capturing team
					else if(!Team.Equals(mostPlayersTeam)) {
						m_CapturingAmount += Time.deltaTime * mostPlayersTeamDiff * m_CaptureMultiplier;
						m_CapturingTeam = mostPlayersTeam;
					}

					// If fully captured update team, team data and show notification.
					if(m_CapturingAmount >= 100) {
						if(Team != null) {
							GameManager.s_Singleton.Teams[CurrentGame.Instance.gameDetail.IndexOfTeam(Team)].AddOrRemoveDistrict(-1);
						}
						Team = m_CapturingTeam;
						transform.parent.GetComponent<CapturableDistrict>().Team = m_CapturingTeam;
						GameManager.s_Singleton.DistrictManager.DestroyCapturingNotification();
						InGameUIManager.s_Singleton.LogUI.AddToLog(LogUI.m_WasCaptured, new object[] { this.name, Team });
						m_CapturingAmount = 0;
						GameManager.s_Singleton.Teams[CurrentGame.Instance.gameDetail.IndexOfTeam(Team)].AddOrRemoveDistrict(1);
					}
				}

				// Update the capturing notification progress.
				if(GameManager.s_Singleton.DistrictManager.CurrentCapturePoint == this && (int)Mathf.Round(m_CapturingAmount) != 0) {
					GameManager.s_Singleton.DistrictManager.GenerateCapturingNotification();

					Color c1 = new Color();
					Color c2 = new Color();
					ColorUtility.TryParseHtmlString(Team.CustomColor, out c1);
					ColorUtility.TryParseHtmlString(m_CapturingTeam.CustomColor, out c2);
					GameManager.s_Singleton.DistrictManager.CapturingNotification.SetColor(c1,c2);

					GameManager.s_Singleton.DistrictManager.CapturingNotification.SetProgress(m_CapturingAmount / 100);
				}
			}

			// Destroy the capturing notification when no longer on capture point.
			if(GameManager.s_Singleton.DistrictManager.CurrentCapturePoint == null) {
				GameManager.s_Singleton.DistrictManager.DestroyCapturingNotification();
			}
		}

		/// <summary>
		/// Adds a player of the specified team to the player list of m_PlayersOnPoint.
		/// </summary>
		[PunRPC]
		private void AddPlayerOnPoint(TeamID team)
		{
			m_PlayersOnPoint[(int)team - 1]++;
#if(UNITY_EDITOR)
			for(int i = 0; i < m_PlayersOnPoint.Length; i++) {
				Debug.Log("TeamID " + i + ": " + m_PlayersOnPoint[i]);
			}
#endif
		}

		/// <summary>
		/// Removes a player of the specified team from the player list of m_PlayersOnPoint.
		/// </summary>
		[PunRPC]
		private void RemovePlayerOnPoint(TeamID team)
		{
			m_PlayersOnPoint[(int)team - 1]--;
#if(UNITY_EDITOR)
			for(int i = 0; i < m_PlayersOnPoint.Length; i++) {
				Debug.Log("TeamID " + i + ": " + m_PlayersOnPoint[i]);
			}
#endif
		}
	}
}