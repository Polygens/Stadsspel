
using System.Collections.Generic;

public class ServerTeam
{
	private string teamName;
	private Dictionary<string, ServerPlayer> players;
	private double treasury;
	private double bankAccount;
	private string customColor;


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