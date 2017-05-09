public class AreaLocation {
  private string id;
  private Point[] points;
  private AreaType type;
  private string name;

  public enum AreaType {
    //SAFE_ZONE,
    DISTRICT_A, DISTRICT_B, MARKET
  }
}
