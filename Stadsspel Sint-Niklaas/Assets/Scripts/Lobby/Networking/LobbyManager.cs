using Photon;
using UnityEngine;
using UnityEngine.Events;

namespace Stadsspel.Networking
{
	public class LobbyManager : PunBehaviour
	{
		[SerializeField]
		RectTransform m_RoomList;
		[SerializeField]
		RectTransform m_NoServerFound;

		/// <summary>
		/// Generic function for handling switching between the different menus.
		/// </summary>
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

		/// <summary>
		/// Gets called every frame.
		/// </summary>
		public void Update()
		{
			if(m_RoomList.childCount != PhotonNetwork.GetRoomList().Length) {
				UpdateRooms();
			}
		}

		/// <summary>
		/// [PunBehaviour] Gets called when the room list has changed.
		/// </summary>
		public override void OnReceivedRoomListUpdate()
		{
			base.OnReceivedRoomListUpdate();
			UpdateRooms();
		}

		/// <summary>
		/// Updates the rooms in the rooms list UI. Removes all rooms first and then adds all existing rooms again. If no rooms are found a message is shown.
		/// </summary>
		public void UpdateRooms()
		{
			int children = m_RoomList.childCount;
			for(int i = children - 1; i >= 0; i--) {
				GameObject.Destroy(m_RoomList.GetChild(i).gameObject);
			}
			RoomInfo[] rooms = PhotonNetwork.GetRoomList();
			if(rooms.Length == 0) {
				m_NoServerFound.gameObject.SetActive(true);
			}
			else {
				m_NoServerFound.gameObject.SetActive(false);
			}
			for(int i = 0; i < rooms.Length; i++) {
				if(rooms[i].PlayerCount != rooms[i].MaxPlayers) {
					GameObject room = Instantiate(Resources.Load("Room") as GameObject);
					room.transform.SetParent(m_RoomList, false);

					string password = rooms[i].CustomProperties[RoomManager.RoomPasswordProp] as string;
					room.GetComponent<Room>().InitializeRoom(rooms[i].Name, rooms[i].PlayerCount, rooms[i].MaxPlayers, password != "" ? password : "");
				}
			}
		}
	}
}
