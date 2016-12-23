using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Person : Element
{
	private List<int> illegalItems = new List<int>();
	//legalItems[(int)Items.diploma] = 10; Bijvoorbeeld
	private List<int> legalItems = new List<int>();

	[SyncVar]
	protected string mName = "player";
	[SyncVar]
	private int mAmountOfMoney = 0;
	private int mCaptureRadius = 35;


	public Person()
	{

	}

	private void Start()
	{
		GetComponent<CircleCollider2D>().radius = mCaptureRadius;
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] newMesh = mesh.vertices;

		newMesh[0] = new Vector3(-mCaptureRadius, -mCaptureRadius);
		newMesh[1] = new Vector3(mCaptureRadius, mCaptureRadius);
		newMesh[2] = new Vector3(mCaptureRadius, -mCaptureRadius);
		newMesh[3] = new Vector3(-mCaptureRadius, mCaptureRadius);
		mesh.SetVertices(newMesh.ToList());

		mesh.RecalculateBounds();
	}

	public void ChangeNameAndID(string playerName, TeamID teamID)
	{
		mName = playerName;
		mTeam = teamID;

		GetComponent<Renderer>().material.color = TeamData.GetColor(mTeam);
		transform.GetChild(0).GetComponent<TextMesh>().text = mName;
	}

	public void UpdatePosition()
	{
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
		legalItems.Clear();
	}

	public void ResetIllegalItems()
	{
		illegalItems.Clear();
	}

	public void GetRobbed()
	{
		mAmountOfMoney = 0;
		ResetLegalItems();
		ResetIllegalItems();
	}

	public List<int> LookUpLegalItems {
		get { return legalItems; }
	}

	public List<int> LookUpIllegalItems {
		get { return illegalItems; }
	}

	public void AddLegalItems(List<int> items)
	{
		for (int i = 0; i < items.Count; i++) {
			legalItems[i] = items[i];
		}
	}

	public void AddIllegalItems(List<int> items)
	{
		for (int i = 0; i < items.Count; i++) {
			illegalItems[i] = items[i];
		}
	}

	public void RemoveMoney(int money)
	{
		mAmountOfMoney -= money;
	}


	public void AddItems(int money)
	{
		mAmountOfMoney += money;
	}

	public void AddGoods(int money, List<int> legalItems, List<int> illegalItems)
	{
		AddItems(money);
		AddLegalItems(legalItems);
		AddIllegalItems(illegalItems);
	}

	public int AmountOfMoney {
		get { return mAmountOfMoney; }
	}
}
