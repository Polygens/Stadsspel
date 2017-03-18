using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobStatus : MonoBehaviour {

  private bool recentlyGotRobbed = false;
  public bool RecentlyGotRobbed
  {
    get
    {
      return recentlyGotRobbed;
    }
    set
    {
      recentlyGotRobbed = value;
    }
  }
}
