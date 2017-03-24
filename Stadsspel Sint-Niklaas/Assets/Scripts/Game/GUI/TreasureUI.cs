using Stadsspel.Districts;
using UnityEngine;
using UnityEngine.UI;

public class TreasureUI : MonoBehaviour
{
	[SerializeField]
	private Text m_AmountOfOwnMoney;
	[SerializeField]
	private Text m_AmountOfChestMoney;
	[SerializeField]
	private InputField m_Input;
	private float m_Timer;

	private Treasure m_CurrentTreasure;

	public Treasure CurrentTreasure()
	{
		return m_CurrentTreasure;
	}

	private void Update()
	{
		m_Timer += Time.deltaTime;
		if(m_Timer >= 1) { //Refreshing for better feedback, can be adjusted if necessary
			UpdateUI();
			m_Timer = 0;
		}
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
		if(m_CurrentTreasure.IsMoneyTranferValid(amount)) {
#if(UNITY_EDITOR)
			Debug.Log("Valid Transaction");
#endif
			GameManager.s_Singleton.Player.Person.TreasureTransaction(amount, false);
		}
		UpdateUI();
	}
}
