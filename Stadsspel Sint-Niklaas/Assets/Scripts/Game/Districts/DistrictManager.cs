using UnityEngine;
using System.Collections.Generic;

namespace Stadsspel.Districts
{
	public enum DistrictStates
	{
		NoTerritory,
		EnemyTerritory,
		FriendlyTerritory,
		UncapturedTerritory,
		CentralMarket
	}

	public class DistrictManager : MonoBehaviour
	{
		private Transform m_PlayerTrans;

		[SerializeField]
		private PolygonCollider2D[] m_DistrictColliders;

		private List<HeadDistrict> m_HeadDistricts = new List<HeadDistrict>();

		public void StartGame(int amountOfTeams)
		{
			m_DistrictColliders = new PolygonCollider2D[gameObject.transform.childCount];
			for(int i = 0; i < gameObject.transform.childCount; i++) {
				m_DistrictColliders[i] = gameObject.transform.GetChild(i).gameObject.GetComponent<PolygonCollider2D>();
			}

			for(int i = 1; i <= 6; i++) {
				if(amountOfTeams >= i) {
					HeadDistrict district = transform.GetChild(i).gameObject.GetComponent<HeadDistrict>();
					district.TeamID = (TeamID)(i);
					district.enabled = true;
					m_HeadDistricts.Add(district);
					Destroy(district.GetComponent<CapturableDistrict>());
					Treasure square = district.transform.GetChild(0).gameObject.GetComponent<Treasure>();
					square.TeamID = (TeamID)(i);
					square.enabled = true;
					Destroy(district.transform.GetChild(0).GetComponent<CapturePoint>());
                
				} else {
					CapturableDistrict district = transform.GetChild(i).gameObject.GetComponent<CapturableDistrict>();
					district.TeamID = TeamID.NoTeam;
					district.enabled = true;
					Destroy(district.GetComponent<HeadDistrict>());
					CapturePoint square = district.transform.GetChild(0).gameObject.GetComponent<CapturePoint>();
					square.TeamID = TeamID.NoTeam;
					square.enabled = true;
					Destroy(district.transform.GetChild(0).GetComponent<Treasure>());
				}
			}
		}

		public void SetPlayerTransform(Transform player)
		{
			m_PlayerTrans = player;
		}

		public DistrictStates CheckDisctrictState()
		{
			for(int i = 0; i < m_DistrictColliders.Length; i++) {
				if(m_DistrictColliders[i].OverlapPoint(m_PlayerTrans.position)) {
					if(m_DistrictColliders[i].transform.childCount > 0) {
						BoxCollider2D square = m_DistrictColliders[i].transform.GetChild(0).GetComponent<BoxCollider2D>();
						if(square.OverlapPoint(m_PlayerTrans.position)) {
							#if (UNITY_EDITOR)
							Debug.Log(square.gameObject.name);
							#endif
						} else {
							#if (UNITY_EDITOR)
							Debug.Log(m_DistrictColliders[i].gameObject.name);
							#endif
						}
					} else {
						#if (UNITY_EDITOR)
						Debug.Log(m_DistrictColliders[i].gameObject.name);
						#endif
					}

					break;
				}
			}
			return DistrictStates.NoTerritory;
		}

		public HeadDistrict GetHeadDistrict(TeamID team)
		{
			return m_HeadDistricts[(int)team - 1];
		}

		public Treasure GetHeadSquare(TeamID team)
		{
			return GetHeadDistrict(team).transform.GetChild(0).GetComponent<Treasure>();
		}
	}
}