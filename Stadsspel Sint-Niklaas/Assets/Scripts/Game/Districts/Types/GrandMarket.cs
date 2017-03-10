using UnityEngine;

namespace Stadsspel.Districts
{
	public class GrandMarket : District
	{
		private new void Start()
		{
			mDistrictType = DistrictType.GrandMarket;
			base.Start();
		}
	}
}