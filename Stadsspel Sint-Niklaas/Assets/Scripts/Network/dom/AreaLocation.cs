using System;

[Serializable]
public class AreaLocation {
  public string id;
  public Point[] points;
  public AreaType type;
  public string name;

  public enum AreaType {
    //SAFE_ZONE,
    DISTRICT_A, DISTRICT_B, MARKET
  }
}
