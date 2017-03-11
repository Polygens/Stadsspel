using UnityEngine;
using UnityEngine.UI;
using Stadsspel.Elements;

public class Header : MonoBehaviour
{
	[SerializeField]
	private RectTransform m_ProfilePanel;
	[SerializeField]
	private RectTransform m_GoodsPanel;
	[SerializeField]
	private Image m_TeamColor;
	[SerializeField]
	private Text m_TeamMoney;
	[SerializeField]
	private Text m_PlayerMoney;

	private float m_UpdateTimer = 0;
	private float m_UpdateTime = 1;

	private void Start()
	{
		m_TeamColor.color = TeamData.GetColor(GameManager.s_Singleton.Player.Person.Team);
	}

	private void Update()
	{
		m_UpdateTimer += Time.deltaTime;
		if(m_UpdateTimer > m_UpdateTime) {
			m_UpdateTimer = 0;
			if(GameManager.s_Singleton.Player.Person) {

				// Header Update 
				UpdatePlayerMoney(GameManager.s_Singleton.Player.Person.AmountOfMoney);
				UpdateTeamMoney(GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].TotalMoney);
			}
		}
	}

	public void OpenProfilePanelOnClick()
	{
		m_ProfilePanel.gameObject.SetActive(true);
	}

	public void OpenInventoryOnClick()
	{
		m_GoodsPanel.gameObject.SetActive(true);
	}

	private void UpdatePlayerMoney(int pPlayerMoney)
	{
		m_PlayerMoney.text = pPlayerMoney.ToString();
		Debug.Log("Update player money");
	}

	private void UpdateTeamMoney(int pTeamMoney)
	{
		m_TeamMoney.text = pTeamMoney.ToString();
	}
}
