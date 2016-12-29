using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class BankAccountManager : NetworkBehaviour
{
	private static List<BankAccount> bankAccounts = new List<BankAccount>();

	void Start()
	{
		CmdAddToList();
	}

	public static List<BankAccount> BankAccounts {
		get { return bankAccounts; }
	}

	public BankAccount CreateBankAccount(byte id)
	{
		return new BankAccount(id);
	}

	[Command]
	void CmdAddToList()
	{
		// this code is only executed on the server
		RpcAddToList(); // invoke Rpc on all clients
	}

	[ClientRpc]
	void RpcAddToList()
	{
		// this code is executed on all clients
		for (byte i = 0; i < GameManager.s_Singleton.mTeams; i++) {
			bankAccounts.Add(CreateBankAccount(i));
		}
	}

	[Command]
	public void CmdTransaction(byte teamID, int amount)
	{
		RpcTransaction(teamID, amount);
	}

	[ClientRpc]
	void RpcTransaction(byte teamID, int amount)
	{
		bankAccounts[teamID].Transaction(amount);
	}

}
