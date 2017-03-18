using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
	[SerializeField]
	private Text m_Text;

	private float m_DestroyTime = 3;

	private void Update()
	{
		m_DestroyTime -= Time.deltaTime;
		if(m_DestroyTime < 0) {
			Destroy(gameObject);
		}
	}

	public void SetText(string text)
	{
		m_Text.text = text;
	}
}
