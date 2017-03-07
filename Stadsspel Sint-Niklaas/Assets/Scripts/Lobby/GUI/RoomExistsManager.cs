using UnityEngine;
using UnityEngine.UI;

public class RoomExistsManager : MonoBehaviour
{
	[SerializeField]
	private Button m_QuitBtn;

	public void EnableDisableMenu(bool newState)
	{
		gameObject.SetActive(newState);
	}

	// Use this for initialization
	void Start()
	{
		m_QuitBtn.onClick.AddListener(() => {
			EnableDisableMenu(false);
		});
	}
}
