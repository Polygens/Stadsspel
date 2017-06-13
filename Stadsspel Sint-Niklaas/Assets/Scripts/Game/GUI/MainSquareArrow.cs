using Stadsspel.Districts;
using UnityEngine;

public class MainSquareArrow : MonoBehaviour
{
	Vector3 m_HeadSquarePos;

	/// <summary>
	/// Initialises the class.
	/// </summary>
	private void Start()
	{
		GameObject district = GameManager.s_Singleton.DistrictManager.GetDistrictByName(CurrentGame.Instance.GetMainSquare());
		HeadDistrict hd = district.GetComponent<HeadDistrict>();
		Treasure t = hd.transform.GetComponentInChildren<Treasure>();
		m_HeadSquarePos = t.transform.position;
	}

	/// <summary>
	/// Gets called every frame. Points the home arrow to the head disctrict of the player.
	/// </summary>
	private void Update()
	{
		Vector3 dir = m_HeadSquarePos - transform.position;
		transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90, Vector3.forward);

		Renderer r = GetComponentInChildren<Renderer>();
		float distance = Vector3.Distance(transform.position, m_HeadSquarePos);
		if (distance < 100 && r.enabled)
		{
			r.enabled = false;
		}
		else if (distance >= 100 && !r.enabled)
		{
			r.enabled = true;
		}
	}
}
