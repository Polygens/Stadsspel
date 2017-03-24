namespace Stadsspel.Districts
{
	internal class Outside : District
	{
		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
		private new void Awake()
		{
			m_DistrictType = DistrictType.Outside;
			base.Awake();
		}
	}
}