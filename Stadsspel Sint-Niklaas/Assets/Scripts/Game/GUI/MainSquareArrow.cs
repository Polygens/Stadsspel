using Stadsspel.Districts;
using UnityEngine;

public class MainSquareArrow : MonoBehaviour
{
	//Vector3 m_HeadSquarePos;
	GameObject m_headDistrictObject;

	/// <summary>
	/// Initialises the class.
	/// </summary>
	private void Start()
	{
		GameObject district = GameManager.s_Singleton.DistrictManager.GetDistrictByName(CurrentGame.Instance.GetMainSquare());
		HeadDistrict hd = district.GetComponent<HeadDistrict>();
		Treasure t = hd.transform.GetComponentInChildren<Treasure>();
		m_headDistrictObject = t.gameObject;
	}

	/// <summary>
	/// Gets called every frame. Points the home arrow to the head disctrict of the player.
	/// </summary>
	private void Update()
	{
		Vector3 dir = m_headDistrictObject.transform.position - transform.position;
		transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90, Vector3.forward);

		Renderer r = GetComponentInChildren<Renderer>();
		float distance = Vector3.Distance(transform.position, m_headDistrictObject.transform.position);
		if (distance < 50 && r.enabled)
		{
			r.enabled = false;
		}
		else if (distance >= 50 && !r.enabled)
		{
			r.enabled = true;
		}
	}
}
