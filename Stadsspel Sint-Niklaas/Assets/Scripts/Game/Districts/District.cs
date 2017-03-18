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
		protected TeamID m_Team = 0;

		[SerializeField]
		protected DistrictType m_DistrictType = 0;

		public TeamID Team {
			get {
				return m_Team;
			}

			set {
				m_Team = value;
				OnTeamChanged();
			}
		}

		protected virtual void OnTeamChanged()
		{
			Color newColor = TeamData.GetColor(m_Team);
			gameObject.GetComponent<Renderer>().material.color = newColor;
		}

		protected void Awake()
		{
			gameObject.GetComponent<Renderer>().material.color = TeamData.GetColor(m_DistrictType);
		}
	}
}