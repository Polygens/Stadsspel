using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
	private static Queue<string> notiQueue = new Queue<string>(0);
	private static bool Displaying = false;

	[SerializeField]
	private Text m_Text;

	private bool m_Permanent = false;

	private float m_DestroyTime = 3;

	private Material m_NotificationLocalMat;

	/// <summary>
	/// Gets called every frame. If not sticky a timer runs and when finished destroys itself.
	/// </summary>
	private void Update()
	{
		if(!m_Permanent) {
			m_DestroyTime -= Time.deltaTime;
			if(m_DestroyTime < 0) {
				Destroy(gameObject);
				Displaying = false;
				DisplayNextNotification();
			}
		}
	}

	/// <summary>
	/// Sets the text of the notification.
	/// </summary>
	public void SetText(string text)
	{
		m_Text.text = text;
	}

	/// <summary>
	/// Sets the default color of the notification and an optional end color if it is used a progressbar.
	/// </summary>
	public void SetColor(Color color, Color colorEnd = default(Color))
	{
		if(!m_NotificationLocalMat) {
			m_NotificationLocalMat = new Material(GetComponent<Image>().material);
			GetComponent<Image>().material = m_NotificationLocalMat;
		}
		m_NotificationLocalMat.SetColor("_Color", color);
		m_NotificationLocalMat.SetColor("_ProgressColor", colorEnd);
	}

	/// <summary>
	/// Set current progress of the progress bar notification.
	/// </summary>
	public void SetProgress(float progress)
	{
		if(!m_NotificationLocalMat) {
			m_NotificationLocalMat = new Material(GetComponent<Image>().material);
			GetComponent<Image>().material = m_NotificationLocalMat;
		}
		m_NotificationLocalMat.SetFloat("_Progress", progress);
	}

	/// <summary>
	/// Set the notification as a sticky permanent notification. Needs to be destroyed manually.
	/// </summary>
	public void SetPermanent(bool isPermanent)
	{
		m_Permanent = isPermanent;
	}


	public static void QueueNotification(string text)
	{
		notiQueue.Enqueue(text);
		DisplayNextNotification();
	}

	private static void DisplayNotification(string text)
	{
		Displaying = true;
		GameObject noti = Instantiate(Resources.Load("Notification") as GameObject, InGameUIManager.s_Singleton.LogNotifications, false);
		//GameObject noti = (GameObject)Instantiate(Resources.Load("Notification"), Vector3.zero, Quaternion.identity);
		Notification script = noti.GetComponent<Notification>();
		script.SetText(text);
		CurrentGame.FixZ(noti);
	}

	private static void DisplayNextNotification()
	{
		if (notiQueue.Count > 0 && !Displaying)
		{
			DisplayNotification(notiQueue.Dequeue());
		}
	}
}
