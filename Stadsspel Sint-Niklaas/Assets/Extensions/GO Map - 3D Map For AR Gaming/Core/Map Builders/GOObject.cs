using GoMap;
using UnityEngine;

public class GOObject : MonoBehaviour
{

	public GOMap map;
	public Coordinates coordinatesGPS;

    public static GOObject AddComponentToObject(GameObject container, GOMap map, Coordinates location)
    {
        container.SetActive(false);
        GOObject obj = container.AddComponent<GOObject>() as GOObject;
        // Variable initialization goes here - 
        // All of this will be called before Player's awake because
        // the object holding the Player component is currently not active.
        obj.map = map;
        obj.coordinatesGPS = location;
        container.SetActive(true);
        return obj;
    }

    // Use this for initialization
	void Awake()
	{

		if (map == null) {
			Debug.LogWarning("GOObject - Map property not set");
			return;
		}

		//register this class for location notifications
		map.locationManager.onOriginSet += LoadData;

	}

	void LoadData(Coordinates currentLocation)
	{//This is called when the origin is set

		map.dropPin(coordinatesGPS.latitude, coordinatesGPS.longitude, gameObject);

	}

}
