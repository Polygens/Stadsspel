using UnityEngine;

namespace Stadsspel.Elements
{
	public class Enemy : MonoBehaviour
	{
		private const int m_DetectionRadius = 150;
		private const float m_DetectionTime = .5f;
		private float m_Timer;
		private Person m_Person;

		private MeshRenderer m_CircleMesh;
		private MeshRenderer m_TextMesh;

		private void Start()
		{
			m_Person = GetComponent<Person>();
			tag = "Enemy";
			Destroy(transform.GetChild(1).gameObject);
			Destroy(transform.GetChild(2).gameObject);
			Destroy(transform.GetChild(3).gameObject);

			m_Timer = m_DetectionTime;

			m_CircleMesh = GetComponent<MeshRenderer>();
			m_TextMesh = transform.GetChild(0).GetComponent<MeshRenderer>();
		}

		private void Update()
		{
			m_Timer -= Time.deltaTime;
			if(m_Timer < 0) {
				m_Timer = m_DetectionTime;
				if(Vector2.Distance(transform.position, GameManager.s_Singleton.Player.transform.position) > m_DetectionRadius) {
					m_CircleMesh.enabled = false;
					m_TextMesh.enabled = false;
				} else {
					m_CircleMesh.enabled = true;
					m_TextMesh.enabled = true;
				}
			}
		}
	}
}