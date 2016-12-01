using UnityEngine;

public enum DistrictType
{
	NotSet,
	HeadDistrict,
	Neutral,
	GrandMarket,
	CapturableDistrict,
	Outside,
	square
}

public class District : MonoBehaviour
{
	[SerializeField]
	private TeamID mTeamID = 0;
	[SerializeField]
	private DistrictType mDistrictType = 0;

	private Color SquareColor = Color.yellow;

	public TeamID TeamID {
		get {
			return mTeamID;
		}

		set {
			mTeamID = value;
			gameObject.GetComponent<Renderer>().material.color = TeamData.GetColor(mTeamID, mDistrictType);
			if (mDistrictType == DistrictType.CapturableDistrict || mDistrictType == DistrictType.HeadDistrict) {
				gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = TeamData.GetColor(mTeamID, DistrictType.square);
			}
		}
	}

	public District(TeamID teamID, DistrictType type)
	{
		mTeamID = teamID;
		mDistrictType = type;
	}

	private void Start()
	{
		TeamID = mTeamID;
	}
}