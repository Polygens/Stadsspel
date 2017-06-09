using System;
using Stadsspel.Elements;
using UnityEngine;
using UnityEngine.UI;

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
	[SerializeField]
	private Text m_GameDuration;

	private float m_UpdateTimer = 0;
	private float m_UpdateTime = 1;

	/// <summary>
	/// Initialises the class.
	/// </summary>
	private void Start()
	{
		m_TeamColor.color = TeamData.GetColor(Stadsspel.Networking.TeamExtensions.GetTeam(PhotonNetwork.player));
	}

	/// <summary>
	/// Gets called every frame. Performs a forced refresh in a regular interval of the in game header.
	/// </summary>
	private void Update()
	{
		m_UpdateTimer += Time.deltaTime;
		if(m_UpdateTimer > m_UpdateTime) {
			m_UpdateTimer = 0;
			if(GameManager.s_Singleton.Player.Person) {
				// Header Update 
				UpdatePlayerMoney(GameManager.s_Singleton.Player.Person.AmountOfMoney);
				UpdateTeamMoney(GameManager.s_Singleton.Teams[CurrentGame.Instance.gameDetail.IndexOfTeam(GameManager.s_Singleton.Player.Person.Team)].TotalMoney);//todo link to actual team data
				UpdateGameTimer();
			}
		}
	}

	/// <summary>
	/// Event for profile panel button. Enables the in game menu.
	/// </summary>
	public void OpenProfilePanelOnClick()
	{
		m_ProfilePanel.gameObject.SetActive(true);
	}

	/// <summary>
	/// Event for inventory panel button. Enables the goods/inventory panel.
	/// </summary>
	public void OpenInventoryOnClick()
	{
		m_GoodsPanel.gameObject.SetActive(true);
	}

	/// <summary>
	/// Updates the textfield of the amount of money the player has.
	/// </summary>
	private void UpdatePlayerMoney(int pPlayerMoney)
	{
		m_PlayerMoney.text = ((int)CurrentGame.Instance.LocalPlayer.money)+""; //todo format
	}

	/// <summary>
	/// Updates the textfield of the amount of money the team has.
	/// </summary>
	private void UpdateTeamMoney(int pTeamMoney)
	{
		m_TeamMoney.text = ((int) (CurrentGame.Instance.PlayerTeam.bankAccount + CurrentGame.Instance.PlayerTeam.treasury)) + ""; //todo format + is this the correct money?
	}

	/// <summary>
	/// Updates the remaining time.
	/// </summary>
	private void UpdateGameTimer()
	{
		float timer = GameManager.s_Singleton.GameLength - Time.timeSinceLevelLoad;

		//int hours = Mathf.FloorToInt(timer / 3600);
		int minutes = Mathf.FloorToInt(timer / 60);
		int defMinutes;
		int hours = Math.DivRem(minutes, 60, out defMinutes);
		int seconds = Mathf.FloorToInt(timer - minutes * 60);

		string time = string.Format("{0:00}:{1:00}:{2:00}", hours, defMinutes, seconds);

		m_GameDuration.text = time;
	}
}
