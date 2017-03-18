using UnityEngine;

namespace Stadsspel.Districts
{
	internal class Outside : District
	{
		private new void Awake()
		{
			m_DistrictType = DistrictType.Outside;
			base.Awake();
		}
	}
}