using Photon;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

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

	public class District : MonoBehaviour
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
		public virtual void OnTeamChanged()
		{
			if (m_Team == null)
			{
				Debug.Log("Team Null");
				return;
			}
			else if (m_Team.customColor == null)
			{
				m_Team.customColor = "#FF0000";
			}
			Color newColor = new Color();
			ColorUtility.TryParseHtmlString(m_Team.customColor, out newColor);
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