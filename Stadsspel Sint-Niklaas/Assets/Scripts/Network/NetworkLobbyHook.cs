using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
	{
		LobbyPlayer lobbyP = lobbyPlayer.GetComponent<LobbyPlayer>();
		gamePlayer.GetComponent<Renderer>().material.color = TeamData.GetColor(lobbyP.mPlayerTeam);
		gamePlayer.transform.GetChild(0).GetComponent<TextMesh>().text = lobbyP.mPlayerName;

		gamePlayer.transform.SetParent(lobbyPlayer.transform);
	}
}
