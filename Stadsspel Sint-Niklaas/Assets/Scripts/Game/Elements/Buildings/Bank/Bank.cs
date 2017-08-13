namespace Stadsspel.Elements
{
	public class Bank : Building
	{
		/// <summary>
		/// Initialises the class.
		/// </summary>
		private new void Start()
		{
			m_BuildingType = BuildingType.Bank;
			base.Start();
		}

		public string BankId { get; set; }
	}
}
