using UnityEngine;

public enum TeamID:byte
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
		Color.magenta,	//Team1
		Color.red,		//Team2
		Color.cyan,		//Team3
		Color.blue,		//Team4
		Color.green,		//Team5
		Color.yellow,			//Team6
		new Color(0, 0, 0, 128),	//Neutral
		new Color(0, 0, 100),		//GrandMarket
		new Color(0, 0, 0, 20),		//Capturable
		new Color(0.5f,0,0,1),					//Outside
		new Color(1,0.5f,0,1)				//Square
	};

	public static Color GetColor(TeamID team, DistrictType type = DistrictType.NotSet)
	{
		if (type == DistrictType.square) {
			return mTeamColors[11];
		}

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
			}
		}
		else {
			return mTeamColors[(int)team];
		}
	}
}

