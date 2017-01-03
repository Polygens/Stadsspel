using UnityEngine;
using UnityEngine.Networking;

public class Element : NetworkBehaviour
{
	private float mActionRadius;

	[SerializeField]
	[SyncVar]
	protected TeamID mTeam = TeamID.NotSet;

	[SerializeField]
	[SyncVar(hook = "OnNameChange")]
	protected string mName = "Not set";

	public Element()
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

	/// <summary>
	/// Checks if the given position is within range based on the action radius
	/// </summary>
	public bool IsInRadius(Vector2 pos)
	{
		throw new System.NotImplementedException();
	}

	public virtual void OnNameChange(string newName)
	{
		gameObject.name = newName;
	}
}
