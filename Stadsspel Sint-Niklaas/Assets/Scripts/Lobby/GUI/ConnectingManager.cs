using Stadsspel.Networking;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingManager : MonoBehaviour
{
	[SerializeField]
	private Button m_CancelConnectingBtn;

	/// <summary>
	/// Initialises the class.
	/// </summary>
	private void Start()
	{
		m_CancelConnectingBtn.onClick.AddListener(() => {
			//PhotonNetwork.LeaveLobby();
			//PhotonNetwork.JoinLobby(TypedLobby.Default);
			NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
			NetworkManager.Singleton.RoomManager.EnableDisableMenu(false);
			EnableDisableMenu(false);
		});
	}

	/// <summary>
	/// Generic function for handling switching between the different menus.
	/// </summary>
	public void EnableDisableMenu(bool newState)
	{
		gameObject.SetActive(newState);
	}
}
