
using System;
using System.Collections.Generic;

[Serializable]
public class ServerTeam
{
	public string teamName;
	public List<ServerPlayer> players;
	public double treasury;
	public double bankAccount;
	public string customColor;
	public List<string> districts;
	public List<string> tradeposts;

	public bool ContainsPlayer(string clientId)
	{
		foreach (ServerPlayer serverPlayer in players)
		{
			if (serverPlayer.clientID.Equals(clientId))
			{
				return true;
			}
		}
		return false;
	}
}