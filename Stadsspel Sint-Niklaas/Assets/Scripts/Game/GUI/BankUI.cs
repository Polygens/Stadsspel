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
	/// TODO
	/// </summary>
	private void OnEnable()
	{
		m_AmountField.text = "";
		UpdateUI();

	}

	/// <summary>
	/// TODO
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
	/// [PunRPC] TODO
	/// </summary>
	[PunRPC]
	public void UpdateUI()
	{
		m_AmountOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		m_AmountBankMoney.text = GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].BankAccount.Balance.ToString();
	}

	/// <summary>
	/// TODO
	/// </summary>
	public void SelectAllOwnMoney()
	{
		m_AmountField.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		TransferMoney();
	}

	/// <summary>
	/// TODO
	/// </summary>
	public void SelectAllBankMoney()
	{
		m_AmountField.text = GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].BankAccount.Balance.ToString();
		RetractMoney();
	}

	/// <summary>
	/// TODO
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
	/// TODO
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
