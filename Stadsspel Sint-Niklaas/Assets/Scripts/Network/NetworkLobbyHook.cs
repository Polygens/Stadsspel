using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
	{
    LobbyPlayer lobbyP = lobbyPlayer.GetComponent<LobbyPlayer>();
		gamePlayer.GetComponent<PlayerInitializer>().Team = lobbyP.mPlayerTeam;
		gamePlayer.GetComponent<PlayerInitializer>().Name = lobbyP.mPlayerName;
	}
}
