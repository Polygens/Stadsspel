using UnityEngine;
using System;
using UnityEngine.Networking;

namespace Stadsspel.Elements
{
	public class Bank : Building
	{
		private new void Start()
		{
			mBuildingType = BuildingType.Bank;
			base.Start();
		}
	}
}
