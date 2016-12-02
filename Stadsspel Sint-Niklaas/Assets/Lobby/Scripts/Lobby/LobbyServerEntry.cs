using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;

namespace Prototype.NetworkLobby
{
    public class LobbyServerEntry : MonoBehaviour 
    {
        public Text serverInfoText;
        public Text slotInfo;
        public Button joinButton;

        private MatchInfoSnapshot _match;
        private LobbyManager _lobbyManager;
        private string _matchName;

		public void Populate(MatchInfoSnapshot match, LobbyManager lobbyManager, Color c)
		{
            serverInfoText.text = match.name;

            slotInfo.text = match.currentSize.ToString() + "/" + match.maxSize.ToString(); ;

            _match = match;
            _lobbyManager = lobbyManager;
            _matchName = match.name;

            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener( () => lobbyManager.DisplayPasswordRequest(this) );
            //joinButton.onClick.AddListener(() => { JoinMatch(networkID, lobbyManager,match.name); });

            GetComponent<Image>().color = c;
        }

        public void JoinMatch(string password)
        {
            
			_lobbyManager.matchMaker.JoinMatch(_match.networkId, password, "", "", 0, 0, _lobbyManager.OnMatchJoined);
			_lobbyManager.backDelegate = _lobbyManager.StopClientClbk;
            _lobbyManager._isMatchmaking = true;
            _lobbyManager.DisplayIsConnecting();
      //lobbyManager.OnLobbyJoinUpdateName(matchName);
      _lobbyManager.SetLobbyNameToJoin(_matchName);
        }
    }
}