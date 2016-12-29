using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class Team : NetworkBehaviour
{
	[SerializeField]
	private TeamID mTeamID;
	[SyncVar]
	private int mTotalMoney = 0;

	public TeamID TeamID {
		get {
			return mTeamID;
		}

		set {
			mTeamID = value;
		}
	}
}
