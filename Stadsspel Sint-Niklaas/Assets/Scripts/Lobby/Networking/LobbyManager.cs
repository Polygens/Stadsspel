using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace Stadsspel.Networking
{
	//todo allow manual refresh / auto refresh every 5 seconds
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
				Rest.UnregisterPlayer(CurrentGame.Instance.LocalPlayer.clientID, CurrentGame.Instance.GameId);
				EnableDisableMenu(false);
				NetworkManager.Singleton.CreateJoinRoomManager.EnableDisableMenu(true);
			});

			var games = Rest.GetStagedGames();
			UpdateRooms(games);
		}


		/// <summary>
		/// Updates the rooms in the rooms list UI. Removes all rooms first and then adds all existing rooms again. If no rooms are found a message is shown.
		/// </summary>
		public void UpdateRooms(List<GameListResource> rooms)
		{
			var children = _mRoomList.childCount;
			for (var i = children - 1; i >= 0; i--)
				Destroy(_mRoomList.GetChild(i).gameObject);

			_mNoServerFound.gameObject.SetActive(rooms.Count == 0);

			foreach (var resource in rooms)
			{
				//todo filter full rooms

				var room = Instantiate(Resources.Load("Room") as GameObject);
				room.transform.SetParent(_mRoomList, false);

				//todo expand gameListResource to have more data?
				room.GetComponent<Room>().InitializeRoom(resource.name, resource.id, 0, 0, resource.hasPassword);
			}
		}
	}
}
