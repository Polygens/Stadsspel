using UnityEngine;

public class MainSquareArrow : MonoBehaviour
{
	Vector3 m_HeadSquarePos;

	/// <summary>
	/// TODO
	/// </summary>
	private void Start()
	{
		m_HeadSquarePos = GameManager.s_Singleton.DistrictManager.GetHeadSquare(GameManager.s_Singleton.Player.Person.Team).transform.position;
	}

	/// <summary>
	/// TODO
	/// </summary>
	private void Update()
	{
		Vector3 dir = m_HeadSquarePos - transform.position;
		transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90, Vector3.forward);
	}
}
