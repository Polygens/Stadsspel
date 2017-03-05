using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Square : District
{
	protected new void Start()
	{
		mDistrictType = DistrictType.square;
		base.Start();
		tag = "Square";
	}
}
