using UnityEngine;
using UnityEngine.SceneManagement;

public class UserPanelUI : MonoBehaviour
{

	/// <summary>
	/// Event for pressing the leave game button. Leaves the current game.
	/// </summary>
	public void LeaveGame()
	{
		//PhotonNetwork.Disconnect();
		//todo leave game properly?
		SceneManager.LoadScene("Lobby");
	}
}
