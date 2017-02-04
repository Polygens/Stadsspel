using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Header : MonoBehaviour {

  public RectTransform mProfilePanel;
  public RectTransform mGoodsPanel;

    public Text teamMoney;
    public Text playerMoney;

    private Player player;
    private bool playerFound = false;

    public void Start()
    {
        player = GameManager.s_Singleton.Player;
        if(player != null)
        {
            playerFound = true;
        }
    }

    public void Update()
    {
        if(Time.frameCount % 30 == 0) //update every 30 frames = each second(mobile running at 30fps?)
        {
            if(playerFound)
            {
                UpdatePlayerMoney(player.Person.AmountOfMoney);
            }      
        }

        if(!playerFound)
        {
            player = GameManager.s_Singleton.Player;

            if(player != null)
            {
                playerFound = true;
                Debug.Log("Player found for header");
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

    public void UpdatePlayerMoney(int pPlayerMoney)
    {
        Debug.Log("MoneyUpdated");
        playerMoney.text = pPlayerMoney.ToString();
    }

    public void UpdateTeamMoney(int pTeamMoney)
    {
        teamMoney.text = pTeamMoney.ToString();
    }
}
