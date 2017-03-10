using System.Linq;
using UnityEngine;
using Photon;

namespace Stadsspel.Elements
{
	public class Element : PunBehaviour
	{
		private float mActionRadius;

		[SerializeField]
		protected TeamID m_Team = TeamID.NotSet;

		[SerializeField]
		private string m_Name = "Not set";

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
				return m_Name;
			}

			set {
				m_Name = value;
			}
		}

		protected float ActionRadius {
			get {
				return mActionRadius;
			}

			set {
				mActionRadius = value;
				GetComponent<CircleCollider2D>().radius = mActionRadius;
				Mesh mesh = GetComponent<MeshFilter>().mesh;
				Vector3[] newMesh = mesh.vertices;

				newMesh[0] = new Vector3(-mActionRadius, -mActionRadius);
				newMesh[1] = new Vector3(mActionRadius, mActionRadius);
				newMesh[2] = new Vector3(mActionRadius, -mActionRadius);
				newMesh[3] = new Vector3(-mActionRadius, mActionRadius);
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