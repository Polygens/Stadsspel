using System;
using Assets.scripts.dom;

[Serializable]
public class Point
{
	public double latitude;
	public double longitude;
	public string type;
	public int index;

	public PointType Type {
		get {
			return (PointType)Enum.Parse(typeof(PointType), type);
		}
	}

	public PointType getType()
	{
		return (PointType)Enum.Parse(typeof(PointType), type);
	}

	public Point(Coordinates coordinates, PointType type = PointType.COORDINATE)
	{
		latitude = coordinates.latitude;
		longitude = coordinates.longitude;
		this.type = type.ToString();
	}

	public Point()
	{
	}
}
