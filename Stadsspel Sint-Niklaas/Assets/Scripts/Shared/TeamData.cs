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
	private static Color mNotSet = new Color(1, 0, 1);

	private static Color[] mTeamColors = new Color[] {
		Color.magenta,	//Team1
		Color.red,		//Team2
		Color.cyan,		//Team3
		Color.blue,		//Team4
		Color.green,	//Team5
		Color.yellow,	//Team6
		Color.grey		//NoTeam
	};

	private static Color[] mDistrictColors = new Color[] {
		new Color(0, 0, 0, 128 / 255f),						//HeadDistrict
		new Color(0, 0, 0, 128 / 255f),						//Neutral
		new Color(245 / 255f, 132 / 255f, 42 / 255f, 128 / 255f),	//GrandMarket
		new Color(0, 0, 0, 20 / 255f),						//Capturable
		new Color(125 / 255f, 0, 0, 255 / 255f),					//Outside
		new Color(255 / 255f, 128 / 255f, 0, 255 / 255f)				//Square
	};

	private static Color[] mBuildingColors = new Color[] {
		new Color(0, 159 / 255f, 227 / 255f),		//Bank
		new Color(245 / 255f, 132 / 255f, 42 / 255f)	//TradingPost
	};

	private static int[] mMaxTeams = {
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
	private static int[] mMaxPlayers = {
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

	public static Color GetColor(TeamID team)
	{
		if(team == TeamID.NotSet) {
			return mNotSet;
		}

		return mTeamColors[(int)team - 1];
	}

	public static Color GetColor(DistrictType type)
	{
		if(type == DistrictType.NotSet) {
			return mNotSet;
		}

		return mDistrictColors[(int)type - 1];
	}

	public static Color GetColor(BuildingType type)
	{
		if(type == BuildingType.NotSet) {
			return mNotSet;
		}

		return mBuildingColors[(int)type - 1];
	}

	public static TeamID GetNextTeam(TeamID team)
	{
		int newTeamID = (int)team;
		newTeamID %= TeamData.GetMaxTeams(PhotonNetwork.room.MaxPlayers);
		newTeamID++;
		return (TeamID)newTeamID;
	}

	public static TeamID GetPreviousTeam(TeamID team)
	{
		int newTeamID = (int)team - 2;
		if(newTeamID < 0) {
			newTeamID = TeamData.GetMaxTeams(PhotonNetwork.room.MaxPlayers);
		}
		return (TeamID)newTeamID;
	}

	public static int GetMaxPlayersPerTeam(int totalPlayers)
	{
		return mMaxPlayers[totalPlayers - 6];
	}

	public static int GetMaxTeams(int totalPlayers)
	{
		return mMaxTeams[totalPlayers - 6];
	}
}
