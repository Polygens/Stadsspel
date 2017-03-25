using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour
{
	//BankAccountManager manager = new BankAccountManager();
	[SerializeField]
	private InputField m_AmountField;
	[SerializeField]
	private Text m_AmountOwnMoney;
	[SerializeField]
	private Text m_AmountBankMoney;

	private float m_Timer;

	/// <summary>
	/// Gets called when the GameObject becomes active.
	/// </summary>
	private void OnEnable()
	{
		m_AmountField.text = "";
		UpdateUI();

	}

	/// <summary>
	/// Gets called every frame. Performs a forced refresh in a regular interval of the bank UI.
	/// </summary>
	private void Update()
	{
		m_Timer += Time.deltaTime;
		if(m_Timer >= 1) { //Refreshing for better feedback, can be adjusted if necessary
			UpdateUI();
			m_Timer = 0;
		}
	}

	/// <summary>
	/// [PunRPC] Updates the value fields of the UI.
	/// </summary>
	[PunRPC]
	public void UpdateUI()
	{
		m_AmountOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		m_AmountBankMoney.text = GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].BankAccount.Balance.ToString();
	}

	/// <summary>
	/// Transfers all the player's money to the bank.
	/// </summary>
	public void SelectAllOwnMoney()
	{
		m_AmountField.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		TransferMoney();
	}

	/// <summary>
	/// Transfers all the bank's money to the player.
	/// </summary>
	public void SelectAllBankMoney()
	{
		m_AmountField.text = GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].BankAccount.Balance.ToString();
		RetractMoney();
	}

	/// <summary>
	/// Verifies if transaction is valid and performs deposition transaction.
	/// </summary>
	public void TransferMoney()
	{
		if(GameManager.s_Singleton.Player.Person.AmountOfMoney >= int.Parse(m_AmountField.text)) {
			GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].PlayerTransaction(int.Parse(m_AmountField.text));
			UpdateUI();

		}

		//GetComponent<PhotonView>().RPC("UpdateUI", PhotonTargets.All);
	}

	/// <summary>
	/// Verifies if transaction is valid and performs retraction transaction.
	/// </summary>
	public void RetractMoney()
	{
		if(GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].BankAccount.Balance >= int.Parse(m_AmountField.text)) {
			GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].PlayerTransaction(-int.Parse(m_AmountField.text));
		}

		UpdateUI();
		//GetComponent<PhotonView>().RPC("UpdateUI", PhotonTargets.All);
	}
}
