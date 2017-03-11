using UnityEngine;

namespace Stadsspel.Districts
{
	public class CapturableDistrict : District
	{
		private new void Start()
		{
			m_DistrictType = DistrictType.CapturableDistrict;
			base.Start();
		}

		protected override void OnTeamChanged()
		{
			gameObject.GetComponent<Renderer>().material.color = TeamData.GetColor(m_TeamID);
			gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = TeamData.GetColor(m_TeamID);
		}
	}
}