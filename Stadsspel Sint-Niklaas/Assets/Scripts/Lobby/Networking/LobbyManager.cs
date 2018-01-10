using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace Stadsspel.Networking
{
	public class LobbyManager : MonoBehaviour
	{
		[SerializeField] private RectTransform _mRoomList;
		[SerializeField] public RectTransform _mNoServerFound;

		public void RegisterToGame(string gameId, string password)
		{
			Debug.Log("Registering to my game: " + gameId);
			var game = JsonUtility.FromJson<CurrentGame.Game>(Rest.GetGameById(gameId));
			var room = Instantiate(Resources.Load("Room") as GameObject);
			Debug.Log("2nd id: "+game.id);
			room.GetComponent<Room>().InitializeRoom(game.roomName, game.id, 0, 0, password.Length > 0);
			room.GetComponent<Room>().ClickJoinRoom();
		}
		
		public void EnableDisableMenu(bool newState)
		{
			gameObject.SetActive(newState);

			if (!newState) return;

			// adds back-button in the top-panel to leave lobby
			NetworkManager.Singleton.TopPanelManager.EnableDisableButton(true, () =>
			{
				//Rest.UnregisterPlayer(CurrentGame.Instance.LocalPlayer.clientID, CurrentGame.Instance.GameId);
				EnableDisableMenu(false);
				NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
			});

			FixedUpdateRooms();
		}

		public void EnableDisableMenu(bool newState, string name)
		{
			gameObject.SetActive(newState);

			if (!newState) return;

			// adds back-button in the top-panel to leave lobby
			NetworkManager.Singleton.TopPanelManager.EnableDisableButton(true, name, () =>
			{
				//Rest.UnregisterPlayer(CurrentGame.Instance.LocalPlayer.clientID, CurrentGame.Instance.GameId);
				EnableDisableMenu(false, name);
				NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true, name);
			});

			FixedUpdateRooms();
		}



		/// <summary>
		/// Updates the rooms in the rooms list UI. Removes all rooms first and then adds all existing rooms again. If no rooms are found a message is shown.
		/// </summary>
		public void UpdateRooms()
		{
			var rooms = Rest.GetStagedGames();

			if (rooms.Count == 0)
			{
				Debug.Log("dsfpok");
				_mNoServerFound.gameObject.SetActive(true);
			}
			else
			{
				_mNoServerFound.gameObject.SetActive(false);
			}

			Room[] currentRooms = _mRoomList.GetComponentsInChildren<Room>();
			List<Room> tempRooms = new List<Room>();

			// Add all the matching rooms to a temporary List
			if (currentRooms.Length > 0)
			{
				foreach (var room in rooms)
				{
					for (var i = currentRooms.Length -1; i >= 0; i--)
					{
						if (room.name == currentRooms[i].RoomName)
						{
							tempRooms.Add(currentRooms[i]);
						}
					}
				}
			}

			//Delete all the rooms that did not match
			for (var i = currentRooms.Length - 1; i >= 0; i--)
			{
				if (!tempRooms.Contains(currentRooms[i])){
					Destroy(currentRooms[i].gameObject);
					currentRooms[i] = null;
				}
			}

			//Remove all rooms that allready exist from staged rooms list.
			List<GameListResource> roomResources = rooms;
			for (var i = roomResources.Count - 1; i >= 0; i--)
			{
				for (int j = 0; j < tempRooms.Count; j++)
				{
					if (roomResources[i].name == tempRooms[j].RoomName)
					{
						roomResources.RemoveAt(i);
						break;
					}
				}
			}

			foreach (var resource in roomResources)
			{
				//todo filter full rooms

				var room = Instantiate(Resources.Load("Room") as GameObject);
				room.transform.SetParent(_mRoomList, false);

				//todo expand gameListResource to have more data?
				room.GetComponent<Room>().InitializeRoom(resource.name, resource.id, resource.players, resource.maxPlayers, resource.hasPassword);
			}
		}

		private void FixedUpdateRooms()
		{
			UpdateRooms();
			Invoke("FixedUpdateRooms", 5f);
		}
	}
}
