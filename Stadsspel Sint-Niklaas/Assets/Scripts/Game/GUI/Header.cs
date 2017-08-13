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
		//m_TeamColor.color = TeamData.GetColor(Stadsspel.Networking.TeamExtensions.GetTeam(PhotonNetwork.player));
		Color c = Color.magenta;
		ColorUtility.TryParseHtmlString(CurrentGame.Instance.PlayerTeam.customColor, out c);
		m_TeamColor.color = c;
	}

	/// <summary>
	/// Gets called every frame. Performs a forced refresh in a regular interval of the in game header.
	/// </summary>
	private void Update()
	{
		m_UpdateTimer += Time.deltaTime;
		if (m_UpdateTimer > m_UpdateTime)
		{
			m_UpdateTimer = 0;
			if (GameManager.s_Singleton.Player.Person)
			{
				// Header Update 
				UpdatePlayerMoney();
				UpdateTeamMoney();
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
	private void UpdatePlayerMoney()
	{
		m_PlayerMoney.text = (int)CurrentGame.Instance.LocalPlayer.money + ""; //todo format
	}

	/// <summary>
	/// Updates the textfield of the amount of money the team has.
	/// </summary>
	private void UpdateTeamMoney()
	{
		m_TeamMoney.text = (int)(CurrentGame.Instance.PlayerTeam.bankAccount + CurrentGame.Instance.PlayerTeam.treasury + CurrentGame.Instance.PlayerTeam.TotalPlayerMoney) + ""; //todo format + is this the correct money?
	}

	/// <summary>
	/// Updates the remaining time.
	/// </summary>
	private void UpdateGameTimer()
	{
		long start = CurrentGame.Instance.gameDetail.startTime;
		long current = (DateTime.UtcNow.Ticks - CurrentGame.timeOffset) / 10000;
		long end = CurrentGame.Instance.gameDetail.endTime;
		TimeSpan ts = TimeSpan.FromMilliseconds((double)(end - current));

		//checks whether a notification should be shown
		if (!CurrentGame.Instance.HalfwayPassed)
		{
			//game is not halfway
			TimeSpan total = TimeSpan.FromMilliseconds((double)(end - start));
			if (ts.TotalMinutes < (total.TotalMinutes / 2) && !CurrentGame.Instance.HalfwayPassed)
			{
				CurrentGame.Instance.HalfwayPassed = true;
				InGameUIManager.s_Singleton.LogUI.AddToLog("Het spel is halfweg", new object[] { });
			}
		} else
		{
			//halfway mark passed
			if (!CurrentGame.Instance.TenMinuteMark)
			{
				//probably more than 10 minutes left
				if (ts.TotalMinutes <= 10.0)
				{
					CurrentGame.Instance.TenMinuteMark = true;
					InGameUIManager.s_Singleton.LogUI.AddToLog("Nog 10 minuten over", new object[] { });
				}
			} else
			{
				//last 10 minute mark passed
				if (!CurrentGame.Instance.LastMinuteMark && ts.TotalMinutes <= 1.0)
				{
					CurrentGame.Instance.LastMinuteMark = true;
					InGameUIManager.s_Singleton.LogUI.AddToLog("Laatste minuut", new object[] { });
				}

			}
		}

		string time = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
		m_GameDuration.text = time;
	}
}
