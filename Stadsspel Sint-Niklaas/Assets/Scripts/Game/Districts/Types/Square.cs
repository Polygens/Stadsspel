using UnityEngine;


namespace Stadsspel.Districts
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class Square : District
	{
		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
		protected new void Awake()
		{
			m_DistrictType = DistrictType.square;
			base.Awake();
			tag = "Square";
		}

	}
}