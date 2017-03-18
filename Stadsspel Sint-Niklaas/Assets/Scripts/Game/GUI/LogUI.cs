﻿using UnityEngine;
using UnityEngine.UI;


public class LogUI : MonoBehaviour
{
	[SerializeField]
	private RectTransform m_LogScrollViewContent;

	public const string m_HasDepositedMoneyInBank = "";
	public const string m_Capturing = "CAPTURING";
	public const string m_WasCaptured = "{0} is nu van {1}";


	public GameObject AddToLog(string text, object[] variables = null, bool permanent = false)
	{
		GameObject notification = Instantiate(Resources.Load("Notification") as GameObject, InGameUIManager.s_Singleton.LogNotifications, false);
		notification.GetComponent<Notification>().SetText(string.Format(text, variables));
		notification.GetComponent<Notification>().SetPermanent(permanent);

		LayoutRebuilder.ForceRebuildLayoutImmediate(InGameUIManager.s_Singleton.LogNotifications);

		GameObject logItem = Instantiate(Resources.Load("LogItem") as GameObject, m_LogScrollViewContent, false);
		Text textLog = logItem.transform.GetChild(0).GetComponent<Text>();
		textLog.text = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ": " + string.Format(text, variables);
		if(logItem.transform.GetSiblingIndex() % 2 == 0) {
			logItem.GetComponent<Image>().color = new Color(.95f, .95f, .95f);
		} else {
			logItem.GetComponent<Image>().color = new Color(.9f, .9f, .9f);
		}

		return notification;
	}

	public void ToggleLogsPanel(bool newState)
	{
		for(int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(newState);
		}
	}
}