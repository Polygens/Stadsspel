using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Element : NetworkBehaviour
{
	private float mActionRadius;

	[SerializeField]
	[SyncVar(hook = "OnTeamChange")]
	protected TeamID mTeam = TeamID.NotSet;

	[SerializeField]
	[SyncVar(hook = "OnNameChange")]
	private string mName = "Not set";

	[Command]
	public void CmdSetName(string newName)
	{
		mName = newName;
	}

	[Command]
	public void CmdSetTeam(TeamID newTeamID)
	{
		mTeam = newTeamID;
	}

	protected void Start()
	{
	}

	public Vector2 Position {
		get {
			throw new System.NotImplementedException();
		}

		set {
		}
	}

	public TeamID Team {
		get {
			return mTeam;
		}

		set {
			mTeam = value;
		}
	}

	public string Name {
		get {
			return mName;
		}

		set {
			mName = value;
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

	public virtual void OnNameChange(string newName)
	{

	}

	public virtual void OnTeamChange(TeamID newTeam)
	{
	}
}
