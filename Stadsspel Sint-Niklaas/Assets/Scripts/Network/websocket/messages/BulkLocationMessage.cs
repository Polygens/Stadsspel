using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class BulkLocationMessage
{
  public IDictionary<string, Point> locations;
  public IDictionary<string, Point> taggable;
  
  //todo check for need of special JSON parse
}
