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

  private float m_Timer;

	private void OnEnable()
	{
		UpdateUI();
	}

  private void Update()
  {
    m_Timer += Time.deltaTime;
    if (m_Timer >= 1)
    { //Refreshing for better feedback, can be adjusted if necessary
      UpdateUI();
      m_Timer = 0;
    }
  }

  [PunRPC]
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
    if (GameManager.s_Singleton.Player.Person.AmountOfMoney >= int.Parse(m_AmountField.text))
    {
      GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].PlayerTransaction(int.Parse(m_AmountField.text));
      UpdateUI();
    }

    //GetComponent<PhotonView>().RPC("UpdateUI", PhotonTargets.All);
	}

	public void RetractMoney()
	{
    if (GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].BankAccount.Balance >= int.Parse(m_AmountField.text))
    {
      GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].PlayerTransaction(-int.Parse(m_AmountField.text));
    }

    UpdateUI();
    //GetComponent<PhotonView>().RPC("UpdateUI", PhotonTargets.All);
  }
}
