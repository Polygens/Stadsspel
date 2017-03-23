using System.Collections.Generic;
using UnityEngine;
using Photon;
using Stadsspel.Districts;
using Stadsspel.Elements;
using GoMap;
using Stadsspel.Networking;

public class GameManager : PunBehaviour
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
			if(m_Player == null) {
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

	//public GameObject lobbyServerEntry;

	// Use this for initialization
	private void Start()
	{
		if(s_Singleton != null) {
			Destroy(this);
			return;
		}
		s_Singleton = this;

		m_AmountOfTeams = TeamData.GetMaxTeams(PhotonNetwork.room.MaxPlayers);
		MasterClientStart();

		m_Treasures = new List<Treasure>();
		m_NextMoneyUpdateTime = m_MoneyUpdateTimeInterval;

		m_GameLength = (int)PhotonNetwork.room.CustomProperties[RoomManager.RoomGameDurationProp];
		#if (UNITY_EDITOR)
		Debug.Log("Game will take: " + m_GameLength + "seconds");
		#endif
	}

	// Update is called once per frame
	private void Update()
	{
		if(Time.timeSinceLevelLoad > m_GameLength && isGameOver == false) {
			isGameOver = true;
			//Debug.Log("GameLength: " + m_GameLength);
			//Debug.Log("Time since load: " + Time.timeSinceLevelLoad);
			InGameUIManager.s_Singleton.FinalScoreUI.gameObject.SetActive(true);
			//Debug.Log("The game has ended");
		}
		if(Time.timeSinceLevelLoad > m_NextMoneyUpdateTime && !isGameOver) {
			if(PhotonNetwork.isMasterClient) {
				// Call GainMoneyOverTime() from each financial object
			
				for(int i = 0; i < m_Treasures.Count; i++) {
					m_Treasures[i].photonView.RPC("GainMoneyOverTime", PhotonTargets.All);
				}
        
			}
			m_NextMoneyUpdateTime = Time.timeSinceLevelLoad + m_MoneyUpdateTimeInterval;
		}
	}

	private void MasterClientStart()
	{
		if(PhotonNetwork.player.IsMasterClient) {
			#if (UNITY_EDITOR)
			Debug.Log("Master client started");
			#endif
			for(int i = 0; i < m_AmountOfTeams; i++) {
				PhotonNetwork.InstantiateSceneObject(m_TeamPrefabName, Vector3.zero, Quaternion.identity, 0, null);
			}

			photonView.RPC("ClientsStart", PhotonTargets.All);
		}
	}

	[PunRPC]
	private void ClientsStart()
	{
		#if (UNITY_EDITOR)
		Debug.Log("Clients start");
		#endif
		m_DistrictManager.StartGame(m_AmountOfTeams);

		m_Teams = new Team[m_AmountOfTeams];
		for(int i = 0; i < m_AmountOfTeams; i++) {
			m_Teams[i] = transform.GetChild(i).gameObject.GetComponent<Team>();
		}

		GameObject temp = PhotonNetwork.Instantiate(m_PlayerPrefabName, Vector3.zero, Quaternion.identity, 0);
		m_DistrictManager.SetPlayerTransform(temp.transform);
		temp.transform.position += new Vector3(0, 0, -10);
	}

	public void UpdateGameDuration(float duration)
	{
		#if (UNITY_EDITOR)
		Debug.Log(duration);
		#endif
		m_GameLength = duration;
	}

	[PunRPC]
	public void AddTreasure(Treasure t)
	{
		m_Treasures.Add(t);
	}

	public Treasure GetTreasureFrom(TeamID id)
	{
		for(int i = 0; i < m_Treasures.Count; i++) {
			if(m_Treasures[i].Team == id) {
				return m_Treasures[i];
			}
		}
		return null;
	}
}
