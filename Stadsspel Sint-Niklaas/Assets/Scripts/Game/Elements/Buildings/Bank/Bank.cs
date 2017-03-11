using UnityEngine;
using System;

namespace Stadsspel.Elements
{
	public class Bank : Building
	{
		private new void Start()
		{
			m_BuildingType = BuildingType.Bank;
			base.Start();
		}
	}
}
