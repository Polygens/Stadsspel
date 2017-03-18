using UnityEngine;

namespace Stadsspel.Districts
{
	public class Neutral : District
	{
		private new void Awake()
		{
			m_DistrictType = DistrictType.Neutral;
			base.Awake();
		}
	}
}