using UnityEngine;

public enum TeamID : byte
{
	NotSet,
	Team1,
	Team2,
	Team3,
	Team4,
	Team5,
	Team6,
	NoTeam
}

public class TeamData
{
	private static Color m_NotSet = new Color(1, 0, 1);

	private static Color[] m_TeamColors = new Color[] {
		Color.blue,	//Team1
		Color.red,		//Team2
		Color.cyan,		//Team3
		Color.magenta,		//Team4
		Color.green,	//Team5
		Color.yellow,	//Team6
		Color.grey		//NoTeam
	};

	private static Color[] mDistrictColors = new Color[] {
		new Color(0, 0, 0, 128 / 255f),						//Neutral
		new Color(245 / 255f, 132 / 255f, 42 / 255f, 200 / 255f),	//GrandMarket
		new Color(0, 0, 0, 128 / 255f),						//HeadDistrict
		new Color(128 / 255f, 128 / 255f, 128 / 255f, 50 / 255f),						//Capturable
		new Color(128 / 255f, 128 / 255f, 128 / 255f, 255 / 255f),			//Square
		new Color(200 / 255f, 0, 0, 255 / 255f)					//Outside

	};

	private static Color[] mBuildingColors = new Color[] {
		new Color(0, 159 / 255f, 227 / 255f),		//Bank
		new Color(245 / 255f, 132 / 255f, 42 / 255f)	//TradingPost
	};

	private static int[] mMaxTeams = {
		2,
		2,
		2,
		2,
		2,
		3,
		3,
		3,
		3,
		4,
		4,
		3,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		6,
		5,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6
	};
	// Max Players per team
	private static int[] mMaxPlayers = {
		2,
		3,
		3,
		4,
		4,
		3,
		4,
		4,
		4,
		4,
		4,
		5,
		4,
		5,
		5,
		5,
		5,
		6,
		6,
		6,
		4,
		5,
		5,
		5,
		5,
		5,
		5,
		6,
		6,
		6,
		6,
		6,
		6
	};

	/// <summary>
	/// Returns the color of the TeamID passed.
	/// </summary>
	public static Color GetColor(TeamID team)
	{
		if(team == TeamID.NotSet) {
			return m_NotSet;
		}

		return m_TeamColors[(int)team - 1];
	}

	/// <summary>
	/// Returns the color of the DistrictType passed.
	/// </summary>
	public static Color GetColor(Stadsspel.Districts.DistrictType type)
	{
		if(type == Stadsspel.Districts.DistrictType.NotSet) {
			return m_NotSet;
		}

		return mDistrictColors[(int)type - 1];
	}

	/// <summary>
	/// Returns the color of the BuildingType passed.
	/// </summary>
	public static Color GetColor(Stadsspel.Elements.BuildingType type)
	{
		if(type == Stadsspel.Elements.BuildingType.NotSet) {
			return m_NotSet;
		}

		return mBuildingColors[(int)type - 1];
	}

	/// <summary>
	/// Returns the next possible TeamID.
	/// </summary>
	public static TeamID GetNextTeam(TeamID team)
	{
		int newTeamID = (int)team;
		//newTeamID %= TeamData.GetMaxTeams(PhotonNetwork.room.MaxPlayers);
		newTeamID++;
		return (TeamID)newTeamID;
	}

	/// <summary>
	/// Returns the previous possible TeamID.
	/// </summary>
	public static TeamID GetPreviousTeam(TeamID team)
	{
		int newTeamID = (int)team - 2;
		if(newTeamID < 0) {
			//newTeamID = TeamData.GetMaxTeams(PhotonNetwork.room.MaxPlayers);
		}
		return (TeamID)newTeamID;
	}

	/// <summary>
	/// Returns the max amount of players per team, based of the max players of the room.
	/// </summary>
	public static int GetMaxPlayersPerTeam(int totalPlayers)
	{
		return mMaxPlayers[totalPlayers - 4]; 
	}

	/// <summary>
	/// Returns the max amount of teams, based of the max players of the room.
	/// </summary>
	public static int GetMaxTeams(int totalPlayers)
	{
		return mMaxTeams[totalPlayers - 4]; 
	}
}
