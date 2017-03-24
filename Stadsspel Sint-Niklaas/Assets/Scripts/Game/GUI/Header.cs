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
	/// TODO
	/// </summary>
	private void Start()
	{
		m_TeamColor.color = TeamData.GetColor(Stadsspel.Networking.TeamExtensions.GetTeam(PhotonNetwork.player));
	}

	/// <summary>
	/// TODO
	/// </summary>
	private void Update()
	{
		m_UpdateTimer += Time.deltaTime;
		if(m_UpdateTimer > m_UpdateTime) {
			m_UpdateTimer = 0;
			if(GameManager.s_Singleton.Player.Person) {

				// Header Update 
				UpdatePlayerMoney(GameManager.s_Singleton.Player.Person.AmountOfMoney);
				UpdateTeamMoney(GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].TotalMoney);
				UpdateGameTimer();
			}
		}
	}

	/// <summary>
	/// TODO
	/// </summary>
	public void OpenProfilePanelOnClick()
	{
		m_ProfilePanel.gameObject.SetActive(true);
	}

	/// <summary>
	/// TODO
	/// </summary>
	public void OpenInventoryOnClick()
	{
		m_GoodsPanel.gameObject.SetActive(true);
	}

	/// <summary>
	/// TODO
	/// </summary>
	private void UpdatePlayerMoney(int pPlayerMoney)
	{
		m_PlayerMoney.text = pPlayerMoney.ToString();
#if(UNITY_EDITOR)
		Debug.Log("Update player money");
#endif
	}

	/// <summary>
	/// TODO
	/// </summary>
	private void UpdateTeamMoney(int pTeamMoney)
	{
		m_TeamMoney.text = pTeamMoney.ToString();
	}

	/// <summary>
	/// TODO
	/// </summary>
	private void UpdateGameTimer()
	{
		float timer = GameManager.s_Singleton.GameLength - Time.timeSinceLevelLoad;

		int hours = Mathf.FloorToInt(timer / 3600);
		int minutes = Mathf.FloorToInt(timer / 60);
		int seconds = Mathf.FloorToInt(timer - minutes * 60);

		string time = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

		m_GameDuration.text = time;
	}
}
