using UnityEngine;
using System;

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
