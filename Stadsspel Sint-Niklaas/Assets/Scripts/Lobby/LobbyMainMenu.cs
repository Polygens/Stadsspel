using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
	//Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
	public class LobbyMainMenu : MonoBehaviour
	{
		public LobbyManager lobbyManager;

		public RectTransform lobbyServerList;
		public RectTransform lobbyPanel;

		public InputField ipInput;
		public InputField matchNameInput;
		public InputField passwordInput;

		public void OnEnable()
		{
			lobbyManager.topPanel.ToggleVisibility(true);

			ipInput.onEndEdit.RemoveAllListeners();
			ipInput.onEndEdit.AddListener(onEndEditIP);

			matchNameInput.onEndEdit.RemoveAllListeners();
			matchNameInput.onEndEdit.AddListener(onEndEditGameName);

			passwordInput.onEndEdit.RemoveAllListeners();
			passwordInput.onEndEdit.AddListener(onEndEditPassword);
		}

		public void OnClickHost()
		{
			lobbyManager.StartHost();
		}

		public void OnClickJoin()
		{
			lobbyManager.ChangeTo(lobbyPanel);

			lobbyManager.networkAddress = ipInput.text;
			lobbyManager.StartClient();

			lobbyManager.backDelegate = lobbyManager.StopClientClbk;
			lobbyManager.DisplayIsConnecting();

		}

		public void OnClickDedicated()
		{
			lobbyManager.ChangeTo(null);
			lobbyManager.StartServer();

			lobbyManager.backDelegate = lobbyManager.StopServerClbk;
		}

		public void OnClickCreateMatchmakingGame()
		{
			lobbyManager.StartMatchMaker();
			lobbyManager.matchMaker.CreateMatch(
				matchNameInput.text,
				(uint)lobbyManager.maxPlayers,
				true,
					passwordInput.text, "", "", 0, 0,
					lobbyManager.OnMatchCreate);

			lobbyManager.backDelegate = lobbyManager.StopHost;
			lobbyManager.DisplayIsConnecting();

			//lobbyManager.OnLobbyJoinUpdateName(matchNameInput.text);
			lobbyManager.SetLobbyNameToJoin(matchNameInput.text);
		}

		public void OnClickOpenServerList()
		{
			lobbyManager.StartMatchMaker();
			lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
			lobbyManager.ChangeTo(lobbyServerList);
		}

		void onEndEditIP(string text)
		{
			if (Input.GetKeyDown(KeyCode.Return)) {
				OnClickJoin();
			}
		}

		void onEndEditGameName(string text)
		{
			if (Input.GetKeyDown(KeyCode.Return)) {
				OnClickCreateMatchmakingGame();
			}
		}

		void onEndEditPassword(string text)
		{
			if (Input.GetKeyDown(KeyCode.Return)) {
				OnClickCreateMatchmakingGame();
			}
		}

	}
}
