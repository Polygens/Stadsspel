using Stadsspel.Districts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Stadsspel.Elements
{
	public class Player : MonoBehaviour
	{
		private List<GameObject> m_EnemiesInRadius = new List<GameObject>();

		[SerializeField] private List<GameObject> m_AllGameObjectsInRadius = new List<GameObject>();

		private Person m_Person;

		private Button[] m_Buttons;
		private int[] m_CurrentButtons;
		private int m_HighestPriority;
		private float m_UpdateTimer = 0;
		private float m_UpdateTime = 1;
		private float m_RobTimer = 30;

		//order of strings is important
		private string[] m_ButtonNames = new string[]
		{
			"Ruil",
			"Bank",
			"Koop",
			"Verkoop",
			"Belastingen innen",
			"Roven",
			"Stelen",
			"Overnemen"
		};

		private RectTransform[] m_Panels;

		private RectTransform m_MainPanel;
		private RectTransform m_ListPanel;
		private RectTransform m_Switch;

		private bool m_UIisInitialized = false;

		private int m_NumberOfButtonsInlistPanel = 0;

		RobStatus robStatus;

		public Person Person {
			get { return m_Person; }

			set { m_Person = value; }
		}

		public List<GameObject> AllGameObjectsInRadius {
			get { return m_AllGameObjectsInRadius; }
		}

		public int NumberOfButtonsInlistPanel {
			get { return m_NumberOfButtonsInlistPanel; }
		}

		public List<GameObject> EnemiesInRadius {
			get { return m_EnemiesInRadius; }
		}

		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
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
		/// <summary>
		/// Initialises the class.
		/// </summary>
		private void Start()
		{
			InitializeUI();

			tag = "Player";

			//Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
			//rigidbody.isKinematic = true;

			Camera.main.GetComponent<AudioListener>().enabled = true;

			MoveAvatar ma = gameObject.AddComponent<MoveAvatar>();
			ma.isPlayer = true;

			CurrentGame.Instance.UIPlayer = this;
			//GameManager.s_Singleton.DistrictManager.mPlayerTrans = transform;
		}

		/// <summary>
		/// Gets called every frame.
		/// </summary>
		private void Update()
		{
			if (robStatus.RecentlyGotRobbed)
			{
				if (m_MainPanel.GetChild(0).GetComponent<Button>().interactable &&
					((Priority)m_HighestPriority).ToString() == "Enemy")
				{
					m_MainPanel.GetChild(0).GetComponent<Button>().interactable = false;
				}
				m_RobTimer -= Time.deltaTime;
				if (m_RobTimer <= 0)
				{
					robStatus.RecentlyGotRobbed = false;
					m_RobTimer = 30;
					if (m_MainPanel.childCount > 0 && ((Priority)m_HighestPriority).ToString() == "Enemy")
					{
						m_MainPanel.GetChild(0).transform.Find("Text").GetComponent<Text>().text = "Stelen";
						m_MainPanel.GetChild(0).GetComponent<Button>().interactable = true;
					}
				} else
				{
					m_UpdateTimer += Time.deltaTime;
					if (m_UpdateTimer > m_UpdateTime)
					{
						m_UpdateTimer = 0;
						if (m_MainPanel.childCount > 0 && ((Priority)m_HighestPriority).ToString() == "Enemy")
							m_MainPanel.GetChild(0).transform.Find("Text").GetComponent<Text>().text =
								"Wacht " + Mathf.RoundToInt(m_RobTimer) + "s";
					}
				}
			}
		}

		/// <summary>
		/// Initialises all the UI variables and sets up the required panels.
		/// </summary>
		private void InitializeUI()
		{
			robStatus = gameObject.GetComponent<RobStatus>();
			RectTransform priorityButtons = InGameUIManager.s_Singleton.PriorityButtons;
			RectTransform panelsInCanvas = InGameUIManager.s_Singleton.Panels;
			int numberOfPanels = panelsInCanvas.transform.childCount;
			m_Panels = new RectTransform[numberOfPanels];
			for (int i = 0; i < numberOfPanels; i++)
			{
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
			for (int i = 0; i < lengthPriorities; i++)
			{
				Button tempButton = Resources.Load("PriorityButton", typeof(Button)) as Button;

				m_Buttons[i] = tempButton;
				m_CurrentButtons[i] = 0;
			}
			m_UIisInitialized = true;
		}

		/// <summary>
		/// Handles all logic for adding and removing buttons from the action button list. TODO: Split up in more managable functions
		/// </summary>
		public void PriorityUpdate(List<GameObject> allGameObjectsInRadius)
		{
			if (allGameObjectsInRadius.Count > 0)
			{
				// In case there were no collidings before.
				m_MainPanel.gameObject.SetActive(true);
				m_Switch.gameObject.SetActive(false);

				if (allGameObjectsInRadius.Count > 1) //todo change check so that button holder does not display when no buttons
				{
					m_ListPanel.gameObject.SetActive(true);
					m_Switch.gameObject.SetActive(true);
				}

				int lengthEnum = Enum.GetValues(typeof(Priority)).Cast<Priority>().Count();
				int[] priorityPresence = new int[lengthEnum];
				for (int i = 0; i < priorityPresence.Length; i++)
				{
					priorityPresence[i] = 0;
				}

				// Set temp priority to lowest
				int tempPriority = 0;
				int priorityNbr = 0;

				//Check with the tag of the gameObject, which priority it has in the enum,
				// If the priority is higher then the current, update the priority
				Priority tempP;
				for (int i = 0; i < allGameObjectsInRadius.Count; i++)
				{
					string tag = allGameObjectsInRadius[i].tag;
#if (UNITY_EDITOR)
					Debug.Log("priority update: " + tag + " And name of object: " + allGameObjectsInRadius[i].name);
#endif
					if (tag == "Treasure") //todo make switch case
					{
						/*Square */
						Debug.Log("tag == Treasure");
						if (allGameObjectsInRadius[i].GetComponent<Square>().Team == m_Person.Team)
						{
							tempP = Priority.Treasure;
						} else
						{
							tempP = Priority.TreasureEnemy;
						}
					} else if (tag == "Square")
					{
						tempP = Priority.EnemyDistrict;
					} else if (tag == "Bank")
					{
						tempP = Priority.Bank;
						GameObject bank = allGameObjectsInRadius[i];
						Bank bankScript = bank.GetComponent<Bank>();
						if (bankScript != null)
						{
							CurrentGame.Instance.nearBank = bankScript.BankId;
						}
					} else if (tag.Equals("TradingPost", StringComparison.InvariantCultureIgnoreCase))
					{
						GameObject tp = allGameObjectsInRadius[i];
						TradingPost tpScript = tp.GetComponent<TradingPost>();

						if (tpScript != null)
						{
							CurrentGame.Instance.nearTP = tpScript.TPId;
						}
						tempP = Priority.TradingPost;
					} else
					{
						if ((Priority)Enum.Parse(typeof(Priority),tag) != Priority.Enemy) //exclude enemies from this check
						{
							tempP = (Priority)Enum.Parse(typeof(Priority), tag);
						}
						else
						{
							continue; //if is enemy skip loop iteration
						}
					}

					if (allGameObjectsInRadius[i].GetComponentInParent<CapturableDistrict>() == null)
					{
						priorityNbr = (int)tempP;
						priorityPresence[priorityNbr] = 1;
					}

					if (priorityNbr > tempPriority)
					{
						if (allGameObjectsInRadius[i].GetComponentInParent<CapturableDistrict>() == null)
							tempPriority = priorityNbr;
					}
				}


				//just in case check for players
				//this should be a shorter version of one loop from the above loop
				if (CurrentGame.Instance.TagablePlayers.Count >0)
				{
					tempP = Priority.Enemy;
					priorityNbr = (int)tempP;
					priorityPresence[priorityNbr] = 1;
					if (priorityNbr > tempPriority)
					{
						tempPriority = priorityNbr;
					}

				}


				// if there is a new highestpriority, do next lines

				m_HighestPriority = tempPriority;
				Button mainButton = null;
				RectTransform tempPanel = null;

				//Make room for new mainbutton
				if (m_MainPanel.childCount > 0)
				{
					for (int i = 0; i < m_MainPanel.childCount; i++)
					{
						Destroy(m_MainPanel.GetChild(i).gameObject);
					}
				}

				if (m_HighestPriority != 0)
				{
					mainButton = Instantiate(m_Buttons[m_HighestPriority], transform.position, transform.rotation, m_MainPanel);
					mainButton.transform.Find("Text").GetComponent<Text>().text = m_ButtonNames[m_HighestPriority];
				}

				if (((Priority)m_HighestPriority).ToString() == "Enemy")
				{
					if (!robStatus.RecentlyGotRobbed)
					{
						mainButton.GetComponent<Button>().onClick.AddListener(() => m_Person.Rob());
					}
				} else
				{
					//names of the panels need to be the same as the priorities & layernames
					for (int j = 0; j < m_Panels.Length; j++)
					{
						if (m_Panels[j].name == ((Priority)m_HighestPriority).ToString())
						{
							if ("Enemy" != ((Priority)m_HighestPriority).ToString())
							{
								tempPanel = m_Panels[j];
								mainButton.GetComponent<Button>().onClick.AddListener(() => buttonClicked(tempPanel));
							}
						}
					}
				}

				// For all priorities, check if more buttons are needed in listpanel
				for (int i = priorityPresence.Length - 1; i >= 0; i--)
				{
					//we dont want mainbutton in the listpanel
					if (i != m_HighestPriority)
					{
						//Spawn specific priority button
						if (m_CurrentButtons[i] == 0 && priorityPresence[i] == 1)
						{
							Button tempB = (Button)Instantiate(m_Buttons[i], transform.position, transform.rotation, m_ListPanel);
							tempB.transform.Find("Text").GetComponent<Text>().text = m_ButtonNames[i];
							tempPanel = null;
							for (int j = 0; j < m_Panels.Length; j++)
							{
								if (m_Panels[j].name == ((Priority)i).ToString())
								{
									tempPanel = m_Panels[j];
								}
							}
							if (tempPanel != null)
							{
								tempB.GetComponent<Button>().onClick.AddListener(() => buttonClicked(tempPanel));
							}

							// 1 indicates that the button of the specific priority is present in the listpanel
							m_CurrentButtons[i] = 1;
							m_NumberOfButtonsInlistPanel++;
						}
						if ((m_CurrentButtons[i] == 1 && priorityPresence[i] == 0))
						{
							TryToDestroyIndexOfListPanel(i);
						}
					} else
					{
						TryToDestroyIndexOfListPanel(i);
					}
				}
			} else
			{
				m_MainPanel.gameObject.SetActive(false);
				m_ListPanel.gameObject.SetActive(false);
				m_Switch.gameObject.SetActive(false);
			}
		}


		/// <summary>
		/// Frankensteined version of the priority update method to ensure tagging is able when the server says so
		/// </summary>
		public void OnTaggablePlayers()
		{
			// In case there were no collidings before.
			m_MainPanel.gameObject.SetActive(true);
			m_Switch.gameObject.SetActive(false);

			if (m_AllGameObjectsInRadius.Count + 1 > 1)
			{
				m_ListPanel.gameObject.SetActive(true);
				m_Switch.gameObject.SetActive(true);
			}

			int lengthEnum = Enum.GetValues(typeof(Priority)).Cast<Priority>().Count();
			int[] priorityPresence = new int[lengthEnum];
			for (int i = 0; i < priorityPresence.Length; i++)
			{
				priorityPresence[i] = 0;
			}

			// Set temp priority to lowest
			int tempPriority = 0;
			int priorityNbr = 0;

			//loop through all gos in radius
			string tag;
			Priority tempP;
			for (int i = 0; i < m_AllGameObjectsInRadius.Count; i++)
			{
				tag = m_AllGameObjectsInRadius[i].tag;
				if (tag == "Treasure") //todo make switch case
				{
					/*Square */
					Debug.Log("tag == Treasure");
					if (m_AllGameObjectsInRadius[i].GetComponent<Square>().Team == m_Person.Team)
					{
						tempP = Priority.Treasure;
					} else
					{
						tempP = Priority.TreasureEnemy;
					}
				} else if (tag == "Square")
				{
					tempP = Priority.EnemyDistrict;
				} else if (tag == "Bank")
				{
					tempP = Priority.Bank;
					GameObject bank = m_AllGameObjectsInRadius[i];
					Bank bankScript = bank.GetComponent<Bank>();
					if (bankScript != null)
					{
						CurrentGame.Instance.nearBank = bankScript.BankId;
					}
				} else if (tag.Equals("TradingPost", StringComparison.InvariantCultureIgnoreCase))
				{
					GameObject tp = m_AllGameObjectsInRadius[i];
					TradingPost tpScript = tp.GetComponent<TradingPost>();

					if (tpScript != null)
					{
						CurrentGame.Instance.nearTP = tpScript.TPId;
					}
					tempP = Priority.TradingPost;
				} else
				{
					tempP = (Priority)Enum.Parse(typeof(Priority), tag);
				}

				if (m_AllGameObjectsInRadius[i].GetComponentInParent<CapturableDistrict>() == null)
				{
					priorityNbr = (int)tempP;
					priorityPresence[priorityNbr] = 1;
				}

				if (priorityNbr > tempPriority)
				{
					if (m_AllGameObjectsInRadius[i].GetComponentInParent<CapturableDistrict>() == null)
						tempPriority = priorityNbr;
				}
			}


			//do one mock loop for the taggable players as we know they are presentstring tag = m_AllGameObjectsInRadius[i].tag;
			tempP = Priority.Enemy;
			priorityNbr = (int)tempP;
			priorityPresence[priorityNbr] = 1;
			if (priorityNbr > tempPriority)
			{
				tempPriority = priorityNbr;
			}

			// if there is a new highestpriority, do next lines
			m_HighestPriority = tempPriority;
			Button mainButton = null;
			RectTransform tempPanel = null;

			//Make room for new mainbutton
			if (m_MainPanel.childCount > 0)
			{
				for (int i = 0; i < m_MainPanel.childCount; i++)
				{
					Destroy(m_MainPanel.GetChild(i).gameObject);
				}
			}

			if (m_HighestPriority != 0)
			{
				mainButton = Instantiate(m_Buttons[m_HighestPriority], transform.position, transform.rotation, m_MainPanel);
				mainButton.transform.Find("Text").GetComponent<Text>().text = m_ButtonNames[m_HighestPriority];
			}

			if (((Priority)m_HighestPriority).ToString() == "Enemy")
			{
				if (!robStatus.RecentlyGotRobbed)
				{
					mainButton.GetComponent<Button>().onClick.AddListener(() => m_Person.Rob());
				}
			} else
			{
				//names of the panels need to be the same as the priorities & layernames
				for (int j = 0; j < m_Panels.Length; j++)
				{
					if (m_Panels[j].name == ((Priority)m_HighestPriority).ToString())
					{
						if ("Enemy" != ((Priority)m_HighestPriority).ToString())
						{
							tempPanel = m_Panels[j];
							mainButton.GetComponent<Button>().onClick.AddListener(() => buttonClicked(tempPanel));
						}
					}
				}
			}

			// For all priorities, check if more buttons are needed in listpanel
			for (int i = priorityPresence.Length - 1; i >= 0; i--)
			{
				//we dont want mainbutton in the listpanel
				if (i != m_HighestPriority)
				{
					//Spawn specific priority button
					if (m_CurrentButtons[i] == 0 && priorityPresence[i] == 1)
					{
						Button tempB = (Button)Instantiate(m_Buttons[i], transform.position, transform.rotation, m_ListPanel);
						tempB.transform.Find("Text").GetComponent<Text>().text = m_ButtonNames[i];
						tempPanel = null;
						for (int j = 0; j < m_Panels.Length; j++)
						{
							if (m_Panels[j].name == ((Priority)i).ToString())
							{
								tempPanel = m_Panels[j];
							}
						}
						if (tempPanel != null)
						{
							tempB.GetComponent<Button>().onClick.AddListener(() => buttonClicked(tempPanel));
						}

						// 1 indicates that the button of the specific priority is present in the listpanel
						m_CurrentButtons[i] = 1;
						m_NumberOfButtonsInlistPanel++;
					}
					if ((m_CurrentButtons[i] == 1 && priorityPresence[i] == 0))
					{
						TryToDestroyIndexOfListPanel(i);
					}
				} else
				{
					TryToDestroyIndexOfListPanel(i);
				}
			}
		}

		/// <summary>
		/// Tries to remove the button with passed index from the action button list.
		/// </summary>
		private void TryToDestroyIndexOfListPanel(int index)
		{
			for (int j = 0; j < m_ListPanel.childCount; j++)
			{
				if (m_ListPanel.GetChild(j).GetChild(0).GetComponent<Text>().text == m_ButtonNames[index])
				{
					m_ListPanel.GetChild(j).GetComponent<Button>().onClick.RemoveListener(() => buttonClicked(null));
					Destroy(m_ListPanel.GetChild(j).gameObject);
					m_CurrentButtons[index] = 0;
					m_NumberOfButtonsInlistPanel--;
				}
			}
		}

		/// <summary>
		/// Event function for action button clicked.
		/// </summary>
		private void buttonClicked(RectTransform panel)
		{
#if (UNITY_EDITOR)
			Debug.Log("Set " + panel.name + " active");
#endif

			panel.gameObject.SetActive(true);
		}

		/// <summary>
		/// Gets called when a collider enters the player's collider. Adds the other collider gameobject to the list of objects in radius.
		/// </summary>
		public void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag != "Untagged")
			{
				if (other.GetComponentInParent<CapturableDistrict>() == null && other.tag != "Team" && other.tag != "Enemy")
				{
					m_AllGameObjectsInRadius.Add(other.gameObject);
				}

				if (other.tag == "Enemy")
				{
					if (CheckAbleToSteal())
					{
						m_AllGameObjectsInRadius.Add(other.gameObject);
						m_EnemiesInRadius.Add(other.gameObject);
					}
				}

				if (m_UIisInitialized)
					PriorityUpdate(m_AllGameObjectsInRadius);
			}
		}

		private bool CheckAbleToSteal()
		{
			return CurrentGame.Instance.IsTaggingPermitted;
			/* todo maybe flesh it out a bit
			string gameDuration = GameObject.Find("GameDuration").GetComponent<Text>().text;
			string[] durations = gameDuration.Split(':');
			float maxTime = GameManager.s_Singleton.GameLength;

			int minutes = Mathf.FloorToInt(maxTime / 60);
			int defMinutes;
			int hours = Math.DivRem(minutes, 60, out defMinutes);
			if (defMinutes == 0)
			{
				defMinutes = 60;
			}

			if (int.Parse(durations[1]) < (defMinutes - 5))
			{
				return true;
			} else
			{
				return false;
			}
			*/
		}

		/// <summary>
		/// Gets called when a collider leaves the player's collider. Removes the other collider gameobject from the list of objects in radius.
		/// </summary>
		public void OnTriggerExit2D(Collider2D other)
		{
			m_EnemiesInRadius.Remove(other.gameObject);
			m_AllGameObjectsInRadius.Remove(other.gameObject);
			PriorityUpdate(m_AllGameObjectsInRadius);
		}

		/// <summary>
		/// Returns a GameObject with the passed name in the list of GameObjects in radius if found.
		/// </summary>
		public GameObject GetGameObjectInRadius(string tag)
		{
			GameObject obj = null;
			foreach (GameObject GO in m_AllGameObjectsInRadius)
			{
				if (GO.tag == tag)
				{
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
