using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Banner : MonoBehaviour {

  public RectTransform profilePanel;
  public RectTransform GoederenPanel;

  public void OpenProfilePanelOnClick()
  {
    profilePanel.gameObject.SetActive(true);
  }

  public void OpenInventoryOnClick()
  {
    GoederenPanel.gameObject.SetActive(true);
  }
}
