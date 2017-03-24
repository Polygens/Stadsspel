using System;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Stadsspel.Networking
{
	/// <summary>
	/// Implements teams in a room/game with help of player properties. Access them by PhotonPlayer.GetTeam extension.
	/// </summary>
	/// <remarks>
	/// Teams are defined by enum Team. Change this to get more / different teams.
	/// There are no rules when / if you can join a team. You could add this in JoinTeam or something.
	/// </remarks>
	///
	public class RoomTeams : Photon.MonoBehaviour
	{
		/// <summary>The main list of teams with their player-lists. Automatically kept up to date.</summary>
		/// <remarks>Note that this is static. Can be accessed by PunTeam.PlayersPerTeam. You should not modify this.</remarks>
		public static Dictionary<TeamID, List<PhotonPlayer>> PlayersPerTeam;

		/// <summary>Defines the player custom property name to use for team affinity of "this" player.</summary>
		public const string TeamPlayerProp = "team";


		#region Events by Unity and Photon


		/// <summary>Needed to update the team lists when joining a room.</summary>
		/// <remarks>Called by PUN. See enum PhotonNetworkingMessage for an explanation.</remarks>
		public void OnJoinedRoom()
		{
			PlayersPerTeam = new Dictionary<TeamID, List<PhotonPlayer>>();
			Array enumVals = Enum.GetValues(typeof(TeamID));

			for(int i = 0; i < TeamData.GetMaxTeams(PhotonNetwork.room.MaxPlayers) + 1; i++) {
				PlayersPerTeam[(TeamID)enumVals.GetValue(i)] = new List<PhotonPlayer>();
			}

			this.UpdateTeams();
		}

		/// <summary>Refreshes the team lists. It could be a non-team related property change, too.</summary>
		/// <remarks>Called by PUN. See enum PhotonNetworkingMessage for an explanation.</remarks>
		public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
		{
			this.UpdateTeams();
		}

		public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			this.UpdateTeams();
		}

		public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
			this.UpdateTeams();
		}

		#endregion


		public void UpdateTeams()
		{
			Array enumVals = Enum.GetValues(typeof(TeamID));

			for(int i = 0; i < TeamData.GetMaxTeams(PhotonNetwork.room.MaxPlayers) + 1; i++) {
				PlayersPerTeam[(TeamID)enumVals.GetValue(i)].Clear();
			}

			for(int i = 0; i < PhotonNetwork.playerList.Length; i++) {
				PhotonPlayer player = PhotonNetwork.playerList[i];
				TeamID playerTeam = player.GetTeam();
				PlayersPerTeam[playerTeam].Add(player);
			}
		}
	}
}

namespace Stadsspel.Networking
{
	/// <summary>Extension used for PunTeams and PhotonPlayer class. Wraps access to the player's custom property.</summary>
	public static class TeamExtensions
	{
		/// <summary>Extension for PhotonPlayer class to wrap up access to the player's custom property.</summary>
		/// <returns>PunTeam.Team.none if no team was found (yet).</returns>
		public static TeamID GetTeam(this PhotonPlayer player)
		{
			object teamId;
			if(player.CustomProperties.TryGetValue(RoomTeams.TeamPlayerProp, out teamId)) {
				return (TeamID)teamId;
			}

			return TeamID.NotSet;
		}

		/// <summary>Switch that player's team to the one you assign.</summary>
		/// <remarks>Internally checks if this player is in that team already or not. Only team switches are actually sent.</remarks>
		/// <param name="player"></param>
		/// <param name="team"></param>
		public static void SetTeam(this PhotonPlayer player, TeamID team)
		{
#if(UNITY_EDITOR)
			if(!PhotonNetwork.connectedAndReady) {
				Debug.LogWarning("JoinTeam was called in state: " + PhotonNetwork.connectionStateDetailed + ". Not connectedAndReady.");
				return;
			}
#endif

			TeamID currentTeam = player.GetTeam();
			if(currentTeam != team) {
				player.SetCustomProperties(new Hashtable() { { RoomTeams.TeamPlayerProp, (byte)team } });
			}
		}

		public static void RequestTeam(this PhotonPlayer player)
		{
#if(UNITY_EDITOR)
			if(!PhotonNetwork.connectedAndReady) {
				Debug.LogWarning("JoinTeam was called in state: " + PhotonNetwork.connectionStateDetailed + ". Not connectedAndReady.");
				return;
			}
#endif

			TeamID newTeam = player.GetTeam();
			int maxIterations = TeamData.GetMaxTeams(PhotonNetwork.room.MaxPlayers);

			do {
				if(maxIterations <= 0) {
					newTeam = TeamID.NotSet;
					break;
				}
				maxIterations--;

				newTeam = TeamData.GetNextTeam(newTeam);
#if(UNITY_EDITOR)
				Debug.Log(newTeam);
#endif
			} while(RoomTeams.PlayersPerTeam[newTeam].Count >= TeamData.GetMaxPlayersPerTeam(PhotonNetwork.room.MaxPlayers));

			if(newTeam == TeamID.NotSet) {
#if(UNITY_EDITOR)
				Debug.Log("ERROR: No empty team found!");
#endif
			}
			else {
				player.SetCustomProperties(new Hashtable() { { RoomTeams.TeamPlayerProp, (byte)newTeam } });
			}
		}
	}
}
