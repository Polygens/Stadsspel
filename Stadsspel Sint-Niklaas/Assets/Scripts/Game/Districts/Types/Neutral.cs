namespace Stadsspel.Districts
{
	public class Neutral : District
	{
		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
		private new void Awake()
		{
			m_DistrictType = DistrictType.Neutral;
			base.Awake();
		}
	}
}