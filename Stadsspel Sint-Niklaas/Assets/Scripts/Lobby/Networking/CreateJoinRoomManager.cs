using UnityEngine;
using UnityEngine.UI;

namespace Stadsspel.Networking
{
	public class CreateJoinRoomManager : MonoBehaviour
	{
		[SerializeField] private InputField _mRoomNameInp;
		[SerializeField] private InputField _mRoomPasswordInp;
		[SerializeField] private Dropdown _mRoomGameDurationDro;
		[SerializeField] private Slider _mRoomAmountOfPlayersSli;
		[SerializeField] private Text _mAmountOfPlayersTxt;


		public void Awake()
		{
			var network = new GameObject("Network");
			network.AddComponent(typeof(WebsocketImpl));
			network.AddComponent(typeof(CurrentGame));
			DontDestroyOnLoad(network);
		}

		public void EnableDisableMenu(bool newState)
		{
			gameObject.SetActive(newState);

			if (!newState) return;

			NetworkManager.Singleton.TopPanelManager.EnableDisableButton(false);
			NetworkManager.Singleton.TopPanelManager.SetName("");
		}

		public void UpdateAmountOfPlayersUI(float numPlayers)
		{
			_mAmountOfPlayersTxt.text = "AANTAL SPELERS: " + numPlayers;
		}

		public void MakeRoom()
		{
			
			CurrentGame.Instance.HostingLoginToken = Rest.DeviceLogin(CurrentGame.Instance.LocalPlayer.clientID);
			
			Debug.Log("hostinglogintoken: " + CurrentGame.Instance.HostingLoginToken);
			Debug.Log("roomname: " + _mRoomNameInp.text);
			Debug.Log("password: " + _mRoomPasswordInp.text);
			
			var gameId = Rest.NewGame(new GameResource(CurrentGame.Instance.HostingLoginToken, _mRoomNameInp.text, 2, 6, _mRoomPasswordInp.text));			
			CurrentGame.Instance.HostedGameId = gameId;
			
			Debug.Log("gameid: " + gameId);
			
			EnableDisableMenu(false);
			NetworkManager.Singleton.LobbyManager.RegisterToGame(gameId, _mRoomPasswordInp.text);
		}

		public void ShowLobby()
		{
			NetworkManager.Singleton.LobbyManager.EnableDisableMenu(true);
			EnableDisableMenu(false);
		}
	}
}
