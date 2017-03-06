using UnityEngine;
using UnityEngine.UI;
using Stadsspel.Networking;

public class ConnectingManager : MonoBehaviour
{
	[SerializeField]
	private Button m_CancelConnectingBtn;

	private void Start()
	{
		m_CancelConnectingBtn.onClick.AddListener(() => {
			
			PhotonNetwork.LeaveLobby();
			PhotonNetwork.JoinLobby(TypedLobby.Default);
			NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
			NetworkManager.Singleton.RoomManager.EnableDisableMenu(false);
			EnableDisableMenu(false);
		});
	}

	public void EnableDisableMenu(bool newState)
	{
		gameObject.SetActive(newState);
	}
}
