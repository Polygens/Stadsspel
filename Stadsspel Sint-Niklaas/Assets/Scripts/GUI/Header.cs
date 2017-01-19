using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Header : MonoBehaviour {

  public RectTransform mProfilePanel;
  public RectTransform mGoodsPanel;

  public void OpenProfilePanelOnClick()
  {
    mProfilePanel.gameObject.SetActive(true);
  }

  public void OpenInventoryOnClick()
  {
    mGoodsPanel.gameObject.SetActive(true);
  }
}
