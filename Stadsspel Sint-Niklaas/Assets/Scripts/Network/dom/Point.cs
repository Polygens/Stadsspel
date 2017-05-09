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
}
