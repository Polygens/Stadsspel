using UnityEngine;


namespace Stadsspel.Districts
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class Square : District
	{
		protected new void Awake()
		{
			m_DistrictType = DistrictType.square;
			base.Awake();
			tag = "Square";
		}

	}
}