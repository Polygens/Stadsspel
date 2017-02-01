using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Header : MonoBehaviour {

  public RectTransform mProfilePanel;
  public RectTransform mGoodsPanel;

    private Text teamMoney;
    private Text playerMoney;

    private Person player;

    public Person Player
    {
        set { player = value; }
    }

    public void Start()
    {
        //teamMoney = transform.FindChild("TeamMoneyAmount").GetComponent<Text>(); -> Null Ref ?
        //playerMoney = transform.FindChild("PlayerMoney").GetComponent<Text>(); -> Same
    }

    public void Update()
    {
        if(Time.frameCount % 30 == 0) //update every 30 frames = each second(mobile running at 30fps?)
        {
            UpdatePlayerMoney(player.AmountOfMoney);
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

    public void UpdatePlayerMoney(int pPlayerMoney)
    {
        //playerMoney.text = pPlayerMoney.ToString(); -> Null Ref ?
    }

    public void UpdateTeamMoney(int pTeamMoney)
    {
        teamMoney.text = pTeamMoney.ToString();
    }
}
