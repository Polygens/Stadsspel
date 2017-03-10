using UnityEngine;
using UnityEngine.UI;
using Stadsspel.Districts;

public class TreasureEnemyUI : MonoBehaviour
{
	public Text amountOfOwnMoney;
	public Text amountOfChestMoney;
	public InputField label;

	private Treasure currentTreasure;

	void Start()
	{
	
	}

	void Update()
	{
	
	}

	private void OnEnable()
	{
		FindCurrentTreasure();
		UpdateUI();
	}

	private void UpdateUI()
	{
		amountOfOwnMoney.text = GameManager.s_Singleton.Player.Person.AmountOfMoney.ToString();
		amountOfChestMoney.text = currentTreasure.AmountOfMoney.ToString();
		label.text = currentTreasure.GetRobAmount().ToString();
	}

	private void FindCurrentTreasure()
	{
		currentTreasure = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Treasure>();
	}

	public void TransferMoney()
	{
		int amount = int.Parse(currentTreasure.GetRobAmount().ToString());
		currentTreasure.CmdTransaction(amount);
		UpdateUI();
	}
}
