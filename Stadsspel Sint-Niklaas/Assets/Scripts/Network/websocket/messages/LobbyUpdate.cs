using System;
using System.Collections.Generic;

[Serializable]
public class LobbyUpdate
{
	public List<ServerTeam> teams;
	public bool isHost;
}
