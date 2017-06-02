using System;
using System.Collections.Generic;

[Serializable]
public class ServerTradePost : PointLocation
{

	[Serializable]
	public class ServerItem
	{
		public string name;
		public double legalSales;
		public double illegalSales;
		public double legalPurchase;
		public double illegalPurchase;
	}

	public List<ServerItem> items;

}

