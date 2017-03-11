using UnityEngine;


namespace Stadsspel.Districts
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class Square : District
	{
		protected new void Start()
		{
			m_DistrictType = DistrictType.square;
			base.Start();
			tag = "Square";
		}
	}
}