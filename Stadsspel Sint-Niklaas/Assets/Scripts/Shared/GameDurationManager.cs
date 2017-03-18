using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDurationManager : MonoBehaviour
{
    public static GameDurationManager instance;
    private float gameDuration;


    public float GameDuration
    {
        get { return gameDuration; }
        set { gameDuration = value; }
    }
	
	void Awake ()
    {
        DontDestroyOnLoad(this);
        instance = this;
	}
	

}
