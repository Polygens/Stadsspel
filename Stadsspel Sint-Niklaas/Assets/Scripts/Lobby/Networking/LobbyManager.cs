using UnityEngine;
using Photon;
using UnityEngine.Events;

namespace Stadsspel.Networking
{
	public class LobbyManager : PunBehaviour
	{
		[SerializeField]
		RectTransform m_RoomList;
		[SerializeField]
		RectTransform m_NoServerFound;

		public void EnableDisableMenu(bool newState)
		{
			gameObject.SetActive(newState);
			if(newState) {
				NetworkManager.Singleton.TopPanelManager.EnableDisableButton(true, new UnityAction(() => {
					EnableDisableMenu(false);
					NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
				}));
			}
		}

		public void Update()
		{
			if(m_RoomList.childCount != PhotonNetwork.GetRoomList().Length) {
				UpdateRooms();
			}
		}

		public override void OnReceivedRoomListUpdate()
		{
			base.OnReceivedRoomListUpdate();
			UpdateRooms();
		}

		public void UpdateRooms()
		{
			int children = m_RoomList.childCount;
			for(int i = children - 1; i >= 0; i--) {
				GameObject.Destroy(m_RoomList.GetChild(i).gameObject);
			}
			RoomInfo[] rooms = PhotonNetwork.GetRoomList();
			if(rooms.Length == 0) {
				m_NoServerFound.gameObject.SetActive(true);
			} else {
				m_NoServerFound.gameObject.SetActive(false);
			}
			for(int i = 0; i < rooms.Length; i++) {
				GameObject room = Instantiate(Resources.Load("Room") as GameObject);
				room.transform.SetParent(m_RoomList, false);

				string password = rooms[i].CustomProperties["password"] as string;
				room.GetComponent<Room>().InitializeRoom(rooms[i].Name, rooms[i].PlayerCount, rooms[i].MaxPlayers, password != "" ? password : "");
			}
		}
	}
}
