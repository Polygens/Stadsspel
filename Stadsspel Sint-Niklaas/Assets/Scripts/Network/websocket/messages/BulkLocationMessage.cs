using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class BulkLocationMessage : ISerializationCallbackReceiver
{
	private const int DEFAULT_ALTITUDE = 1;
	[Serializable]
	public class BulkLocation
	{
		[Serializable]
		public class BulkPoint
		{
			public double lat;
			public double lng;
		}

		public string key;
		public BulkPoint location;
		public bool taggable;
	}

	public BulkLocation[] locations;
	public IDictionary<string, Point> Taggable { get; private set; }
	public IDictionary<string, Point> Locations { get; private set; }

	//todo check for need of special JSON parse
	public void OnBeforeSerialize()
	{
		throw new NotSupportedException();
	}

	public void OnAfterDeserialize()
	{
		Taggable = new Dictionary<string, Point>();
		Locations = new Dictionary<string, Point>();

		foreach (BulkLocation bulkLocation in locations)
		{
			if (bulkLocation.taggable)
			{
				Taggable.Add(bulkLocation.key, new Point(new Coordinates(bulkLocation.location.lat, bulkLocation.location.lng, DEFAULT_ALTITUDE)));
			} else
			{
				Locations.Add(bulkLocation.key, new Point(new Coordinates(bulkLocation.location.lat, bulkLocation.location.lng, DEFAULT_ALTITUDE)));
			}
		}
	}
}
