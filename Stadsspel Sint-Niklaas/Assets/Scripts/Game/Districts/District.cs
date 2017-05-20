using Photon;
using UnityEngine;

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
		protected ServerTeam m_Team;

		[SerializeField]
		protected DistrictType m_DistrictType = 0;

		public ServerTeam Team {
			get {
				return m_Team;
			}

			set {
				m_Team = value;
				OnTeamChanged();
			}
		}

		public DistrictType DistrictType {
			get { return m_DistrictType; }
		}

		/// <summary>
		/// Handles the change of team.
		/// </summary>
		protected virtual void OnTeamChanged()
		{
			if (m_Team == null)
			{
				Debug.Log("Team Null");
				return;
			}
			else if (m_Team.CustomColor == null)
			{
				m_Team.CustomColor = "#FF0000";
			}
			Color newColor = new Color();
			ColorUtility.TryParseHtmlString(m_Team.CustomColor, out newColor);
			gameObject.GetComponent<Renderer>().material.color = newColor;
		}

		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
		protected void Awake()
		{
			gameObject.GetComponent<Renderer>().material.color = TeamData.GetColor(m_DistrictType);
		}
	}
}