using UnityEngine;

namespace Stadsspel.Districts
{
	public class HeadDistrict : District
	{
		private new void Start()
		{
			m_DistrictType = DistrictType.HeadDistrict;
			base.Start();
			OnTeamChanged();
		}

		protected override void OnTeamChanged()
		{
			Color newColor = TeamData.GetColor(m_TeamID);
			newColor.a = 0.2f;
			gameObject.GetComponent<Renderer>().material.color = newColor;
		}
	}
}