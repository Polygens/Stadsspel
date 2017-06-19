using System;
using System.Collections.Generic;

[Serializable]
public class WinningTeamMessage
{
	public List<TeamScore> scoreList;

	[Serializable]
	public class TeamScore
	{
		public string name;
		public double score;

		public TeamScore()
		{
		}

		public TeamScore(string name, double score)
		{
			this.name = name;
			this.score = score;
		}

		public string getName()
		{
			return name;
		}

		public void setName(string name)
		{
			this.name = name;
		}

		public double getScore()
		{
			return score;
		}

		public void setScore(double score)
		{
			this.score = score;
		}
	}

}
