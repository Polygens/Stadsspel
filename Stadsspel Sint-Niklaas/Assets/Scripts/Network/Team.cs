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

	private BankAccount mBankAccount;

	public TeamID TeamID {
		get {
			return mTeamID;
		}

		set {
			mTeamID = value;
		}
	}

	public int AmountOfDistricts {
		get {
			return mAmountOfDistricts;
		}
	}

	public BankAccount BankAccount {
		get {
			return mBankAccount;
		}

		set {
			mBankAccount = value;
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

	private void Start()
	{
		mBankAccount = GetComponent<BankAccount>();
	}

	[Command]
	public void CmdPlayerTransaction(int amount)
	{
		mBankAccount.PlayerTransaction(amount);
	}

	[Command]
	public void CmdTransaction(int amount)
	{
		mBankAccount.Transaction(amount);
	}
}
