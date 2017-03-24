using Photon;
using Stadsspel.Elements;
using UnityEngine;

public class Team : PunBehaviour
{
	[SerializeField]
	private TeamID m_TeamID;

	[SerializeField]
	//[SyncVar]
	private int m_TotalMoney = 0;

	//[SyncVar]
	private int m_AmountOfDistricts = 0;

	private BankAccount m_BankAccount;

	public TeamID TeamID {
		get {
			return m_TeamID;
		}
	}

	public int AmountOfDistricts {
		get {
			return m_AmountOfDistricts;
		}
	}

	public int TotalMoney {
		get {
			return m_TotalMoney;
		}
	}

	public BankAccount BankAccount {
		get {
			return m_BankAccount;
		}

		set {
			m_BankAccount = value;
		}
	}

	private void Awake()
	{
		transform.SetParent(GameManager.s_Singleton.transform);
		m_TeamID = (TeamID)(transform.GetSiblingIndex() + 1);
		name = TeamID.ToString();
	}

	[PunRPC]
	public void AddOrRemoveMoney(int amount)
	{
		m_TotalMoney += amount;
	}

	public void AddOrRemoveDistrict(int amount)
	{
		m_AmountOfDistricts += amount;
	}

	private void Start()
	{
		m_BankAccount = GetComponent<BankAccount>();
		m_BankAccount.Team = m_TeamID;
	}

	public void PlayerTransaction(int amount)
	{
		m_BankAccount.PlayerTransaction(amount);
		m_BankAccount.GetComponent<PhotonView>().RPC("Transaction", PhotonTargets.All, amount);
	}
}
