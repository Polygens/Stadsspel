using UnityEngine;

public class LogUI : MonoBehaviour
{

	public void ToggleLogsPanel(bool newState)
	{
		for(int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(newState);
		}
	}
}
