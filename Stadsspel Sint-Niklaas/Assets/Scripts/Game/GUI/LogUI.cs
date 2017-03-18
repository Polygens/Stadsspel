using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
	[SerializeField]
	private RectTransform m_LogScrollViewContent;

	public const string m_HasDepositedMoneyInBank = "";

	public void AddToLog(string text, int[] variables)
	{
		GameObject notification = Instantiate(Resources.Load("Notification") as GameObject, InGameUIManager.s_Singleton.LogNotifications, false);
		notification.GetComponent<Notification>().SetText(text);

		GameObject logItem = Instantiate(Resources.Load("LogItem") as GameObject, m_LogScrollViewContent, false);
		Text textLog = logItem.transform.GetChild(0).GetComponent<Text>();
		textLog.text = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ": " + text;
		if(logItem.transform.GetSiblingIndex() % 2 == 0) {
			logItem.GetComponent<Image>().color = new Color(.95f, .95f, .95f);
		} else {
			logItem.GetComponent<Image>().color = new Color(.9f, .9f, .9f);
		}
	}

	public void ToggleLogsPanel(bool newState)
	{
		for(int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(newState);
		}
	}
}
