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
		ServerTeam playerTeam =CurrentGame.Instance.gameDetail.teams.Find(t => t.teamName.Equals(CurrentGame.Instance.PlayerTeam.teamName));
		m_AmountOwnMoney.text = ""+CurrentGame.Instance.LocalPlayer.money;
		m_AmountBankMoney.text = "" + playerTeam.bankAccount;
	}

	/// <summary>
	/// Transfers all the player's money to the bank.
	/// </summary>
	public void SelectAllOwnMoney()
	{
		m_AmountField.text = "" + CurrentGame.Instance.LocalPlayer.money; ;
		TransferMoney();
	}

	/// <summary>
	/// Transfers all the bank's money to the player.
	/// </summary>
	public void SelectAllBankMoney()
	{
		ServerTeam playerTeam = CurrentGame.Instance.gameDetail.teams.Find(t => t.teamName.Equals(CurrentGame.Instance.PlayerTeam.teamName));
		m_AmountField.text = "" + playerTeam.bankAccount;
		RetractMoney();
	}

	/// <summary>
	/// Verifies if transaction is valid and performs deposition transaction.
	/// </summary>
	public void TransferMoney()
	{
		if(CurrentGame.Instance.LocalPlayer.money >= (double.Parse(m_AmountField.text)-0.000001)) {
			CurrentGame.Instance.Ws.SendBankDeposit(double.Parse(m_AmountField.text),CurrentGame.Instance.nearBank);
		}

		//GetComponent<PhotonView>().RPC("UpdateUI", PhotonTargets.All);
	}

	/// <summary>
	/// Verifies if transaction is valid and performs retraction transaction.
	/// </summary>
	public void RetractMoney()
	{
		ServerTeam playerTeam = CurrentGame.Instance.gameDetail.teams.Find(t => t.teamName.Equals(CurrentGame.Instance.PlayerTeam.teamName));
		if (playerTeam.bankAccount >= (double.Parse(m_AmountField.text) - 0.000001)) {
			CurrentGame.Instance.Ws.SendBankWithdrawal(double.Parse(m_AmountField.text), CurrentGame.Instance.nearBank);
		}

		UpdateUI();
		//GetComponent<PhotonView>().RPC("UpdateUI", PhotonTargets.All);
	}
}
