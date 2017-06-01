
using System;
using System.Collections.Generic;

[Serializable]
public class ServerTeam
{
	public string teamName;
	public Dictionary<string, ServerPlayer> players;
	public double treasury;
	public double bankAccount;
	public string customColor;


	public string TeamName
	{
		get { return teamName; }
		set { teamName = value; }
	}

	public Dictionary<string, ServerPlayer> Players
	{
		get { return players; }
		set { players = value; }
	}

	public double Treasury
	{
		get { return treasury; }
		set { treasury = value; }
	}

	public double BankAccount
	{
		get { return bankAccount; }
		set { bankAccount = value; }
	}

	public string CustomColor
	{
		get { return customColor; }
		set { customColor = value; }
	}

	public bool ContainsPlayer(string clientId)
	{
		return players.ContainsKey(clientId);
	}
}