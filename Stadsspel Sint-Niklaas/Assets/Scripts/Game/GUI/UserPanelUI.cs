using UnityEngine;
using UnityEngine.SceneManagement;

public class UserPanelUI : MonoBehaviour
{

	public void LeaveGame()
	{
		PhotonNetwork.Disconnect();
		SceneManager.LoadScene("Lobby");
	}
}
