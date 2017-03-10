using UnityEngine;
using UnityEngine.UI;
using Stadsspel.Elements;

public class Header : MonoBehaviour
{
	[SerializeField]
	private RectTransform mProfilePanel;
	[SerializeField]
	private RectTransform mGoodsPanel;
	[SerializeField]
	private Image mTeamColor;
	[SerializeField]
	private Text mTeamMoney;
	[SerializeField]
	private Text mPlayerMoney;


	private Person mPerson;
	private bool mPersonIsSet = false;

	private float mUpdateTimer = 0;
	private float mUpdateTime = 1;

	public void Update()
	{
		mUpdateTimer += Time.deltaTime;
		if(mUpdateTimer > mUpdateTime) {
			mUpdateTimer = 0;
			if(GameManager.s_Singleton.Player.Person) {
				if(!mPersonIsSet) {
					mPersonIsSet = true;
					// Start: initial setup
					mTeamColor.color = TeamData.GetColor(GameManager.s_Singleton.Player.Person.Team);
				}

				// Header Update 
				UpdatePlayerMoney(GameManager.s_Singleton.Player.Person.AmountOfMoney);
				UpdateTeamMoney(GameManager.s_Singleton.Teams[(int)GameManager.s_Singleton.Player.Person.Team - 1].TotalMoney);
			}
		}
	}

	public void OpenProfilePanelOnClick()
	{
		mProfilePanel.gameObject.SetActive(true);
	}

	public void OpenInventoryOnClick()
	{
		mGoodsPanel.gameObject.SetActive(true);
	}

	private void UpdatePlayerMoney(int pPlayerMoney)
	{
		mPlayerMoney.text = pPlayerMoney.ToString();
		Debug.Log("Update player money");
	}

	private void UpdateTeamMoney(int pTeamMoney)
	{
		mTeamMoney.text = pTeamMoney.ToString();
	}
}
