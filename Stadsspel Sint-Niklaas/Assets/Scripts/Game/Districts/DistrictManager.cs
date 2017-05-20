using Stadsspel.Elements;
using System.Collections.Generic;
using UnityEngine;

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
		private GameObject m_CurrentDistrict = null;
		private Notification m_CapturingNotification;
		private CapturePoint m_CurrentCapturePoint = null;

		public Notification CapturingNotification {
			get {
				return m_CapturingNotification;
			}
		}

		public CapturePoint CurrentCapturePoint {
			get {
				return m_CurrentCapturePoint;
			}
		}

		public GameObject CurrentDistrict {
			get { return m_CurrentDistrict; }
		}

		[SerializeField]
		private PolygonCollider2D[] m_DistrictColliders;

		private List<HeadDistrict> m_HeadDistricts = new List<HeadDistrict>();

		/// <summary>
		/// Starts the game for the DistrictManager for a given amount of teams.
		/// </summary>
		public void StartGame(int amountOfTeams)
		{
			m_DistrictColliders = new PolygonCollider2D[gameObject.transform.childCount];
			for(int i = 0; i < gameObject.transform.childCount; i++) {
				m_DistrictColliders[i] = gameObject.transform.GetChild(i).gameObject.GetComponent<PolygonCollider2D>();
			}

			// Loops trough all district groups and allocates the teams and types.
			for(int i = 1; i <= 6; i++) {
				if(amountOfTeams >= i) {
					HeadDistrict district = transform.GetChild(i).gameObject.GetComponent<HeadDistrict>();
					district.Team = CurrentGame.Instance.gameDetail.GetTeamByIndex(i);
					district.enabled = true;
					m_HeadDistricts.Add(district);
					Destroy(district.GetComponent<CapturableDistrict>());
					Treasure square = district.transform.GetChild(0).gameObject.GetComponent<Treasure>();
					square.Team = CurrentGame.Instance.gameDetail.GetTeamByIndex(i);
					square.enabled = true;
					Destroy(district.transform.GetChild(0).GetComponent<CapturePoint>());

				}
				else {
					CapturableDistrict district = transform.GetChild(i).gameObject.GetComponent<CapturableDistrict>();
					district.Team = null;
					district.enabled = true;
					Destroy(district.GetComponent<HeadDistrict>());
					CapturePoint square = district.transform.GetChild(0).gameObject.GetComponent<CapturePoint>();
					square.Team = null;
					square.enabled = true;
					Destroy(district.transform.GetChild(0).GetComponent<Treasure>());
				}
			}
		}

		/// <summary>
		/// Initialises the Player Transform.
		/// </summary>
		public void SetPlayerTransform(Transform player)
		{
			m_PlayerTrans = player;
		}

		/// <summary>
		/// Check on which district the player is positioned.
		/// </summary>
		public void CheckDisctrictState()
		{
			GameObject newDistrict = null;
			for(int i = 0; i < m_DistrictColliders.Length; i++) {
				if(m_DistrictColliders[i].OverlapPoint(m_PlayerTrans.position)) {
					if(m_DistrictColliders[i].transform.childCount > 0) {
						BoxCollider2D square = m_DistrictColliders[i].transform.GetChild(0).GetComponent<BoxCollider2D>();
						if(square.OverlapPoint(m_PlayerTrans.position)) {
							newDistrict = square.gameObject;
						}
						else {
							newDistrict = m_DistrictColliders[i].gameObject;
						}
					}
					else {
						newDistrict = m_DistrictColliders[i].gameObject;
					}
					break;
				}
			}
			if(newDistrict != m_CurrentDistrict) {
				if(newDistrict) {
					HandleDistrictChange(m_CurrentDistrict, newDistrict);
#if(UNITY_EDITOR)
					Debug.Log(newDistrict.name);
#endif
				}
				m_CurrentDistrict = newDistrict;

			}
		}

		/// <summary>
		/// Handles the transitioning between the districts.
		/// </summary>
		private void HandleDistrictChange(GameObject oldDistrict, GameObject newDistrict)
		{
			m_CurrentCapturePoint = null;
			if(oldDistrict) {
				CapturePoint capturePointOld = oldDistrict.GetComponent<CapturePoint>();
				if(capturePointOld) {
					capturePointOld.photonView.RPC("RemovePlayerOnPoint", PhotonTargets.All, m_PlayerTrans.GetComponent<Person>().Team);
				}
			}

			CapturePoint capturePointNew = newDistrict.GetComponent<CapturePoint>();
			if(capturePointNew) {
				m_CurrentCapturePoint = capturePointNew;
				capturePointNew.photonView.RPC("AddPlayerOnPoint", PhotonTargets.All, m_PlayerTrans.GetComponent<Person>().Team);
			}
		}


		/// <summary>
		/// Safely instantiates the capturing notification.
		/// </summary>
		public void GenerateCapturingNotification()
		{
			if(!m_CapturingNotification) {
				m_CapturingNotification = InGameUIManager.s_Singleton.LogUI.AddToLog(LogUI.m_Capturing, new object[] { }, true).GetComponent<Notification>();
			}
		}

		/// <summary>
		/// Safely destroys the capturing notification.
		/// </summary>
		public void DestroyCapturingNotification()
		{
			if(m_CapturingNotification) {
				Destroy(m_CapturingNotification.gameObject);
			}
		}


		/// <summary>
		/// Returns the HeadDistrict class of the passed TeamID.
		/// </summary>
		public HeadDistrict GetHeadDistrict(TeamID team)
		{
			return m_HeadDistricts[(int)team - 1];
		}

		/// <summary>
		/// Returns the Treasure of the passed TeamID.
		/// </summary>
		public Treasure GetHeadSquare(TeamID team)
		{
			return GetHeadDistrict(team).transform.GetChild(0).GetComponent<Treasure>();
		}
	}
}