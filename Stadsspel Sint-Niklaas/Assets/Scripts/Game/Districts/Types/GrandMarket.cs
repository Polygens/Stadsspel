using UnityEngine;

namespace Stadsspel.Districts
{
	public class GrandMarket : District
	{
		private new void Start()
		{
			m_DistrictType = DistrictType.GrandMarket;
			base.Start();
		}
	}
}