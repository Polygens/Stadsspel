using UnityEngine;

public class MainSquareArrow : MonoBehaviour
{
	Vector3 m_HeadSquarePos;

	/// <summary>
	/// Initialises the class.
	/// </summary>
	private void Start()
	{
		m_HeadSquarePos = GameManager.s_Singleton.DistrictManager.GetHeadSquare(GameManager.s_Singleton.Player.Person.Team).transform.position;
	}

	/// <summary>
	/// Gets called every frame. Points the home arrow to the head disctrict of the player.
	/// </summary>
	private void Update()
	{
		Vector3 dir = m_HeadSquarePos - transform.position;
		transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90, Vector3.forward);
	}
}
