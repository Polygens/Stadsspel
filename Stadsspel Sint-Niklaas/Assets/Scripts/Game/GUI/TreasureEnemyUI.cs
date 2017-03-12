using UnityEngine;
using UnityEngine.UI;
using Stadsspel.Districts;

public class TreasureEnemyUI : MonoBehaviour
{
	[SerializeField]
	private Text m_AmountOfOwnMoney;
	[SerializeField]
	private Text m_AmountOfChestMoney;
	[SerializeField]
	private InputField m_Label;

	private Treasure m_CurrentTreasure;

	private void Start()
	{
	
	}

	private void Update()
	{
	
	}

	private void OnEnable()
	{
		FindCurrentTreasure();
		UpdateUI();
	}

	private void UpdateUI()
	{
		m_AmountOfOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		m_AmountOfChestMoney.text = m_CurrentTreasure.AmountOfMoney.ToString();
		m_Label.text = m_CurrentTreasure.GetRobAmount().ToString();
	}

	private void FindCurrentTreasure()
	{
		m_CurrentTreasure = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Treasure>();
	}

	public void TransferMoney()
	{
		int amount = int.Parse(m_CurrentTreasure.GetRobAmount().ToString());
		GameManager.s_Singleton.Player.Person.TreasureTransaction(amount, true);
		UpdateUI();
	}
}
