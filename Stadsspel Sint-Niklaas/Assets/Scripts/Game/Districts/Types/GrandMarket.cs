namespace Stadsspel.Districts
{
	public class GrandMarket : District
	{
		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
		private new void Awake()
		{
			m_DistrictType = DistrictType.GrandMarket;
			base.Awake();
		}
	}
}