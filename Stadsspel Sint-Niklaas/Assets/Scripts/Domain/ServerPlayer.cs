using System;
using System.Collections.Generic;

[Serializable]
public class ServerPlayer
{
	public string name;
	public string clientID;

	public string Name
	{
		get { return name; }
		set { name = value; }
	}

	public string ClientId
	{
		get { return clientID; }
		set { clientID = value; }
	}
}
