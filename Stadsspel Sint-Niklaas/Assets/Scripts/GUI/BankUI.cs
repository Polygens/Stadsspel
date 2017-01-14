using UnityEngine;
using UnityEngine.UI;
using System;

public class BankUI : MonoBehaviour
{
	//BankAccountManager manager = new BankAccountManager();
	public Text amountField;

	public Text amountOwnMoney;
	public Text amountBankMoney;

	private Player player;

	public void Awake()
	{
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
	}

	private void OnEnable()
	{
		UpdateUI();
	}

	public void UpdateUI()
	{
		amountOwnMoney.text = player.AmountOfMoney.ToString();
		amountBankMoney.text = BankAccountManager.BankAccounts[Convert.ToInt32(player.Team)].Balance.ToString();
	}

	public void SelectAll(bool value)
	{
		if (value) {
			amountField.text = player.AmountOfMoney.ToString();
		}
	}

	public void TransferMoney()
	{
		int amount = int.Parse(amountField.text);
	}

	public void RetractMoney()
	{
		int amount = int.Parse(amountField.text);
	}
}
