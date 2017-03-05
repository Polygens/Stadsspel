using UnityEngine;
using UnityEngine.UI;

public class KickedManager : MonoBehaviour
{
	[SerializeField]
	private Button m_QuitPanelBtn;

	public void EnableDisableMenu(bool newState)
	{
		gameObject.SetActive(newState);
	}

	// Use this for initialization
	void Start()
	{
		m_QuitPanelBtn.onClick.AddListener(() => {
			gameObject.SetActive(false);
		});
	}
}
