using UnityEngine;

namespace Stadsspel.Districts
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class CapturePoint : Square
	{
		private byte[] m_PlayersOnPoint;
		private float m_CapturingAmount = 0;
		private TeamID m_CapturingTeam = TeamID.NoTeam;

		private const float m_CaptureMultiplier = 10;

		private new void Awake()
		{
			Team = TeamID.NoTeam;
			base.Awake();
			Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
			rigidbody.isKinematic = true;
			m_PlayersOnPoint = new byte[TeamData.GetMaxTeams(PhotonNetwork.room.MaxPlayers)];
		}

		private void Update()
		{
			int totalPlayers = 0;
			int mostPlayers = 0;
			TeamID mostPlayersTeam = TeamID.NotSet;

			for(int i = 0; i < m_PlayersOnPoint.Length; i++) {
				totalPlayers += m_PlayersOnPoint[i];
				if(m_PlayersOnPoint[i] > mostPlayers) {
					mostPlayers = m_PlayersOnPoint[i];
					mostPlayersTeam = (TeamID)i + 1;
				}
			}

			if(mostPlayers > 0) {
				int competingPlayers = totalPlayers - mostPlayers;
				int mostPlayersTeamDiff = mostPlayers - competingPlayers;
				if(mostPlayersTeamDiff > 0) {
					// Reverse capturing progress competing team
					if(mostPlayersTeam != m_CapturingTeam) {
						m_CapturingAmount -= Time.deltaTime * mostPlayersTeamDiff * m_CaptureMultiplier;
						if(m_CapturingAmount <= 0) {
							m_CapturingTeam = mostPlayersTeam;
						}
					} else if(Team != GameManager.s_Singleton.Player.Person.Team) { // Progress capturing of capturing team
						m_CapturingAmount += Time.deltaTime * mostPlayersTeamDiff * m_CaptureMultiplier;
						m_CapturingTeam = mostPlayersTeam;
					}

					if(m_CapturingAmount >= 100) {
						Team = mostPlayersTeam;
						transform.parent.GetComponent<CapturableDistrict>().Team = mostPlayersTeam;
						GameManager.s_Singleton.DistrictManager.DestroyCapturingNotification();
						InGameUIManager.s_Singleton.LogUI.AddToLog(LogUI.m_WasCaptured, new object[]{ this.name, Team });
						m_CapturingAmount = 0;
					}
				}
					
				if(GameManager.s_Singleton.DistrictManager.CurrentCapturePoint == this && (int)Mathf.Round(m_CapturingAmount) != 0) {
					GameManager.s_Singleton.DistrictManager.GenerateCapturingNotification();
					GameManager.s_Singleton.DistrictManager.CapturingNotification.SetColor(TeamData.GetColor(Team), TeamData.GetColor(m_CapturingTeam));
					GameManager.s_Singleton.DistrictManager.CapturingNotification.SetProgress(m_CapturingAmount / 100);
				} 
			}
			if(GameManager.s_Singleton.DistrictManager.CurrentCapturePoint == null) {
				GameManager.s_Singleton.DistrictManager.DestroyCapturingNotification();
			}
		}

		[PunRPC]
		private void AddPlayerOnPoint(TeamID team)
		{
			m_PlayersOnPoint[(int)team - 1]++;
			#if (UNITY_EDITOR)
			for(int i = 0; i < m_PlayersOnPoint.Length; i++) {
				Debug.Log("TeamID " + i + ": " + m_PlayersOnPoint[i]);
			}
			#endif
		}

		[PunRPC]
		private void RemovePlayerOnPoint(TeamID team)
		{
			m_PlayersOnPoint[(int)team - 1]--;
			#if (UNITY_EDITOR)
			for(int i = 0; i < m_PlayersOnPoint.Length; i++) {
				Debug.Log("TeamID " + i + ": " + m_PlayersOnPoint[i]);
			}
			#endif
		}
	}
}