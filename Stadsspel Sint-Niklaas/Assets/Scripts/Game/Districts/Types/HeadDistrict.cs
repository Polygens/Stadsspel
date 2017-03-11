using UnityEngine;

namespace Stadsspel.Districts
{
	public class HeadDistrict : District
	{
		private new void Start()
		{
			m_DistrictType = DistrictType.HeadDistrict;
			base.Start();
		}

		protected override void OnTeamChanged()
		{
			gameObject.GetComponent<Renderer>().material.color = TeamData.GetColor(m_TeamID);
			gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = TeamData.GetColor(m_TeamID);
		}
	}
}