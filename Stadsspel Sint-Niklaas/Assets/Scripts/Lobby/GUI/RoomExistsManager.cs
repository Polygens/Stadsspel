using UnityEngine;
using UnityEngine.UI;

public class RoomExistsManager : MonoBehaviour
{
	[SerializeField]
	private Button m_QuitBtn;

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
		m_QuitBtn.onClick.AddListener(() => {
			EnableDisableMenu(false);
		});
	}
}
