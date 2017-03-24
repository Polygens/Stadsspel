using UnityEngine;

namespace Stadsspel.Elements
{
	public enum BuildingType
	{
		NotSet,
		Bank,
		Tradingpost
	}

	public class Building : Element
	{
		protected BuildingType m_BuildingType = BuildingType.NotSet;

		/// <summary>
		/// Initialises the class.
		/// </summary>
		protected new void Start()
		{
			Team = TeamID.NoTeam;
			base.Start();
			ActionRadius = 15;

			GetComponent<Renderer>().material.color = TeamData.GetColor(m_BuildingType);
		}
	}
}