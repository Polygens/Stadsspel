using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Domain
{
	public class LocalPlayer : ServerPlayer
	{
		public IDictionary<string, int> legalItems;
		public IDictionary<string, int> illegalItems;
		public string token;
		public List<string> taggedByTeams;
		public Point location;
		public double money;
	}
}
