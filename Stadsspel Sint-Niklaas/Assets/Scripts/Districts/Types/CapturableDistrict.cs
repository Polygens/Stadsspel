using UnityEngine;

public class CapturableDistrict : District
{
	private new void Start()
	{
		mDistrictType = DistrictType.CapturableDistrict;
		base.Start();
	}

	protected override void OnTeamChanged()
	{
		gameObject.GetComponent<Renderer>().material.color = TeamData.GetColor(mTeamID);
		gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = TeamData.GetColor(mTeamID);
	}
}
