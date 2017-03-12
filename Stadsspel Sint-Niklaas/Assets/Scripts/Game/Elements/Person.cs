using System.Collections.Generic;
using UnityEngine;

namespace Stadsspel.Elements
{
	public class Person : Element
	{
		private List<int> m_IllegalItems = new List<int>();

		//legalItems[(int)Items.diploma] = 10; Bijvoorbeeld
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

		//public void Rob()
		//{
		//	foreach (GameObject enemy in enemiesInRadius) {
		//		AddGoods(enemy.GetComponent<Person>().AmountOfMoney, enemy.GetComponent<Person>().LookUpLegalItems, enemy.GetComponent<Person>().LookUpIllegalItems);
		//		enemy.GetComponent<Person>().GetRobbed();
		//	}
		//}

		public void ResetLegalItems()
		{
			for(int i = 0; i < m_LegalItems.Count; i++) {
				m_LegalItems[i] = 0;
			}
		}

		public void ResetIllegalItems()
		{
			for(int i = 0; i < m_IllegalItems.Count; i++) {
				m_IllegalItems[i] = 0;
			}
		}

		public void GetRobbed()
		{
			GameManager.s_Singleton.Teams[(int)m_Team].AddOrRemoveMoney(-m_AmountOfMoney);
			m_AmountOfMoney = 0;
			ResetLegalItems();
			ResetIllegalItems();
		}

		public List<int> LookUpLegalItems {
			get { return m_LegalItems; }
		}

		public List<int> LookUpIllegalItems {
			get { return m_IllegalItems; }
		}

		public void AddLegalItems(List<int> items)
		{
			for(int i = 0; i < items.Count; i++) {
				m_LegalItems[i] += items[i];
			}
		}

		public void AddIllegalItems(List<int> items)
		{
			for(int i = 0; i < items.Count; i++) {
				m_IllegalItems[i] += items[i];
			}
		}


		[PunRPC]
		public void MoneyTransaction(int money)
		{
			m_AmountOfMoney += money;
			GameManager.s_Singleton.Teams[(int)m_Team - 1].GetComponent<PhotonView>().RPC("AddOrRemoveMoney", PhotonTargets.All, money);
    }

		[PunRPC]
		public void TreasureTransaction(int amount, bool isEnemyTreasure)
		{
			TeamID id = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Districts.Treasure>().TeamID;

			//GameManager.s_Singleton.GetTreasureFrom(id).EmptyChest(amount);
			GameManager.s_Singleton.GetTreasureFrom(id).GetComponent<PhotonView>().RPC("EmptyChest", PhotonTargets.All, amount);
			m_AmountOfMoney += amount;
			if(isEnemyTreasure) {
        GameManager.s_Singleton.Teams[(int)m_Team - 1].GetComponent<PhotonView>().RPC("AddOrRemoveMoney", PhotonTargets.All, amount);
        GameManager.s_Singleton.Teams[(int)id - 1].GetComponent<PhotonView>().RPC("AddOrRemoveMoney", PhotonTargets.All, -amount);

        //GameManager.s_Singleton.Teams[(int)m_Team - 1].AddOrRemoveMoney(amount);
				//GameManager.s_Singleton.Teams[(int)id - 1].AddOrRemoveMoney(-amount);
			}

		}

		public void AddGoods(int money, List<int> legalItems, List<int> illegalItems)
		{
			MoneyTransaction(money);
			AddLegalItems(legalItems);
			AddIllegalItems(illegalItems);
		}

		public int AmountOfMoney {
			get { return m_AmountOfMoney; }
		}

		public void BankTransaction(int money)
		{ 
			m_AmountOfMoney += money; 
		}


		private void Update()
		{
			/*int amountOfTeams; //= LobbyPlayerList._instance.LobbyPlayerMatrix.GetLength(0);
		if(!mIsReady && GameManager.s_Singleton.transform.childCount == amountOfTeams && Name != "Not set" && Team != TeamID.NotSet) {
			mIsReady = true;
			Debug.Log("Starting game");

			transform.GetChild(0).GetComponent<TextMesh>().text = Name;
			GetComponent<Renderer>().material.color = TeamData.GetColor(mTeam);
			transform.SetParent(GameManager.s_Singleton.transform.GetChild(((int)Team) - 1));

			NetworkIdentity networkIdentity = GetComponent<NetworkIdentity>();
			if(networkIdentity.isLocalPlayer) {
				Player player = gameObject.AddComponent<Player>();
				Debug.Log("GameManager Player has been set");
				GameManager.s_Singleton.Player = player;
				GameManager.s_Singleton.DistrictManager = GameObject.FindWithTag("Districts").GetComponent<DistrictManager>();
				GameManager.s_Singleton.DistrictManager.mPlayerTrans = transform;
				name = "Player ID:" + networkIdentity.netId + " (" + Name + ")";
			} else if(LobbyPlayer.mLocalPlayerTeam == mTeam) {
				Friend friend = gameObject.AddComponent<Friend>();
				name = "Friend ID:" + networkIdentity.netId + " (" + Name + ")";
			} else {
				Enemy enemy = gameObject.AddComponent<Enemy>();
				name = "Enemy ID:" + networkIdentity.netId + " (" + Name + ")";
			}
		}*/
		}
	}
}