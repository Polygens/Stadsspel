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
						#if (UNITY_EDITOR)
						Debug.Log("Room creation failed");
						#endif
						NetworkManager.Singleton.RoomExistsManager.EnableDisableMenu(true);
						return;
					}
				}
				NetworkManager.Singleton.ConnectingManager.EnableDisableMenu(true);
				NetworkManager.Singleton.RoomManager.InitializeRoom(m_RoomNameInp.text, m_RoomPasswordInp.text, (int)GameDurationDropdown.m_Durations[m_RoomGameDurationDro.value].TotalSeconds, (byte)Mathf.Round(m_RoomAmountOfPlayersSli.value));
				gameObject.SetActive(false);
			} else {
				#if (UNITY_EDITOR)
				Debug.Log("ERROR: No name given for the room!");
				#endif
			}
		}

		public void ShowLobby()
		{
			NetworkManager.Singleton.LobbyManager.EnableDisableMenu(true);
			EnableDisableMenu(false);
		}
	}
}