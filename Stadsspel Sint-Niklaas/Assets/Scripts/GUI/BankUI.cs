﻿using UnityEngine;
using UnityEngine.UI;
using System;

public class BankUI : MonoBehaviour
{
	//BankAccountManager manager = new BankAccountManager();
	public Text amountField;

	public Text amountOwnMoney;
	public Text amountBankMoney;

	private void OnEnable()
	{
		UpdateUI();
	}

	public void UpdateUI()
	{
		amountOwnMoney.text = GameManager.s_Singleton.Player.AmountOfMoney.ToString();
		amountBankMoney.text = GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Team - 1].BankAccount.Balance.ToString();
	}

	public void SelectAll(bool value)
	{
		if (value) {
			amountField.text = GameManager.s_Singleton.Player.AmountOfMoney.ToString();
		}
	}

	public void TransferMoney()
	{
		GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Team - 1].CmdTransaction(int.Parse(amountField.text));
	}

	public void RetractMoney()
	{
		GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Team - 1].CmdTransaction(-int.Parse(amountField.text));
	}
}
