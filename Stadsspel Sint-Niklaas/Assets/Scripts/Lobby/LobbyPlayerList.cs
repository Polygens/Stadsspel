using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Prototype.NetworkLobby
{
	//List of players in the lobby
	public class LobbyPlayerList : MonoBehaviour
	{
		public static LobbyPlayerList _instance = null;

		[SerializeField]
		public RectTransform playerListContentTransform;
		[SerializeField]
		private GameObject warningDirectPlayServer;

		protected VerticalLayoutGroup _layout;

		private LobbyPlayer[,] mLobbyPlayerMatrix;
		public int AmountOfTeams { get { return mLobbyPlayerMatrix.GetLength(0); } }
		public int MaxPlayers { get { return mLobbyPlayerMatrix.GetLength(1); } }
		public int AmountOfPlayersInLobby { get; private set; }

		public LobbyPlayer[,] LobbyPlayerMatrix {
			get {
				return mLobbyPlayerMatrix;
			}
		}

		public void OnEnable()
		{
			_instance = this;
			_layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();

			UpdateTeamsAndMaxPlayers(LobbyManager.s_Singleton.GetTeams(), LobbyManager.s_Singleton.GetPlayers());
		}

		public void DisplayDirectServerWarning(bool enabled)
		{
			if (warningDirectPlayServer != null)
				warningDirectPlayServer.SetActive(enabled);
		}

		void Update()
		{
			//this dirty the layout to force it to recompute every frame (a sync problem between client/server
			//sometime to child being assigned before layout was enabled/init, leading to broken layouting)

			if (_layout)
				_layout.childAlignment = Time.frameCount % 2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
		}

		public bool AddPlayer(LobbyPlayer player)
		{
			if (player.mPlayerTeam == TeamID.NotSet) {
				return false;
			}
			bool result = false;
			for (int i = 0; i < mLobbyPlayerMatrix.GetLength(1); i++) {
				if (mLobbyPlayerMatrix[(int)player.mPlayerTeam - 1, i] == null) {
					mLobbyPlayerMatrix[(int)player.mPlayerTeam - 1, i] = player;
					result = true;
				}
			}

			if (result) {
				player.transform.SetParent(playerListContentTransform, false);
				PlayerListModified();
			}

			return result;
		}

		public bool RemovePlayer(LobbyPlayer player)
		{
			bool result = false;
			for (int i = 0; i < mLobbyPlayerMatrix.GetLength(1); i++) {
				if (mLobbyPlayerMatrix[(int)player.mPlayerTeam - 1, i] == player) {
					mLobbyPlayerMatrix[(int)player.mPlayerTeam - 1, i] = null;
					result = true;
				}
			}
			if (result) {
				PlayerListModified();
			}
			return result;
		}

		public void PlayerListModified()
		{
			int i = 0;
			foreach (LobbyPlayer p in mLobbyPlayerMatrix) {
				if (p != null) {
					p.OnPlayerListChanged(i++);
				}
			}
			AmountOfPlayersInLobby = i;
		}

		private void UpdateTeamsAndMaxPlayers(int amountOfTeams, int maxPlayers)
		{
			Debug.Log("Lobby made for " + amountOfTeams + " teams with " + maxPlayers + " players.");
			mLobbyPlayerMatrix = new LobbyPlayer[amountOfTeams, maxPlayers];
		}

		public bool IsTeamFull(TeamID teamID)
		{
			for (int i = 0; i < mLobbyPlayerMatrix.GetLength(1); i++) {
				if (mLobbyPlayerMatrix[(int)teamID - 1, i] == null) {
					return false;
				}
			}
			return true;
		}
	}
}
