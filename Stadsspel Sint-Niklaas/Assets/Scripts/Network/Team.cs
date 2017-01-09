using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class Team : NetworkBehaviour
{
	[SerializeField]
	[SyncVar]
	private TeamID mTeamID;

	[SerializeField]
	[SyncVar]
	private int mTotalMoney = 0;

    [SyncVar]
    private int mAmountOfDistricts = 1;

	public TeamID TeamID {
		get {
			return mTeamID;
		}

		set {
			mTeamID = value;
		}
	}

    public int AmountOfDistricts
    {
        get
        {
            return mAmountOfDistricts;
        }
    }

	public void AddOrRemoveMoney(int amount)
	{
		mTotalMoney += amount;
	}

    public void AddOrRemoveDistrict(int amount)
    {
        mAmountOfDistricts += amount;
    }
}
