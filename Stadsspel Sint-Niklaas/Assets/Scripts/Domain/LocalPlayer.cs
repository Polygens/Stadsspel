using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Domain
{
	public class LocalPlayer : ServerPlayer
	{
		public Dictionary<string, int> legalItems;
		public Dictionary<string, int> illegalItems;
		public string token;
		public List<string> taggedByTeams;
		public Point location;
		public double money;
	}
}
