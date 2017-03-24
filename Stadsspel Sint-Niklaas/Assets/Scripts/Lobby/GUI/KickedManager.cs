using UnityEngine;
using UnityEngine.UI;

public class KickedManager : MonoBehaviour
{
	[SerializeField]
	private Button m_QuitPanelBtn;

	/// <summary>
	/// Generic function for handling switching between the different menus.
	/// </summary>
	public void EnableDisableMenu(bool newState)
	{
		gameObject.SetActive(newState);
	}

	/// <summary>
	/// Initialises the class.
	/// </summary>
	void Start()
	{
		m_QuitPanelBtn.onClick.AddListener(() => {
			EnableDisableMenu(false);
		});
	}
}
