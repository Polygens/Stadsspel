using UnityEngine;

namespace Stadsspel.Districts
{
	internal class Outside : District
	{
		private new void Start()
		{
			mDistrictType = DistrictType.Outside;
			base.Start();
		}
	}
}