using GoMap;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Stadsspel.Districts;

namespace Stadsspel.Elements
{
	public class Player : MonoBehaviour
	{
		private List<GameObject> m_EnemiesInRadius = new List<GameObject>();

		[SerializeField]
		private List<GameObject> m_AllGameObjectsInRadius = new List<GameObject>();

		private Person m_Person;

		private Button[] m_Buttons;
		private int[] m_CurrentButtons;
		private int m_HighestPriority;

		//order of strings is important
		private string[] m_ButtonNames = new string[] {
			"Ruil",
			"Bank",
			"Koop",
			"Verkoop",
			"Belastingen innen",
			"Belastingen stelen",
			"Stelen",
			"Overnemen"
		};

		private RectTransform[] m_Panels;

		private RectTransform m_MainPanel;
		private RectTransform m_ListPanel;
		private RectTransform m_Switch;

		private int m_NumberOfButtonsInlistPanel = 0;

		public Person Person {
			get {
				return m_Person;
			}

			set {
				m_Person = value;
			}
		}

		public List<GameObject> AllGameObjectsInRadius {
			get {
				return m_AllGameObjectsInRadius;
			}
		}

		public int NumberOfButtonsInlistPanel {
			get {
				return m_NumberOfButtonsInlistPanel;
			}
		}

		private void Awake()
		{
			m_Person = GetComponent<Person>();
			GameManager.s_Singleton.Player = this;
		}

		// Team,
		//Bank,
		//TradingPost,
		//Markt,
		//Schatkist,
		//SchatkistEnemy
		// EnemyPlein,
		//Enemy
		private void Start()
		{
			InitializeUI();

			tag = "Player";

			//Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
			//rigidbody.isKinematic = true;

			Camera.main.GetComponent<AudioListener>().enabled = true;


			gameObject.AddComponent<MoveAvatar>();

			//GameManager.s_Singleton.DistrictManager.mPlayerTrans = transform;
		}

		private void InitializeUI()
		{
			RectTransform priorityButtons = InGameUIManager.s_Singleton.PriorityButtons;
			RectTransform panelsInCanvas = InGameUIManager.s_Singleton.Panels;
			int numberOfPanels = panelsInCanvas.transform.childCount;
			m_Panels = new RectTransform[numberOfPanels];
			for(int i = 0; i < numberOfPanels; i++) {
				m_Panels[i] = (RectTransform)panelsInCanvas.GetChild(i);
			}
			m_MainPanel = (RectTransform)priorityButtons.GetChild(1);
			m_ListPanel = (RectTransform)priorityButtons.GetChild(0).GetChild(0);
			m_Switch = (RectTransform)priorityButtons.GetChild(2);
			m_MainPanel.gameObject.SetActive(false);
			m_ListPanel.gameObject.SetActive(false);
			m_Switch.gameObject.SetActive(false);
			int lengthPriorities = Enum.GetValues(typeof(Priority)).Cast<Priority>().Count();
			m_Buttons = new Button[lengthPriorities];
			m_CurrentButtons = new int[lengthPriorities];
			for(int i = 0; i < lengthPriorities; i++) {
				Button tempButton = Resources.Load("PriorityButton", typeof(Button)) as Button;

				m_Buttons[i] = tempButton;
				m_CurrentButtons[i] = 0;
			}
		}

		public void Rob()
		{
			foreach(GameObject enemy in m_EnemiesInRadius) {
				m_Person.AddGoods(enemy.GetComponent<Person>().AmountOfMoney, enemy.GetComponent<Person>().LookUpLegalItems, enemy.GetComponent<Person>().LookUpIllegalItems);
				enemy.GetComponent<Person>().GetRobbed();
			}
		}

		public void PriorityUpdate(List<GameObject> allGameObjectsInRadius, Collider2D other)
		{
			if(allGameObjectsInRadius.Count > 0) {

				// In case there were no collidings before.
				m_MainPanel.gameObject.SetActive(true);
				m_ListPanel.gameObject.SetActive(true);
				if(allGameObjectsInRadius.Count != 1) {
					m_Switch.gameObject.SetActive(true);
				}

				int lengthEnum = Enum.GetValues(typeof(Priority)).Cast<Priority>().Count();
				int[] priorityPresence = new int[lengthEnum];
				for(int i = 0; i < priorityPresence.Length; i++) {
					priorityPresence[i] = 0;
				}

				// Set temp priority to lowest
				int tempPriority = 0;
				int priorityNbr = 0;

				//Check with the tag of the gameObject, which priority it has in the enum,
				// If the priority is higher then the current, update the priority
				for(int i = 0; i < allGameObjectsInRadius.Count; i++) {
					string tag = allGameObjectsInRadius[i].tag;
					Debug.Log("priority update: " + tag + " And name of object: " + allGameObjectsInRadius[i].name);
					Priority tempP;
					if(tag == "Treasure") { /*Square */
						if(allGameObjectsInRadius[i].GetComponent<Square>().TeamID == m_Person.Team) {
							tempP = Priority.Treasure;
						} else {
							tempP = Priority.TreasureEnemy;
						}

					} else if(tag == "Square") {
						tempP = Priority.EnemyDistrict;
					} else {
						tempP = (Priority)Enum.Parse(typeof(Priority), tag);
					}

					priorityNbr = (int)tempP;
					priorityPresence[priorityNbr] = 1;
					if(priorityNbr > tempPriority) {
						tempPriority = priorityNbr;
					}
				}

				// if there is a new highestpriority, do next lines
				if(tempPriority != m_HighestPriority) {
					m_HighestPriority = tempPriority;

					//Make room for new mainbutton
					if(m_MainPanel.childCount > 0) {
						GameObject tempB = m_MainPanel.GetChild(0).gameObject;
						Destroy(tempB);
					}

					Button mainButton = (Button)Instantiate(m_Buttons[m_HighestPriority], transform.position, transform.rotation, m_MainPanel);
					mainButton.transform.FindChild("Text").GetComponent<Text>().text = m_ButtonNames[m_HighestPriority];
					RectTransform tempPanel = null;

					//names of the panels need to be the same as the priorities & layernames
					for(int j = 0; j < m_Panels.Length; j++) {
						if(m_Panels[j].name == ((Priority)m_HighestPriority).ToString()) {
							if("Enemy" != ((Priority)m_HighestPriority).ToString()) {
								tempPanel = m_Panels[j];
								mainButton.GetComponent<Button>().onClick.AddListener(() => buttonClicked(tempPanel));
							} else {
								mainButton.GetComponent<Button>().onClick.AddListener(() => Rob());
							}
						}
					}
				}

				// For all priorities, check if more buttons are needed in listpanel
				for(int i = priorityPresence.Length - 1; i >= 0; i--) {

					//we dont want mainbutton in the listpanel
					if(i != m_HighestPriority) {

						//Spawn specific priority button
						if(m_CurrentButtons[i] == 0 && priorityPresence[i] == 1) {
							Button tempB = (Button)Instantiate(m_Buttons[i], transform.position, transform.rotation, m_ListPanel);
							tempB.transform.FindChild("Text").GetComponent<Text>().text = m_ButtonNames[i];
							RectTransform tempPanel = null;
							for(int j = 0; j < m_Panels.Length; j++) {
								if(m_Panels[j].name == ((Priority)i).ToString()) {
									tempPanel = m_Panels[j];
								}
							}
							if(tempPanel != null) {
								tempB.GetComponent<Button>().onClick.AddListener(() => buttonClicked(tempPanel));
							}

							// 1 indicates that the button of the specific priority is present in the listpanel
							m_CurrentButtons[i] = 1;
							m_NumberOfButtonsInlistPanel++;
						}
						if((m_CurrentButtons[i] == 1 && priorityPresence[i] == 0)) {
							TryToDestroyIndexOfListPanel(i);
						}
					} else {
						TryToDestroyIndexOfListPanel(i);
					}

					//Debug.Log("For: " + i + " is currentButtons" + currentButtons[i] + " and priorityPresence " + priorityPresence[i]);
				}
			} else {
				m_MainPanel.gameObject.SetActive(false);
				m_ListPanel.gameObject.SetActive(false);
				m_Switch.gameObject.SetActive(false);
			}
		}

		private void TryToDestroyIndexOfListPanel(int index)
		{
			for(int j = 0; j < m_ListPanel.childCount; j++) {
				if(m_ListPanel.GetChild(j).GetChild(0).GetComponent<Text>().text == m_ButtonNames[index]) {
					m_ListPanel.GetChild(j).GetComponent<Button>().onClick.RemoveListener(() => buttonClicked(null));
					Destroy(m_ListPanel.GetChild(j).gameObject);
					m_CurrentButtons[index] = 0;
					m_NumberOfButtonsInlistPanel--;
				}
			}
		}

		private void buttonClicked(RectTransform panel)
		{
			Debug.Log("Set " + panel.name + " active");

			//for (int i = 0; i < panels.Length; i++)
			//{
			//  if (panels[i].gameObject.activeSelf)
			//  {
			//    panels[i].gameObject.SetActive(false);
			//  }
			//
			panel.gameObject.SetActive(true);
		}

		public void OnTriggerEnter2D(Collider2D other)
		{
			//Debug.Log(other.tag);
			if(other.tag != "Untagged") {
				m_AllGameObjectsInRadius.Add(other.gameObject);
				if(other.tag == "Enemy") {
					m_EnemiesInRadius.Add(other.gameObject);
				}

				PriorityUpdate(m_AllGameObjectsInRadius, other);
			}
		}

		public void OnTriggerExit2D(Collider2D other)
		{
			m_EnemiesInRadius.Remove(other.gameObject);
			m_AllGameObjectsInRadius.Remove(other.gameObject);
			PriorityUpdate(m_AllGameObjectsInRadius, other);
		}

		public GameObject GetGameObjectInRadius(string tag)
		{
			GameObject obj = null;
			foreach(GameObject GO in m_AllGameObjectsInRadius) {
				if(GO.tag == tag) {
					obj = GO.transform.gameObject;
				}
			}
			return obj;
		}
	}

	public enum Priority : byte
	{
		Team,
		Bank,
		TradingPost,
		GrandMarket,
		Treasure,
		TreasureEnemy,
		Enemy,
		EnemyDistrict
	}
}