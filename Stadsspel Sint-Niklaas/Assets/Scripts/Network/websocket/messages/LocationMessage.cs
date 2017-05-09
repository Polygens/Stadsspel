using System;

[Serializable]
public class LocationMessage {
  public double latitude;
  public double longitude;

  public LocationMessage(double latitude, double longitude) {
    this.latitude = latitude;
    this.longitude = longitude;
  }
}
