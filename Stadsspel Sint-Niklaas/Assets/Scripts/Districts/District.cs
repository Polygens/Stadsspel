using UnityEngine;

public class District : MonoBehaviour
{
	private int mTeam = 0;

	public int Team {
		get {
			return mTeam;
		}

		set {
			mTeam = value;
		}
	}
}