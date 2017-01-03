using UnityEngine;
using System.Collections;

public class TeamInitializer : MonoBehaviour
{
	private void Awake()
	{
		transform.SetParent(GameManager.s_Singleton.transform.GetChild(0).GetChild(1));
	}
}
