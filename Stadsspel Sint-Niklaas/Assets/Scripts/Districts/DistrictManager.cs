using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public enum DistrictStates
{
	NoTerritory,
	EnemyTerritory,
	FriendlyTerritory,
	UncapturedTerritory,
	CentralMarket
}

public class DistrictManager : MonoBehaviour
{
	public Transform mPlayerTrans;

  [SerializeField]
	private PolygonCollider2D[] mDistrictColliders;

	public void StartGame(int amountOfTeams)
	{
		mDistrictColliders = new PolygonCollider2D[gameObject.transform.childCount];
		for (int i = 0; i < gameObject.transform.childCount; i++) {
			mDistrictColliders[i] = gameObject.transform.GetChild(i).gameObject.GetComponent<PolygonCollider2D>();
		}

		for (int i = 1; i <= 6; i++) {
			if (amountOfTeams > i) {
				HeadDistrict district = transform.GetChild(i).gameObject.AddComponent<HeadDistrict>();
				district.TeamID = (TeamID)(i);
				Treasure square = district.transform.GetChild(0).gameObject.AddComponent<Treasure>();
				square.TeamID = (TeamID)(i);
			}
			else {
				CapturableDistrict district = transform.GetChild(i).gameObject.AddComponent<CapturableDistrict>();
				district.TeamID = TeamID.NoTeam;
				CapturePoint square = district.transform.GetChild(0).gameObject.AddComponent<CapturePoint>();
				square.TeamID = TeamID.NoTeam;
			}
		}
	}

	public DistrictStates CheckDisctrictState()
	{
		for (int i = 0; i < mDistrictColliders.Length; i++) {
			if (mDistrictColliders[i].OverlapPoint(mPlayerTrans.position)) {
				if (mDistrictColliders[i].transform.childCount > 0) {
					BoxCollider2D square = mDistrictColliders[i].transform.GetChild(0).GetComponent<BoxCollider2D>();
					if (square.OverlapPoint(mPlayerTrans.position)) {
						Debug.Log(square.gameObject.name);
					}
					else {
						Debug.Log(mDistrictColliders[i].gameObject.name);
					}
				}
				else {
					Debug.Log(mDistrictColliders[i].gameObject.name);
				}

				break;
			}
		}
		return DistrictStates.NoTerritory;
	}
}
