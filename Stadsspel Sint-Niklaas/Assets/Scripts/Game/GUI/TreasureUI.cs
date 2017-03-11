using UnityEngine;
using UnityEngine.UI;
using Stadsspel.Districts;

public class TreasureUI : MonoBehaviour
{
	[SerializeField]
	private Text m_AmountOfOwnMoney;
	[SerializeField]
	private Text m_AmountOfChestMoney;
	[SerializeField]
	private InputField m_Input;

	private Treasure m_CurrentTreasure;

	private void Start()
	{
	
	}

	private void Update()
	{
	
	}

	private void OnEnable()
	{
		m_Input.text = "";
		FindCurrentTreasure();
		UpdateUI();
	}

	private void UpdateUI()
	{
		m_AmountOfOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		m_AmountOfChestMoney.text = m_CurrentTreasure.AmountOfMoney.ToString();
	}

	private void FindCurrentTreasure()
	{
		m_CurrentTreasure = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Treasure>();
	}

	public void TransferMoney()
	{
		int amount;
		int.TryParse(m_Input.text, out amount);
		Debug.Log("amount = " + amount);
		m_CurrentTreasure.CmdTransaction(amount);
		UpdateUI();
	}
}
