using UnityEngine;

namespace Stadsspel.Districts
{
	public class HeadDistrict : District
	{
		private new void Awake()
		{
			m_DistrictType = DistrictType.HeadDistrict;
			base.Awake();
		}

		protected override void OnTeamChanged()
		{
			base.OnTeamChanged();
			Color newColor = gameObject.GetComponent<Renderer>().material.color;
			newColor.a = 0.2f;
			gameObject.GetComponent<Renderer>().material.color = newColor;
		}
	}
}