using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;
using UnityEngine.Events;

namespace Stadsspel.Networking
{
	//todo allow manual refresh / auto refresh every 5 seconds
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
			if (newState)
			{
				NetworkManager.Singleton.TopPanelManager.EnableDisableButton(true, new UnityAction(() =>
				{
					EnableDisableMenu(false);
					NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
				}));
			}
		}


		public void Start()
		{ 
			List<GameListResource> games = Rest.GetStagedGames();
			UpdateRooms(games);
		}

		/// <summary>
		/// Gets called every frame.
		/// </summary>
		public void Update()
		{
			/* todo actually delete
			if(m_RoomList.childCount != PhotonNetwork.GetRoomList().Length) {
				UpdateRooms();
			}
			*/
		}

		/// <summary>
		/// [PunBehaviour] Gets called when the room list has changed.
		/// </summary>
		public override void OnReceivedRoomListUpdate()
		{
			/* todo actually delete
			base.OnReceivedRoomListUpdate();
			UpdateRooms();
			*/
		}

		/*todo ectually delete
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
		*/


		/// <summary>
		/// Updates the rooms in the rooms list UI. Removes all rooms first and then adds all existing rooms again. If no rooms are found a message is shown.
		/// </summary>
		public void UpdateRooms(List<GameListResource> rooms)
		{
			int children = m_RoomList.childCount;
			for (int i = children - 1; i >= 0; i--)
			{
				GameObject.Destroy(m_RoomList.GetChild(i).gameObject);
			}

			//RoomInfo[] rooms = PhotonNetwork.GetRoomList();

			if (rooms.Count == 0)
			{
				m_NoServerFound.gameObject.SetActive(true);
			}
			else
			{
				m_NoServerFound.gameObject.SetActive(false);
			}

			foreach (GameListResource resource in rooms)
			{
				//todo filter full rooms

				GameObject room = Instantiate(Resources.Load("Room") as GameObject);
				room.transform.SetParent(m_RoomList, false);

				//string password = rooms[i].CustomProperties[RoomManager.RoomPasswordProp] as string;
				//todo expand gameListResource to have more data?
				room.GetComponent<Room>().InitializeRoom(resource.name, 0, 16, "");
			}
		}
	}
}
