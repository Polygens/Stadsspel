using System;
using UnityEngine;

[Serializable]
public class InfoNotification
{
	private GameEventType eGameEventType; //taged or robbed
	private bool typeInit = false;

	[SerializeField]
	private string gameEventType;
	public string by;
	public string locationId;

	public GameEventType GameEventType {
		get
		{
			if (!typeInit)
			{
				eGameEventType = (GameEventType)Enum.Parse(typeof(GameEventType), gameEventType);
			}
			return eGameEventType;
		}
	}

}