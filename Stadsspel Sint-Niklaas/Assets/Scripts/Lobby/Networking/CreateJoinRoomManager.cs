using UnityEngine;
using UnityEngine.UI;

namespace Stadsspel.Networking
{
	public class CreateJoinRoomManager : MonoBehaviour
	{
		[SerializeField]
		private InputField m_RoomNameInp;
		[SerializeField]
		private InputField m_RoomPasswordInp;
		[SerializeField]
		private Dropdown m_RoomGameDurationDro;
		[SerializeField]
		private Slider m_RoomAmountOfPlayersSli;
		[SerializeField]
		private Text m_AmountOfPlayersTxt;


		public void EnableDisableMenu(bool newState)
		{
			gameObject.SetActive(newState);
			if(newState) {
				NetworkManager.Singleton.TopPanelManager.EnableDisableButton(false);
				NetworkManager.Singleton.TopPanelManager.SetName("");
			}
		}

		public void UpdateAmountOfPlayersUI(float numPlayers)
		{
			m_AmountOfPlayersTxt.text = "AANTAL SPELERS: " + numPlayers;
		}

		public void MakeRoom()
		{
			if(m_RoomNameInp.text != "") {
				RoomInfo[] rooms = PhotonNetwork.GetRoomList();
				foreach(RoomInfo room in rooms) {
					if(room.Name == m_RoomNameInp.text) {
						Debug.Log("Room creation failed");
						NetworkManager.Singleton.RoomExistsManager.EnableDisableMenu(true);
						return;
					}
				}
				int gameDuration = 50;
				NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(true);
				NetworkManager.Singleton.RoomManager.InitializeRoom(m_RoomNameInp.text, m_RoomPasswordInp.text, gameDuration, (byte)Mathf.Round(m_RoomAmountOfPlayersSli.value));
				gameObject.SetActive(false);
			} else {
				Debug.Log("ERROR: No name given for the room!");
			}
		}

		public void ShowLobby()
		{
			NetworkManager.Singleton.LobbyManager.EnableDisableMenu(true);
			EnableDisableMenu(false);
		}
	}
}