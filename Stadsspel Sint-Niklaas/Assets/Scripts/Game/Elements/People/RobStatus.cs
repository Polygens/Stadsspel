using UnityEngine;

public class RobStatus : MonoBehaviour
{

	private bool m_RecentlyGotRobbed = false;

	public bool RecentlyGotRobbed {
		get {
			return m_RecentlyGotRobbed;
		}
		set {
			m_RecentlyGotRobbed = value;
		}
	}
}
