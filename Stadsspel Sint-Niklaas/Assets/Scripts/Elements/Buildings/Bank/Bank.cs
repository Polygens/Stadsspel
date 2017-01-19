using UnityEngine;
using System;
using UnityEngine.Networking;

public class Bank : Building
{
	private new void Start()
	{
		mBuildingType = BuildingType.Bank;
		base.Start();
	}
}
