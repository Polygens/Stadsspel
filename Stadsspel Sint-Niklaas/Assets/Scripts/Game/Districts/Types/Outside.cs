using UnityEngine;

namespace Stadsspel.Districts
{
	internal class Outside : District
	{
		private new void Start()
		{
			m_DistrictType = DistrictType.Outside;
			base.Start();
		}
	}
}