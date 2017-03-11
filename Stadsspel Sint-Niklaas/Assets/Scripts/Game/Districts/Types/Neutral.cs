using UnityEngine;

namespace Stadsspel.Districts
{
	public class Neutral : District
	{
		private new void Start()
		{
			m_DistrictType = DistrictType.Neutral;
			base.Start();
		}
	}
}