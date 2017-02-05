using UnityEngine;
using UnityEngine.Networking;

public class Team : NetworkBehaviour
{
	[SerializeField]
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
	}

	public int AmountOfDistricts {
		get {
			return mAmountOfDistricts;
		}
	}

    public int TotalMoney
    {
        get
        {
            return mTotalMoney;
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

	private void Awake()
	{
		transform.SetParent(GameManager.s_Singleton.transform);
		mTeamID = (TeamID)(transform.GetSiblingIndex() + 1);
		name = TeamID.ToString();
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
