using UnityEngine;

public enum BuildingType
{
	NotSet,
	Bank,
	Tradingpost
}

public class Building : Element
{
	protected BuildingType mBuildingType = BuildingType.NotSet;

	protected new void Start()
	{
		Team = TeamID.NoTeam;
		base.Start();
		ActionRadius = 15;

		GetComponent<Renderer>().material.color = TeamData.GetColor(mBuildingType);
	}
}
