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

	/// <summary>
	/// Returns the treasure the player is currently on.
	/// </summary>
	public Treasure CurrentTreasure()
	{
		return m_CurrentTreasure;
	}

	/// <summary>
	/// Gets called every frame. Performs a forced refresh in a regular interval of the Treasure UI.
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
	/// Gets called when the GameObject becomes active.
	/// </summary>
	private void OnEnable()
	{
		m_Input.text = "";
		FindCurrentTreasure();
		UpdateUI();
	}

	/// <summary>
	/// Updates the value fields of the UI.
	/// </summary>
	private void UpdateUI()
	{
		m_AmountOfOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		m_AmountOfChestMoney.text = m_CurrentTreasure.AmountOfMoney.ToString();
	}

	/// <summary>
	/// Searches the treasure the player is on.
	/// </summary>
	private void FindCurrentTreasure()
	{
		m_CurrentTreasure = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Treasure>();
	}

	/// <summary>
	/// Transfers the amount in the textfield from the treasure to the player.
	/// </summary>
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
