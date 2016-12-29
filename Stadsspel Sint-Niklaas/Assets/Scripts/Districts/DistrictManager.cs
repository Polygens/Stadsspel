using UnityEngine;
using System.Collections;

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

	private PolygonCollider2D[] mDistrictColliders;

	// Use this for initialization
	void Start()
	{
		mDistrictColliders = new PolygonCollider2D[gameObject.transform.childCount];
		for (int i = 0; i < gameObject.transform.childCount; i++) {
			mDistrictColliders[i] = gameObject.transform.GetChild(i).gameObject.GetComponent<PolygonCollider2D>();
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
