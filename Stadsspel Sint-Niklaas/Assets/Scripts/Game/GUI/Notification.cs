using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
	[SerializeField]
	private Text m_Text;

	private bool m_Permanent = false;

	private float m_DestroyTime = 3;

	private Material m_NotificationLocalMat;

	private void Update()
	{
		if(!m_Permanent) {
			m_DestroyTime -= Time.deltaTime;
			if(m_DestroyTime < 0) {
				Destroy(gameObject);
			}
		}
	}

	/// <summary>
	/// TODO
	/// </summary>
	public void SetText(string text)
	{
		m_Text.text = text;
	}

	/// <summary>
	/// TODO
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
	/// TODO
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
	/// TODO
	/// </summary>
	public void SetPermanent(bool isPermanent)
	{
		m_Permanent = isPermanent;
	}
}
