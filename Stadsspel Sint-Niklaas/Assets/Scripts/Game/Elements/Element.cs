using System.Linq;
using UnityEngine;
using Photon;

namespace Stadsspel.Elements
{
	public class Element : PunBehaviour
	{
		private float m_ActionRadius;

		[SerializeField]
		protected TeamID m_Team = TeamID.NotSet;

		[SerializeField]
		private string m_GameName = "Not set";

		protected void Start()
		{
		}

		public TeamID Team {
			get {
				return m_Team;
			}

			set {
				m_Team = value;
			}
		}

		public string Name {
			get {
				return m_GameName;
			}

			set {
				m_GameName = value;
			}
		}

		protected float ActionRadius {
			get {
				return m_ActionRadius;
			}

			set {
				m_ActionRadius = value;
				GetComponent<CircleCollider2D>().radius = m_ActionRadius;
				Mesh mesh = GetComponent<MeshFilter>().mesh;
				Vector3[] newMesh = mesh.vertices;

				newMesh[0] = new Vector3(-m_ActionRadius, -m_ActionRadius);
				newMesh[1] = new Vector3(m_ActionRadius, m_ActionRadius);
				newMesh[2] = new Vector3(m_ActionRadius, -m_ActionRadius);
				newMesh[3] = new Vector3(-m_ActionRadius, m_ActionRadius);
				mesh.SetVertices(newMesh.ToList());

				mesh.RecalculateBounds();
			}
		}

		/// <summary>
		/// Checks if the given position is within range based on the action radius
		/// </summary>
		public bool IsInRadius(Vector2 pos)
		{
			throw new System.NotImplementedException();
		}
	}
}