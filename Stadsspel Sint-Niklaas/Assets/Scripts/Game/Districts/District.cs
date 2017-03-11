using UnityEngine;
using Photon;

namespace Stadsspel.Districts
{
	public enum DistrictType
	{
		NotSet,
		Neutral,
		GrandMarket,
		HeadDistrict,
		CapturableDistrict,
		square,
		Outside,
	}

	public class District : PunBehaviour
	{
		[SerializeField]
		//[SyncVar]
		protected TeamID m_TeamID = 0;

		[SerializeField]
		protected DistrictType m_DistrictType = 0;

		public TeamID TeamID {
			get {
				return m_TeamID;
			}

			set {
				m_TeamID = value;
				OnTeamChanged();
			}
		}

		protected virtual void OnTeamChanged()
		{
		}

		protected void Start()
		{
			gameObject.GetComponent<Renderer>().material.color = TeamData.GetColor(m_DistrictType);
		}
	}
}