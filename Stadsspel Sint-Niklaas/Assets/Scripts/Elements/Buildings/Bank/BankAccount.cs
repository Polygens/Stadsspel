using UnityEngine;
using UnityEngine.Networking;

public class BankAccount : NetworkBehaviour
{
	[SerializeField]
	[SyncVar]
	private int mBalance;

	private float interestMultiplier = 0.02f;

	public int Balance {
		get { return mBalance; }
	}

	private void Update()
	{
		mBalance += Mathf.RoundToInt(mBalance * interestMultiplier);//Add 2% from current balance to the total balance
	}

	public void Transaction(int amount)
	{
		mBalance += amount;
	}

	public bool PlayerTransaction(int amount)
	{
		if (amount > 0) //Add money to bank, subtract from player
		{
			if (amount <= GameManager.s_Singleton.Player.AmountOfMoney) {
				GameManager.s_Singleton.Player.MoneyTransaction(amount);
				mBalance += amount;
				return true;
			}
		}
		else //Subtract money from bank, add to player
		{
			if (amount <= mBalance) {
				GameManager.s_Singleton.Player.MoneyTransaction(amount);
				mBalance += amount;
				return true;
			}
		}
		return false;
	}
}
