using Stadsspel.Districts;
using Stadsspel.Elements;
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
	/// Gets called every frame. Performs a forced refresh in a regular interval of the Enemy Treasure UI.
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
		FindCurrentTreasure();
		UpdateUI();
	}

	/// <summary>
	/// Updates the value fields of the Enemy Treasure UI.
	/// </summary>
	private void UpdateUI()
	{
		//m_AmountOfOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		//m_AmountOfChestMoney.text = m_CurrentTreasure.AmountOfMoney.ToString();
		//m_Label.text = m_CurrentTreasure.GetRobAmount().ToString();
		m_AmountOfOwnMoney.text = CurrentGame.Instance.LocalPlayer.money.ToString();
		m_AmountOfChestMoney.text = "?";
		m_Label.text = "?";
	}

	/// <summary>
	/// Searches the treasure the player is on.
	/// </summary>
	private void FindCurrentTreasure()
	{
		Player p = GameManager.s_Singleton.Player;
		m_CurrentTreasure = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Treasure>();
	}

	/// <summary>
	/// Transfers the maximum rob amount from the treasure to the player.
	/// </summary>
	public void TransferMoney()
	{
		//int amount = int.Parse(m_CurrentTreasure.GetRobAmount().ToString());
		//GameManager.s_Singleton.Player.Person.TreasureTransaction(amount, true);

		CurrentGame.Instance.Ws.SendTreasuryRobbery(CurrentGame.Instance.currentDistrictID); //todo currently relying on the client to check for distance to treasury --> fix to server


		UpdateUI();
	}
}
