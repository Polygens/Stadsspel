using UnityEngine;
using UnityEngine.UI;


public class LogUI : MonoBehaviour
{
	[SerializeField]
	private RectTransform m_LogScrollViewContent;

	public const string m_HasDepositedMoneyInBank = "{0} heeft {1} in de bank gestort";
	public const string m_HasWithdrawnMoneyInBank = "{0} heeft {1} in de bank opgehaald";
	public const string m_Capturing = "AAN HET VEROVEREN";
	public const string m_WasCaptured = "{0} is nu van {1}";
	public const string m_TaxesIncome = "Belastingsresultaat: {0}";


	/// <summary>
	/// Creates a notification and puts data in the log panel. Text(presets are above) is passed along with optional variables and a boolean if the notification should be permanent.
	/// </summary>
	public GameObject AddToLog(string text, object[] variables = null, bool permanent = false)
	{
		GameObject notification = Instantiate(Resources.Load("Notification") as GameObject, InGameUIManager.s_Singleton.LogNotifications, false);
		notification.GetComponent<Notification>().SetText(string.Format(text, variables));
		notification.GetComponent<Notification>().SetPermanent(permanent);

		// Forces the VerticalLayoutGroup to update with the new notification.
		LayoutRebuilder.ForceRebuildLayoutImmediate(InGameUIManager.s_Singleton.LogNotifications);

		GameObject logItem = Instantiate(Resources.Load("LogItem") as GameObject, m_LogScrollViewContent, false);
		Text textLog = logItem.transform.GetChild(0).GetComponent<Text>();
		textLog.text = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ": " + string.Format(text, variables);
		if(logItem.transform.GetSiblingIndex() % 2 == 0) {
			logItem.GetComponent<Image>().color = new Color(.95f, .95f, .95f);
		}
		else {
			logItem.GetComponent<Image>().color = new Color(.9f, .9f, .9f);
		}

		return notification;
	}

	/// <summary>
	/// Event for pressing the logs button. Show the log of the game.
	/// </summary>
	public void ToggleLogsPanel(bool newState)
	{
		for(int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(newState);
		}
	}
}
