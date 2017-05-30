using System;
using Assets.scripts.dom;

[Serializable]
public class Point {
  public double latitude;
  public double longitude;
  private PointType eType;
  public string type;
  public int index;


  public PointType getType() {
    return (PointType)Enum.Parse(typeof(PointType), type);
  }

	public Point(Coordinates coordinates, PointType type = PointType.COORDINATE)
	{
		latitude = coordinates.latitude;
		longitude = coordinates.longitude;
		eType = type;
		this.type = eType.ToString();
	}

	public Point()
	{
	}
}
