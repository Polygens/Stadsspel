using GoMap;
using Photon;
using Stadsspel.Districts;
using Stadsspel.Elements;
using Stadsspel.Networking;
using System.Collections.Generic;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

public class GameManager : MonoBehaviour
{
	static public GameManager s_Singleton;

	[SerializeField]
	private DistrictManager m_DistrictManager;

	[SerializeField]
	private LocationManager m_LocationManager;

	private const string m_TeamPrefabName = "Team";
	private const string m_PlayerPrefabName = "Player";

	private int m_AmountOfTeams;

	private float m_GameLength;
	private float m_NextMoneyUpdateTime;
	private const float m_MoneyUpdateTimeInterval = 30;

	private Team[] m_Teams;

	private Player m_Player;

	private List<Treasure> m_Treasures;

	private bool isGameOver = false;

	public Player Player {
		get {
			return m_Player;
		}
		set {
			if (m_Player == null)
			{
				m_Player = value;
			}
		}
	}

	public Team[] Teams {
		get {
			return m_Teams;
		}
	}

	public DistrictManager DistrictManager {
		get {
			return m_DistrictManager;
		}
	}

	public LocationManager LocationManager {
		get {
			return m_LocationManager;
		}
	}

	public float GameLength {
		get { return m_GameLength; }
	}

	private void Awake()
	{
		InitializeMapLocations();
	}


	/// <summary>
	/// Initialises the class.
	/// </summary>
	private void Start()
	{
		if (s_Singleton != null)
		{
			Destroy(this);
			return;
		}
		s_Singleton = this;

		//m_GameLength = (int)PhotonNetwork.room.CustomProperties[RoomManager.RoomGameDurationProp]; todo fix
		m_GameLength = 60;

		//m_AmountOfTeams = TeamData.GetMaxTeams(PhotonNetwork.room.MaxPlayers);todo fix
		m_AmountOfTeams = TeamData.GetMaxTeams(6);
		MasterClientStart();

		m_Treasures = new List<Treasure>();
		Debug.Log("INIT TREASURES");
		m_NextMoneyUpdateTime = m_MoneyUpdateTimeInterval;

#if (UNITY_EDITOR)
		Debug.Log("Game will take: " + m_GameLength + "seconds");
#endif
	}

	/// <summary>
	/// Gets called every frame.
	/// </summary>
	private void Update()
	{
		if (Time.timeSinceLevelLoad > m_GameLength && isGameOver == false)
		{
			var a = Time.timeSinceLevelLoad;

			isGameOver = true;
			InGameUIManager.s_Singleton.FinalScoreUI.gameObject.SetActive(true);
		}
		if (Time.timeSinceLevelLoad > m_NextMoneyUpdateTime && !isGameOver)
		{
			if (PhotonNetwork.isMasterClient)
			{
				// Call GainMoneyOverTime() from each financial object

				for (int i = 0; i < m_Treasures.Count; i++)
				{
					m_Treasures[i].photonView.RPC("RetrieveTaxes", PhotonTargets.All);
				}

			}
			m_NextMoneyUpdateTime = Time.timeSinceLevelLoad + m_MoneyUpdateTimeInterval;
		}
	}

	/// <summary>
	/// Starts the game for the masterclient. Calls starts for the other clients and sets up networked dynamic objects.
	/// </summary>
	private void MasterClientStart()
	{
#if (UNITY_EDITOR)
		Debug.Log("Master client started");
#endif

		//todo remove init of dummy data
		//CurrentGame.Instance.gameDetail = new CurrentGame.Game(m_AmountOfTeams);

		for (int i = 0; i < m_AmountOfTeams; i++)
		{
			//PhotonNetwork.InstantiateSceneObject(m_TeamPrefabName, Vector3.zero, Quaternion.identity, 0, null);
			Instantiate(Resources.Load(m_TeamPrefabName), Vector3.zero, Quaternion.identity);
		}
		//photonView.RPC("ClientsStart", PhotonTargets.All);
		ClientsStart();
	}

	/// <summary>
	/// [PunRPC] Starts the game for the clients, including the master client.
	/// </summary>
	[PunRPC]
	private void ClientsStart()
	{
#if (UNITY_EDITOR)
		Debug.Log("Clients start");
#endif
		m_DistrictManager.StartGame(m_AmountOfTeams);

		m_Teams = new Team[m_AmountOfTeams];
		for (int i = 0; i < m_AmountOfTeams; i++)
		{
			m_Teams[i] = transform.GetChild(i).gameObject.GetComponent<Team>();
		}

		//GameObject temp = PhotonNetwork.Instantiate(m_PlayerPrefabName, Vector3.zero, Quaternion.identity, 0);
		GameObject temp = (GameObject)Instantiate(Resources.Load(m_PlayerPrefabName), Vector3.zero, Quaternion.identity);
		Person person = temp.GetComponent<Person>();
		person.Player = CurrentGame.Instance.LocalPlayer;

		m_DistrictManager.SetPlayerTransform(temp.transform);
		temp.transform.position += new Vector3(0, 0, -10);
	}

	private void InitializeMapLocations()
	{
		List<PointLocation> banks = CurrentGame.Instance.gameDetail.banks;
		List<PointLocation> tradeposts = CurrentGame.Instance.gameDetail.tradePosts;
		List<AreaLocation> districts = CurrentGame.Instance.gameDetail.districts;
		List<AreaLocation> markets = CurrentGame.Instance.gameDetail.markets;
		//todo load all into map
		GameObject mapobj = GameObject.Find("Map");
		GameObject container = new GameObject("TESTEST");
		//GameObject container = GameObject.Find("MapElements");
		if (mapobj != null)
		{
			GOMap map = mapobj.GetComponent<GOMap>();
			if (map != null)
			{
				GameObject serverBanks = new GameObject("Server Banks");
				serverBanks.transform.parent = container.transform;
				GameObject serverTPs = new GameObject("Server Tradingposts");
				serverTPs.transform.parent = container.transform;
				foreach (PointLocation bank in banks)
				{
					GameObject temp = (GameObject)Instantiate(Resources.Load("Bank"), Vector3.zero, Quaternion.identity);
					temp.transform.parent = serverBanks.transform;
					Bank bankScript = temp.GetComponent<Bank>();
					if (bankScript != null)
					{
						bankScript.BankId = bank.id;
					}


					GOObject obj = GOObject.AddComponentToObject(temp, map,
						new Coordinates(bank.point.latitude, bank.point.longitude, 1.0));
					//map.dropPin(bank.point.latitude,bank.point.longitude,temp);
				}
				foreach (PointLocation tp in tradeposts)
				{
					GameObject temp = (GameObject)Instantiate(Resources.Load("Tradingpost"), Vector3.zero, Quaternion.identity);
					temp.transform.parent = serverTPs.transform;
					TradingPost tpScript = temp.GetComponent<TradingPost>();
					if (tpScript != null)
					{
						tpScript.TPId = tp.id;
					}


					GOObject obj = GOObject.AddComponentToObject(temp, map,
						new Coordinates(tp.point.latitude, tp.point.longitude, 1.0));
					//map.dropPin(tp.point.latitude, tp.point.longitude, temp);
				}

			} else
			{
				Debug.Log("ERROR: MAP NOT LOADED");
			}
		}
		else
		{
			Debug.Log("ERROR: MAP NOT LOADED");
		}
	}

	/// <summary>
	/// [PunRPC] Adds the passed Treasure to the global list.
	/// </summary>
	[PunRPC]
	public void AddTreasure(Treasure t)
	{
		Debug.Log("ADD TREASURE");
		Debug.Log(t);
		Debug.Log(m_Treasures);
		if (m_Treasures == null)
		{
			m_Treasures = new List<Treasure>();
			//todo g: I added this check but it shouldnt be needed?
		}
		m_Treasures.Add(t);
	}

	/// <summary>
	/// Returns the treasure of the given TeamID.
	/// </summary>
	public Treasure GetTreasureFrom(TeamID id)
	{
		/* todo make event for server
		for (int i = 0; i < m_Treasures.Count; i++)
		{
			if (m_Treasures[i].Team == id)
			{
				return m_Treasures[i];
			}
		}
		*/
		return null;
	}
}
