﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using GoMap;

public class Player : Person
{
	private int mDetectionRadius;
	private List<GameObject> enemiesInRadius = new List<GameObject>();
	private List<GameObject> allGameObjectsInRadius = new List<GameObject>();
	private Button[] buttons;
	private int[] currentButtons;
	private int highestPriority;

  //order of strings is important
  private string[] buttonNames = new string[] { "Ruil", "Bank", "Koop", "Verkoop", "Belastingen innen", "Belastingen stelen", "Plein overnemen", "Stelen" }; 

	public Button prefabButton;
  private RectTransform[] panels; 

	private RectTransform MainPanel;
	private RectTransform ListPanel;
	private RectTransform Switch;
	public int mNumberOfButtonsInlistPanel = 0;

	// Team,
	//Bank,
	//TradingPost,
	//Markt,
	//Schatkist,
	//SchatkistEnemy
	// EnemyPlein,
	//Enemy
	private new void Start()
	{
		base.Start();

		MoveAvatar moveAvatar = gameObject.AddComponent<MoveAvatar>();
		moveAvatar.mAvatarDirection = transform.GetChild(1);

		DistrictManager districtManagar = GameObject.FindWithTag("Districts").GetComponent<DistrictManager>();
		moveAvatar.mDistrictManager = districtManagar;
		districtManagar.mPlayerTrans = transform;
		moveAvatar.mLocationManager = GameObject.FindWithTag("LocationManager").GetComponent<LocationManager>();

		RectTransform priorityButtons = (RectTransform)GameObject.FindWithTag("Canvas").transform.FindChild("PriorityButtons");
    RectTransform panelsInCanvas = (RectTransform)GameObject.FindWithTag("Canvas").transform.FindChild("Panels");
    int numberOfPanels = panelsInCanvas.transform.childCount;
    panels = new RectTransform[numberOfPanels];
    for (int i = 0; i < numberOfPanels; i++)
    {
      panels[i] = (RectTransform)panelsInCanvas.GetChild(i);
    }
		MainPanel = (RectTransform)priorityButtons.GetChild(1);
		ListPanel = (RectTransform)priorityButtons.GetChild(0).GetChild(0);
		Switch    = (RectTransform)priorityButtons.GetChild(2);
		MainPanel.gameObject.SetActive(false);
		ListPanel.gameObject.SetActive(false);
		Switch.gameObject.SetActive(false);
		int lengthPriorities = Enum.GetValues(typeof(priority)).Cast<priority>().Count();
		buttons = new Button[lengthPriorities];
		currentButtons = new int[lengthPriorities];
		for (int i = 0; i < lengthPriorities; i++) {
			Button tempButton = prefabButton;
			buttons[i] = tempButton;
			currentButtons[i] = 0;
		}
	}

	public void Rob()
	{
		foreach (GameObject enemy in enemiesInRadius) {
			AddGoods(enemy.GetComponent<Person>().AmountOfMoney, enemy.GetComponent<Person>().LookUpLegalItems, enemy.GetComponent<Person>().LookUpIllegalItems);
			enemy.GetComponent<Person>().GetRobbed();
		}
		throw new System.NotImplementedException();
	}

	public void PriorityUpdate(List<GameObject> allGameObjectsInRadius, Collider2D other)
	{
		if (allGameObjectsInRadius.Count > 0) {

			// In case there were no collidings before.
			MainPanel.gameObject.SetActive(true);
			ListPanel.gameObject.SetActive(true);
			Switch.gameObject.SetActive(true);

			int lengthEnum = Enum.GetValues(typeof(priority)).Cast<priority>().Count();
			int[] priorityPresence = new int[lengthEnum];
			for (int i = 0; i < priorityPresence.Length; i++) {
				priorityPresence[i] = 0;
			}

			// Set temp priority to lowest
			int tempPriority = 0;
			int priorityNbr = 0;

			//Check with the tag of the gameObject, which priority it has in the enum,
			// If the priority is higher then the current, update the priority
			for (int i = 0; i < allGameObjectsInRadius.Count; i++) {
				string name = LayerMask.LayerToName(allGameObjectsInRadius[i].layer);
				priority tempP = (priority)Enum.Parse(typeof(priority), name);
				priorityNbr = (int)tempP;
				priorityPresence[priorityNbr] = 1;
				if (priorityNbr > tempPriority) {
					tempPriority = priorityNbr;
				}
			}

      // if there is a new highestpriority, do next lines
			if (tempPriority != highestPriority) {
				highestPriority = tempPriority;

        //Make room for new mainbutton
				if (MainPanel.childCount > 0) {
					GameObject tempB = MainPanel.GetChild(0).gameObject;
					Destroy(tempB);
				}

				Button mainButton = (Button)Instantiate(buttons[highestPriority], transform.position, transform.rotation, MainPanel);
				mainButton.transform.FindChild("Text").GetComponent<Text>().text = buttonNames[highestPriority];
        RectTransform tempPanel = null;

        //names of the panels need to be the same as the priorities & layernames
        for (int j = 0; j < panels.Length; j++)
        {
          if (panels[j].name == ((priority)highestPriority).ToString())
          {
            tempPanel = panels[j];
          }
        }
        mainButton.GetComponent<Button>().onClick.AddListener(() => buttonClicked(tempPanel));
      }

      // For all priorities, check if more buttons are needed in listpanel
			for (int i = priorityPresence.Length - 1; i >= 0; i--) {
        //we dont want mainbutton in the listpanel
				if (i != highestPriority) {
					//Spawn specific priority button
					if (currentButtons[i] == 0 && priorityPresence[i] == 1) {
						Button tempB = (Button)Instantiate(buttons[i], transform.position, transform.rotation, ListPanel);
						tempB.transform.FindChild("Text").GetComponent<Text>().text = buttonNames[i];
            RectTransform tempPanel = null;
            for (int j = 0; j < panels.Length; j++)
            {
              if (panels[j].name == ((priority)i).ToString())
              {
                tempPanel = panels[j];
              }
            }
            if (tempPanel != null)
            {
              tempB.GetComponent<Button>().onClick.AddListener(() => buttonClicked(tempPanel));
            }

            // 1 indicates that the button of the specific priority is present in the listpanel
						currentButtons[i] = 1;
						mNumberOfButtonsInlistPanel++;
					}
					if ((currentButtons[i] == 1 && priorityPresence[i] == 0)) {
						TryToDestroyIndexOfListPanel(i);
					}
				}
				else {
					TryToDestroyIndexOfListPanel(i);
				}

				//Debug.Log("For: " + i + " is currentButtons" + currentButtons[i] + " and priorityPresence " + priorityPresence[i]);
			}
		}
		else {
			MainPanel.gameObject.SetActive(false);
			ListPanel.gameObject.SetActive(false);
			Switch.gameObject.SetActive(false);
		}
	}

	private void TryToDestroyIndexOfListPanel(int index)
	{
		for (int j = 0; j < ListPanel.childCount; j++) {
			if (ListPanel.GetChild(j).GetChild(0).GetComponent<Text>().text == buttonNames[index]) {
        ListPanel.GetChild(j).GetComponent<Button>().onClick.RemoveListener(() => buttonClicked(null));
        Destroy(ListPanel.GetChild(j).gameObject);
				currentButtons[index] = 0;
				mNumberOfButtonsInlistPanel--;
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
    //}
    panel.gameObject.SetActive(true);
  }

	public void OnTriggerEnter2D(Collider2D other)
	{
		allGameObjectsInRadius.Add(other.gameObject);
		if (other.gameObject.layer.ToString() == "Enemy") {
			enemiesInRadius.Add(other.gameObject);
		}

		PriorityUpdate(allGameObjectsInRadius, other);
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		enemiesInRadius.Remove(other.gameObject);
		allGameObjectsInRadius.Remove(other.gameObject);
		PriorityUpdate(allGameObjectsInRadius, other);
	}
}

public enum priority : byte
{
	Team,
	Bank,
	TradingPost,
	Markt,
	Schatkist,
	SchatkistEnemy,
	EnemyPlein,
	Enemy
}
