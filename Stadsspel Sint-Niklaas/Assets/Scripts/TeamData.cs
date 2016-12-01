using UnityEngine;

public enum TeamID
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
	private static Color[] mTeamColors = new Color[] {
		new Color(1, 0, 1),		//NotSet
		new Color(.5f, .5f, .5f),	//Team1
		new Color(0, 0, 200),		//Team2
		new Color(200, 0, 0),		//Team3
		new Color(200, 200, 0),		//Team4
		new Color(0, 200, 0),		//Team5
		new Color(0, 0, 0),			//Team6
		new Color(0, 0, 0, 128),	//Neutral
		new Color(0, 0, 100),		//GrandMarket
		new Color(0, 0, 0, 20),		//Capturable
		Color.red,					//Outside
		Color.yellow				//Square
	};

	public static Color GetColor(TeamID team, DistrictType type = DistrictType.NotSet)
	{
		if (team == TeamID.NotSet || team == TeamID.NoTeam) {
			switch (type) {
				case DistrictType.NotSet:
				case DistrictType.HeadDistrict:
				default:
					return mTeamColors[0];
				case DistrictType.Neutral:
					return mTeamColors[7];
				case DistrictType.GrandMarket:
					return mTeamColors[8];
				case DistrictType.CapturableDistrict:
					return mTeamColors[9];
				case DistrictType.Outside:
					return mTeamColors[10];
				case DistrictType.square:
					return mTeamColors[11];
			}
		}
		else {
			return mTeamColors[(int)team];
		}
	}
}

