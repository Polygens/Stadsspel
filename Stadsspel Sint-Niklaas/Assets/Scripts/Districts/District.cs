using UnityEngine;
using UnityEngine.Networking;

public enum DistrictType
{
	NotSet,
	Neutral,
	GrandMarket,
	HeadDistrict,
	CapturableDistrict,
	square,
	Outside,
}

public class District : NetworkBehaviour
{
	[SerializeField]
	[SyncVar]
	protected TeamID mTeamID = 0;

	[SerializeField]
	protected DistrictType mDistrictType = 0;

	public TeamID TeamID {
		get {
			return mTeamID;
		}

		set {
			mTeamID = value;
			OnTeamChanged();
		}
	}

	protected virtual void OnTeamChanged()
	{
	}

	protected void Start()
	{
		gameObject.GetComponent<Renderer>().material.color = TeamData.GetColor(mDistrictType);
	}
}
