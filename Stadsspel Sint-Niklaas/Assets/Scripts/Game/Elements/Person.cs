using System;
using Boo.Lang;
using UnityEngine;

namespace Stadsspel.Elements
{
	public class Person : Element
	{
		[SerializeField]
		private System.Collections.Generic.List<int> m_IllegalItems = new System.Collections.Generic.List<int>();

		//legalItems[(int)Items.diploma] = 10; Bijvoorbeeld
		[SerializeField]
		private System.Collections.Generic.List<int> m_LegalItems = new System.Collections.Generic.List<int>();

		[SerializeField]
		private int m_AmountOfMoney = 0;

		private Districts.DistrictManager districtManager;

		public float colliderRadius = 23f;

		[SerializeField]
		public ServerPlayer Player { get; set; }

		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
		private void Awake()
		{
		}

		/// <summary>
		/// Initialises the class.
		/// </summary>
		protected new void Start()
		{
			//START: originally in awake
			Debug.Log("Server player: " + Player);
			if (Player != null)
			{
				Debug.Log("player name: " + Player.Name);
				if (CurrentGame.Instance.LocalPlayer.clientID.Equals(Player.clientID))
				{
					gameObject.AddComponent<Player>();
					transform.GetComponentInChildren(typeof(MainSquareArrow), true).gameObject.SetActive(true);
					m_Team = CurrentGame.Instance.PlayerTeam;
				} else if (CurrentGame.Instance.PlayerTeam.ContainsPlayer(Player.clientID))
				{
					gameObject.AddComponent<Friend>();
					m_Team = CurrentGame.Instance.PlayerTeam;
				} else
				{
					gameObject.AddComponent<Enemy>();
					m_Team = CurrentGame.Instance.gameDetail.findTeamByPlayer(Player.clientID);
				}

				districtManager = GameObject.FindWithTag("Districts").GetComponent<Districts.DistrictManager>();
			}
			//END

			Color color = new Color();
			ColorUtility.TryParseHtmlString(m_Team.customColor, out color);
			GetComponent<MeshRenderer>().material.color = color;

			transform.SetParent(GameManager.s_Singleton.Teams[CurrentGame.Instance.gameDetail.IndexOfTeam(m_Team)].transform, false);
			transform.GetChild(0).GetComponent<TextMesh>().text = Player.Name;
			ActionRadius = colliderRadius;

			//instantiate list with 3 numbers for each list.

			for (int i = 0; i < 3; i++)
			{
				m_LegalItems.Add(0);
				m_IllegalItems.Add(0);
			}
		}

		/// <summary>
		/// Robs every player in the player radius.
		/// </summary>
		public void Rob()
		{
			CurrentGame.Instance.Ws.SendTag(CurrentGame.Instance.TagablePlayers,CurrentGame.Instance.currentDistrictID);
			//todo these parameters look stupid.
		}

		/// <summary>
		/// [PunRPC] TODO
		/// </summary>
		[PunRPC]
		public void ResetLegalItems()
		{
			for (int i = 0; i < m_LegalItems.Count; i++)
			{
				m_LegalItems[i] = 0;
			}
		}

		/// <summary>
		/// [PunRPC] TODO
		/// </summary>
		[PunRPC]
		public void ResetIllegalItems()
		{
			for (int i = 0; i < m_IllegalItems.Count; i++)
			{
				m_IllegalItems[i] = 0;
			}
		}

		/// <summary>
		/// [PunRPC] TODO
		/// todo g: server
		/// </summary>
		[PunRPC]
		public void GetRobbed(int teamId)
		{
			/*
			GameManager.s_Singleton.Teams[teamId - 1].AddOrRemoveMoney(-m_AmountOfMoney);
			m_AmountOfMoney = 0;

			if (districtManager.CurrentDistrict.GetComponent<Districts.District>() != null)
			{
				Districts.District currentDistrict = districtManager.CurrentDistrict.GetComponent<Districts.District>();
				if (currentDistrict.DistrictType == Districts.DistrictType.square || currentDistrict.DistrictType == Districts.DistrictType.CapturableDistrict || currentDistrict.DistrictType == Districts.DistrictType.HeadDistrict)
				{
					if ((int)currentDistrict.Team != teamId && currentDistrict.Team != null && currentDistrict.Team != TeamID.NotSet)
					{
						ResetLegalItems();
					}
				}
			}

			ResetIllegalItems();
			gameObject.GetComponent<RobStatus>().RecentlyGotRobbed = true;
			*/
		}



		public System.Collections.Generic.List<int> LookUpLegalItems {
			get { return m_LegalItems; }
		}

		public System.Collections.Generic.List<int> LookUpIllegalItems {
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
			/* todo what is this?
			m_AmountOfMoney += money;
			//photonView.RPC("UpdateTeamMoneyFromServer", PhotonTargets.MasterClient, money, (int)m_Team);
			GameManager.s_Singleton.Teams[(int)m_Team - 1].AddOrRemoveMoney(money);
			*/
		}

		/// <summary>
		/// Performs a transaction of the passed amount on the treasure in the radius.
		/// </summary>
		public void TreasureTransaction(int amount, bool isEnemyTreasure)
		{
			//todo this is rob an enemy treasure as well as your own --> mux different events
			/*
			TeamID id = GameManager.s_Singleton.Player.GetGameObjectInRadius("Treasure").GetComponent<Districts.Treasure>().Team;

			//GameManager.s_Singleton.GetTreasureFrom(id).EmptyChest(amount);
			GameManager.s_Singleton.GetTreasureFrom(id).GetComponent<PhotonView>().RPC("ReduceChestMoney", PhotonTargets.All, amount);
			GameManager.s_Singleton.Player.Person.photonView.RPC("TransactionMoney", PhotonTargets.AllViaServer, amount);

			if (isEnemyTreasure)
			{
				GameManager.s_Singleton.Teams[(int)m_Team - 1].GetComponent<PhotonView>().RPC("AddOrRemoveMoney", PhotonTargets.All, amount);
				GameManager.s_Singleton.Teams[(int)id - 1].GetComponent<PhotonView>().RPC("AddOrRemoveMoney", PhotonTargets.All, -amount);

				//GameManager.s_Singleton.Teams[(int)m_Team - 1].AddOrRemoveMoney(amount);
				//GameManager.s_Singleton.Teams[(int)id - 1].AddOrRemoveMoney(-amount);
			}
			*/

		}

		/// <summary>
		/// TODO
		/// </summary>
		public void AddGoods(int money, System.Collections.Generic.List<int> legalItems, System.Collections.Generic.List<int> illegalItems, int enemyTeamId)// Used when stealing from someone
		{
			/* todo this should be handled by the server
			photonView.RPC("MoneyTransaction", PhotonTargets.All, money);

			GameManager.s_Singleton.DistrictManager.CheckDisctrictState();

			if (districtManager.CurrentDistrict.GetComponent<Districts.District>() != null)
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

			for (int i = 0; i < illegalItems.Count; i++)
			{
				photonView.RPC("AddIllegalItem", PhotonTargets.All, i, illegalItems[i]);
			}
			*/

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