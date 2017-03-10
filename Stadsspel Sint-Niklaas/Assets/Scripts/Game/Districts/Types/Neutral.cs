using UnityEngine;

namespace Stadsspel.Districts
{
	public class Neutral : District
	{
		private new void Start()
		{
			mDistrictType = DistrictType.Neutral;
			base.Start();
		}
	}
}