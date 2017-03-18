using System.Collections.Generic;
using UnityEngine;

namespace Stadsspel.Elements
{
	public class Person : Element
	{
		[SerializeField]
		private List<int> m_IllegalItems = new List<int>();

		//legalItems[(int)Items.diploma] = 10; Bijvoorbeeld
		[SerializeField]
		private List<int> m_LegalItems = new List<int>();

		//[SyncVar]
		[SerializeField]
		private int m_AmountOfMoney = 0;

    private void Awake()
		{
			m_Team = Stadsspel.Networking.TeamExtensions.GetTeam(photonView.owner);
			if(PhotonNetwork.player == photonView.owner) {
				gameObject.AddComponent<Player>();
				transform.GetComponentInChildren(typeof(MainSquareArrow), true).gameObject.SetActive(true);
			} else if(m_Team == Stadsspel.Networking.TeamExtensions.GetTeam(PhotonNetwork.player)) {
				gameObject.AddComponent<Friend>();
			} else {
				gameObject.AddComponent<Enemy>();
			}
		}

		protected new void Start()
		{
			GetComponent<MeshRenderer>().material.color = TeamData.GetColor(m_Team);
			transform.SetParent(GameManager.s_Singleton.Teams[(byte)m_Team - 1].transform, false);
			transform.GetChild(0).GetComponent<TextMesh>().text = photonView.owner.NickName;
			ActionRadius = 40;

			//instantiate list with 3 numbers for each list.

			for(int i = 0; i < 3; i++) {
				m_LegalItems.Add(0);
				m_IllegalItems.Add(0);
			}
		}

        public void Rob()
        {
            foreach (GameObject enemy in GameManager.s_Singleton.Player.EnemiesInRadius)
            {
                int enemyMoney = enemy.GetComponent<Person>().AmountOfMoney;
                AddGoods(enemyMoney, enemy.GetComponent<Person>().LookUpLegalItems, enemy.GetComponent<Person>().LookUpIllegalItems);
                enemy.GetComponent<Person>().GetComponent<PhotonView>().RPC("GetRobbed", PhotonTargets.All, (int)enemy.GetComponent<Person>().Team);
                
            }
        }

		[PunRPC]
		public void ResetLegalItems()
		{
			for(int i = 0; i < m_LegalItems.Count; i++) {
				m_LegalItems[i] = 0;
			}
		}

		[PunRPC]
		public void ResetIllegalItems()
		{
			for(int i = 0; i < m_IllegalItems.Count; i++) {
				m_IllegalItems[i] = 0;
			}
		}

		[PunRPC]
		public void GetRobbed(int teamId)
		{
			GameManager.s_Singleton.Teams[teamId - 1].AddOrRemoveMoney(-m_AmountOfMoney);
			m_AmountOfMoney = 0;
			ResetLegalItems();
			ResetIllegalItems();
      gameObject.GetComponent<RobStatus>().RecentlyGotRobbed = true;
    }



		public List<int> LookUpLegalItems
        {
			get { return m_LegalItems; }
		}

		public List<int> LookUpIllegalItems {
			get { return m_IllegalItems; }
		}

		[PunRPC]
		public void AddLegalItem(int index, int item)
		{
			m_LegalItems[index] += item;
		}

		[PunRPC]
		public void AddIllegalItem(int index, int item)
		{
			m_IllegalItems[index] += item;
		}


		[PunRPC]
		public void MoneyTransaction(int money)
		{
			m_AmountOfMoney += money;
			//photonView.RPC("UpdateTeamMoneyFromServer", PhotonTargets.MasterClient, money, (int)m_Team);
			GameManager.s_Singleton.Teams[(int)m_Team - 1].AddOrRemoveMoney(money);

		}

		//[PunRPC]
		public void TreasureTransaction(int amount, bool isEnemyTreasure)
		{
			TeamID id = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Districts.Treasure>().Team;

			//GameManager.s_Singleton.GetTreasureFrom(id).EmptyChest(amount);
			GameManager.s_Singleton.GetTreasureFrom(id).GetComponent<PhotonView>().RPC("EmptyChest", PhotonTargets.All, amount);
			GameManager.s_Singleton.Player.Person.photonView.RPC("TransactionMoney", PhotonTargets.AllViaServer, amount);
			if(isEnemyTreasure) {
				GameManager.s_Singleton.Teams[(int)m_Team - 1].GetComponent<PhotonView>().RPC("AddOrRemoveMoney", PhotonTargets.All, amount);
				GameManager.s_Singleton.Teams[(int)id - 1].GetComponent<PhotonView>().RPC("AddOrRemoveMoney", PhotonTargets.All, -amount);

				//GameManager.s_Singleton.Teams[(int)m_Team - 1].AddOrRemoveMoney(amount);
				//GameManager.s_Singleton.Teams[(int)id - 1].AddOrRemoveMoney(-amount);
			}

		}

		public void AddGoods(int money, List<int> legalItems, List<int> illegalItems)// Used when stealing from someone
		{     
			photonView.RPC("MoneyTransaction", PhotonTargets.All, money);

			for(int i = 0; i < legalItems.Count; i++) {
				photonView.RPC("AddLegalItem", PhotonTargets.All, i, legalItems[i]);
			}
			for(int i = 0; i < illegalItems.Count; i++) {
				photonView.RPC("AddIllegalItem", PhotonTargets.All, i, illegalItems[i]);
			}
            
		}

		public int AmountOfMoney {
			get { return m_AmountOfMoney; }
		}


		[PunRPC]
		public void TransactionMoney(int money)
		{ 
			m_AmountOfMoney += money; 
		}


		//private void Update()
		//{
		//	/*int amountOfTeams; //= LobbyPlayerList._instance.LobbyPlayerMatrix.GetLength(0);
		//if(!mIsReady && GameManager.s_Singleton.transform.childCount == amountOfTeams && Name != "Not set" && Team != TeamID.NotSet) {
		//	mIsReady = true;
		//	Debug.Log("Starting game");

		//	transform.GetChild(0).GetComponent<TextMesh>().text = Name;
		//	GetComponent<Renderer>().material.color = TeamData.GetColor(mTeam);
		//	transform.SetParent(GameManager.s_Singleton.transform.GetChild(((int)Team) - 1));

		//	NetworkIdentity networkIdentity = GetComponent<NetworkIdentity>();
		//	if(networkIdentity.isLocalPlayer) {
		//		Player player = gameObject.AddComponent<Player>();
		//		Debug.Log("GameManager Player has been set");
		//		GameManager.s_Singleton.Player = player;
		//		GameManager.s_Singleton.DistrictManager = GameObject.FindWithTag("Districts").GetComponent<DistrictManager>();
		//		GameManager.s_Singleton.DistrictManager.mPlayerTrans = transform;
		//		name = "Player ID:" + networkIdentity.netId + " (" + Name + ")";
		//	} else if(LobbyPlayer.mLocalPlayerTeam == mTeam) {
		//		Friend friend = gameObject.AddComponent<Friend>();
		//		name = "Friend ID:" + networkIdentity.netId + " (" + Name + ")";
		//	} else {
		//		Enemy enemy = gameObject.AddComponent<Enemy>();
		//		name = "Enemy ID:" + networkIdentity.netId + " (" + Name + ")";
		//	}
		//}*/
		//}
	}
}