using Stadsspel.Districts;
using UnityEngine;
using UnityEngine.UI;

public class TreasureEnemyUI : MonoBehaviour
{
	[SerializeField]
	private Text m_AmountOfOwnMoney;
	[SerializeField]
	private Text m_AmountOfChestMoney;
	[SerializeField]
	private InputField m_Label;

	private Treasure m_CurrentTreasure;
	private float m_Timer;

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
	/// TODO
	/// </summary>
	private void OnEnable()
	{
		FindCurrentTreasure();
		UpdateUI();
	}

	/// <summary>
	/// TODO
	/// </summary>
	private void UpdateUI()
	{
		m_AmountOfOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		m_AmountOfChestMoney.text = m_CurrentTreasure.AmountOfMoney.ToString();
		m_Label.text = m_CurrentTreasure.GetRobAmount().ToString();
	}

	/// <summary>
	/// TODO
	/// </summary>
	private void FindCurrentTreasure()
	{
		m_CurrentTreasure = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Treasure>();
	}

	/// <summary>
	/// TODO
	/// </summary>
	public void TransferMoney()
	{
		int amount = int.Parse(m_CurrentTreasure.GetRobAmount().ToString());
		GameManager.s_Singleton.Player.Person.TreasureTransaction(amount, true);
		UpdateUI();
	}
}
