using UnityEngine;
using UnityEngine.UI;
using System;

public class BankUI : MonoBehaviour
{
	//BankAccountManager manager = new BankAccountManager();
	[SerializeField]
	private Text m_AmountField;
	[SerializeField]
	private Text m_AmountOwnMoney;
	[SerializeField]
	private Text m_AmountBankMoney;

	private void OnEnable()
	{
		UpdateUI();
	}

	public void UpdateUI()
	{
		m_AmountOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		m_AmountBankMoney.text = GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].BankAccount.Balance.ToString();
	}

	public void SelectAll(bool value)
	{
		if(value) {
			m_AmountField.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		}
	}

	public void TransferMoney()
	{
		GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].CmdPlayerTransaction(int.Parse(m_AmountField.text));
		UpdateUI();
	}

	public void RetractMoney()
	{
		GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].CmdPlayerTransaction(-int.Parse(m_AmountField.text));
		UpdateUI();
	}
}
