using UnityEngine;

namespace Stadsspel.Elements
{
	public class Friend : MonoBehaviour
	{
		private Person m_Person;

		/// <summary>
		/// Initialises the class.
		/// </summary>
		private void Start()
		{
			m_Person = GetComponent<Person>();
			tag = "Team";
			Destroy(transform.GetChild(1).gameObject);
			Destroy(transform.GetChild(2).gameObject);
			Destroy(transform.GetChild(3).gameObject);
		}
	}
}