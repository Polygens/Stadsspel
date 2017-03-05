using UnityEngine;

public class Neutral : District
{
	private new void Start()
	{
		mDistrictType = DistrictType.Neutral;
		base.Start();
	}
}
