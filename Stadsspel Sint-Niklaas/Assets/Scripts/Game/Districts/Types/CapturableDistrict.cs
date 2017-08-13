using UnityEngine;

namespace Stadsspel.Districts
{
	public class CapturableDistrict : District
	{
		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
		private new void Awake()
		{
			m_DistrictType = DistrictType.CapturableDistrict;
			Team = null;
			base.Awake();
		}

		/// <summary>
		/// Handles the change of team.
		/// </summary>
		public override void OnTeamChanged()
		{
			base.OnTeamChanged();
			Color newColor = gameObject.GetComponent<Renderer>().material.color;
			newColor.a = 0.4f;
			gameObject.GetComponent<Renderer>().material.color = newColor;
		}
	}
}