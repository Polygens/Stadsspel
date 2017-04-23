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

		[SerializeField]
		private int m_AmountOfMoney = 0;

		private Districts.DistrictManager districtManager;

        public float colliderRadius = 23f;

		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
		private void Awake()
		{
			m_Team = Stadsspel.Networking.TeamExtensions.GetTeam(photonView.owner);
			if(PhotonNetwork.player == photonView.owner) {
				gameObject.AddComponent<Player>();
				transform.GetComponentInChildren(typeof(MainSquareArrow), true).gameObject.SetActive(true);
			}
			else if(m_Team == Stadsspel.Networking.TeamExtensions.GetTeam(PhotonNetwork.player)) {
				gameObject.AddComponent<Friend>();
			}
			else {
				gameObject.AddComponent<Enemy>();
			}
			districtManager = GameObject.FindWithTag("Districts").GetComponent<Districts.DistrictManager>();
		}

		/// <summary>
		/// Initialises the class.
		/// </summary>
		protected new void Start()
		{
			GetComponent<MeshRenderer>().material.color = TeamData.GetColor(m_Team);
			transform.SetParent(GameManager.s_Singleton.Teams[(byte)m_Team - 1].transform, false);
			transform.GetChild(0).GetComponent<TextMesh>().text = photonView.owner.NickName;
			ActionRadius = colliderRadius;

			//instantiate list with 3 numbers for each list.

			for(int i = 0; i < 3; i++) {
				m_LegalItems.Add(0);
				m_IllegalItems.Add(0);
			}
        }

		/// <summary>
		/// Robs every player in the player radius.
		/// </summary>
		public void Rob()
		{
			foreach(GameObject enemy in GameManager.s_Singleton.Player.EnemiesInRadius) {
				int enemyMoney = enemy.GetComponent<Person>().AmountOfMoney;
				AddGoods(enemyMoney, enemy.GetComponent<Person>().LookUpLegalItems, enemy.GetComponent<Person>().LookUpIllegalItems, (int)enemy.GetComponent<Person>().Team);
				enemy.GetComponent<Person>().GetComponent<PhotonView>().RPC("GetRobbed", PhotonTargets.All, (int)enemy.GetComponent<Person>().Team);

			}
		}

		/// <summary>
		/// [PunRPC] TODO
		/// </summary>
		[PunRPC]
		public void ResetLegalItems()
		{
			for(int i = 0; i < m_LegalItems.Count; i++) {
				m_LegalItems[i] = 0;
			}
		}

		/// <summary>
		/// [PunRPC] TODO
		/// </summary>
		[PunRPC]
		public void ResetIllegalItems()
		{
			for(int i = 0; i < m_IllegalItems.Count; i++) {
				m_IllegalItems[i] = 0;
			}
		}

		/// <summary>
		/// [PunRPC] TODO
		/// </summary>
		[PunRPC]
		public void GetRobbed(int teamId)
		{
			GameManager.s_Singleton.Teams[teamId - 1].AddOrRemoveMoney(-m_AmountOfMoney);
			m_AmountOfMoney = 0;

            if (districtManager.CurrentDistrict.GetComponent<Districts.District>() != null)
            {
                Districts.District currentDistrict = districtManager.CurrentDistrict.GetComponent<Districts.District>();
                if (currentDistrict.DistrictType == Districts.DistrictType.square || currentDistrict.DistrictType == Districts.DistrictType.CapturableDistrict || currentDistrict.DistrictType == Districts.DistrictType.HeadDistrict)
                {
                    if ((int)currentDistrict.Team != teamId && currentDistrict.Team != TeamID.NoTeam && currentDistrict.Team != TeamID.NotSet)
                    {
                        ResetLegalItems();
                    }
                }
            }
                
			ResetIllegalItems();
			gameObject.GetComponent<RobStatus>().RecentlyGotRobbed = true;
		}



		public List<int> LookUpLegalItems {
			get { return m_LegalItems; }
		}

		public List<int> LookUpIllegalItems {
			get { return m_IllegalItems; }
		}

		/// <summary>
		/// [PunRPC] TODO
		/// </summary>
		[PunRPC]
		public void AddLegalItem(int index, int item)
		{
			m_LegalItems[index] += item;
		}

		/// <summary>
		/// [PunRPC] TODO
		/// </summary>
		[PunRPC]
		public void AddIllegalItem(int index, int item)
		{
			m_IllegalItems[index] += item;
		}


        /// <summary>
        /// [PunRPC] Performs a money transaction from outside the team to the player.
        /// </summary>
        [PunRPC]
		public void MoneyTransaction(int money)
		{
			m_AmountOfMoney += money;
			//photonView.RPC("UpdateTeamMoneyFromServer", PhotonTargets.MasterClient, money, (int)m_Team);
			GameManager.s_Singleton.Teams[(int)m_Team - 1].AddOrRemoveMoney(money);

		}

		/// <summary>
		/// Performs a transaction of the passed amount on the treasure in the radius.
		/// </summary>
		public void TreasureTransaction(int amount, bool isEnemyTreasure)
		{
			TeamID id = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Districts.Treasure>().Team;

			//GameManager.s_Singleton.GetTreasureFrom(id).EmptyChest(amount);
			GameManager.s_Singleton.GetTreasureFrom(id).GetComponent<PhotonView>().RPC("ReduceChestMoney", PhotonTargets.All, amount);
			GameManager.s_Singleton.Player.Person.photonView.RPC("TransactionMoney", PhotonTargets.AllViaServer, amount);

			if(isEnemyTreasure)
            {
				GameManager.s_Singleton.Teams[(int)m_Team - 1].GetComponent<PhotonView>().RPC("AddOrRemoveMoney", PhotonTargets.All, amount);
				GameManager.s_Singleton.Teams[(int)id - 1].GetComponent<PhotonView>().RPC("AddOrRemoveMoney", PhotonTargets.All, -amount);

				//GameManager.s_Singleton.Teams[(int)m_Team - 1].AddOrRemoveMoney(amount);
				//GameManager.s_Singleton.Teams[(int)id - 1].AddOrRemoveMoney(-amount);
			}

		}

		/// <summary>
		/// TODO
		/// </summary>
		public void AddGoods(int money, List<int> legalItems, List<int> illegalItems, int enemyTeamId)// Used when stealing from someone
		{
			photonView.RPC("MoneyTransaction", PhotonTargets.All, money);

            GameManager.s_Singleton.DistrictManager.CheckDisctrictState();
			
            if(districtManager.CurrentDistrict.GetComponent<Districts.District>() != null)
            {
                Districts.District currentDistrict = districtManager.CurrentDistrict.GetComponent<Districts.District>();
                if (currentDistrict.DistrictType == Districts.DistrictType.square || currentDistrict.DistrictType == Districts.DistrictType.CapturableDistrict || currentDistrict.DistrictType == Districts.DistrictType.HeadDistrict)
                {
                    if ((int)currentDistrict.Team != enemyTeamId && currentDistrict.Team != TeamID.NoTeam && currentDistrict.Team != TeamID.NotSet)
                    {
                        for (int i = 0; i < legalItems.Count; i++)
                        {
                            photonView.RPC("AddLegalItem", PhotonTargets.All, i, legalItems[i]);
                        }
                    }
                }
            }
			
			for(int i = 0; i < illegalItems.Count; i++) {
				photonView.RPC("AddIllegalItem", PhotonTargets.All, i, illegalItems[i]);
			}

		}

		public int AmountOfMoney {
			get { return m_AmountOfMoney; }
		}


		/// <summary>
		/// [PunRPC] Performs a money transaction from within the team to the player.
		/// </summary>
		[PunRPC]
		public void TransactionMoney(int money)
		{
			m_AmountOfMoney += money;
		}
	}
}