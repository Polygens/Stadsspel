using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

namespace Prototype.NetworkLobby
{
	public class LobbyServerList : MonoBehaviour
	{
		public LobbyManager lobbyManager;

		public RectTransform serverListRect;
		public GameObject serverEntryPrefab;
		public GameObject noServerFound;

		static Color OddServerColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		static Color EvenServerColor = new Color(.94f, .94f, .94f, 1.0f);

		private float mRefreshTime = 5f;
		private float mTimer;

		private void OnEnable()
		{
			mTimer = mRefreshTime;

			foreach (Transform t in serverListRect)
				Destroy(t.gameObject);

			noServerFound.SetActive(false);

			UpdateList();
		}

		private void Update()
		{
			mTimer -= Time.deltaTime;
			if (mTimer < 0) {
				mTimer = mRefreshTime;
				UpdateList();
			}
		}

		public void OnGUIMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
		{
			if (matches.Count == 0) {
				noServerFound.SetActive(true);
				return;
			}

			noServerFound.SetActive(false);
			foreach (Transform t in serverListRect)
				Destroy(t.gameObject);

			for (int i = 0; i < matches.Count; ++i) {
				GameObject o = Instantiate(serverEntryPrefab) as GameObject;

				o.GetComponent<LobbyServerEntry>().Populate(matches[i], lobbyManager, (i % 2 == 0) ? OddServerColor : EvenServerColor);

				o.transform.SetParent(serverListRect, false);
			}
		}

		public void UpdateList()
		{
			lobbyManager.matchMaker.ListMatches(0, 1000, "", false, 0, 0, OnGUIMatchList);
		}
	}
}