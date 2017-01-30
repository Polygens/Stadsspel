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

    private Treasure[] mTreasures;

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

    public Treasure[] Treasures
    {
        get
        {
            return mTreasures;
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
				nextMoneyUpdateTime = Time.timeSinceLevelLoad + moneyUpdateTimeInterval;
			}
		}
	}

	public void StartGame(int amountOfTeams)
	{
		CmdCreateTeams(amountOfTeams);
        FindTreasures();
	}

	[Command]
	public void CmdCreateTeams(int amountOfTeams)
	{
		for (int i = 0; i < amountOfTeams; i++) {
			GameObject temp = Instantiate(mteamPrefab);

			NetworkServer.Spawn(temp);
		}
		RpcClientsStart(amountOfTeams);
	}

	[ClientRpc]
	private void RpcClientsStart(int amountOfTeams)
	{
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

    private void FindTreasures()
    {
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Treasure");
        mTreasures = new Treasure[tempList.Length];
        
        for (int i = 0; i < mTreasures.Length; i++)
        {
            mTreasures[i] = tempList[i].GetComponent<Treasure>();
        }
        Debug.Log(mTreasures.Length + " Treasures found");
    }
}
