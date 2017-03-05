using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    static public GameManager s_Singleton;

    [SerializeField]
    private DistrictManager mDistrictManager;

    [SerializeField]
    private GameObject mteamPrefab;

    private float mGameLength;
    private bool mGameIsRunning = false;
    private float nextMoneyUpdateTime;
    private float moneyUpdateTimeInterval = 5;

    private Team[] mTeams;

    private Player mPlayer;

    private List<Treasure> mTreasures;

    public Player Player {
        get {
            return mPlayer;
        }

        set {
            mPlayer = value;
        }
    }

    public Team[] Teams {
        get {
            return mTeams;
        }

        set {
            mTeams = value;
        }
    }

	public DistrictManager DistrictManager {
		get {
			return mDistrictManager;
		}

		set {
			mDistrictManager = value;
		}
	}

	//public GameObject lobbyServerEntry;

	// Use this for initialization
	private void Start()
	{
		if (s_Singleton != null) {
			Destroy(this);
			return;
		}
		s_Singleton = this;
        mTreasures = new List<Treasure>();
		nextMoneyUpdateTime = moneyUpdateTimeInterval;
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	private void Update()
	{
		if (mGameIsRunning) {
			if (Time.timeSinceLevelLoad > mGameLength) {

				//Debug.Log("The game has ended");
			}
			if (Time.timeSinceLevelLoad > nextMoneyUpdateTime) {

				// Call GainMoneyOverTime() from each financial object
                for (int i = 0; i < mTreasures.Count; i++)
                {
                    mTreasures[i].GainMoneyOverTime();
                }
				nextMoneyUpdateTime = Time.timeSinceLevelLoad + moneyUpdateTimeInterval;
			}
		}
	}

	public void StartGame(int amountOfTeams)
	{
        Debug.Log("Gamemanager.StartGame(" + amountOfTeams + ")");
        CmdCreateTeams(amountOfTeams);
    }
    
	[Command]
	public void CmdCreateTeams(int amountOfTeams)
	{
        Debug.Log("Command Create Teams");
		for (int i = 0; i < amountOfTeams; i++) {
			GameObject temp = Instantiate(mteamPrefab);

			NetworkServer.Spawn(temp);
		}
		RpcClientsStart(amountOfTeams);
	}

	[ClientRpc]
	private void RpcClientsStart(int amountOfTeams)
	{
        Debug.Log("RPC client start");
		mDistrictManager = GameObject.FindWithTag("Districts").GetComponent<DistrictManager>();
		mDistrictManager.StartGame(amountOfTeams);
		mGameIsRunning = true;

		Teams = new Team[amountOfTeams];
		for (int i = 0; i < amountOfTeams; i++) {
			Teams[i] = transform.GetChild(i).gameObject.GetComponent<Team>();
		}
	}

	public void UpdateGameDuration(float duration)
	{
		Debug.Log(duration);
		mGameLength = duration;
	}

    public void AddTreasure(Treasure t)
    {
        mTreasures.Add(t);
    }

    public Treasure GetTreasureFrom(TeamID id)
    {
        return mTreasures[(int)id];
    }
}
