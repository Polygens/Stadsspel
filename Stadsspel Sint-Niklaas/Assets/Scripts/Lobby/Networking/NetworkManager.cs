using UnityEngine;

namespace Stadsspel.Networking
{
	public class NetworkManager : MonoBehaviour
	{
		public static NetworkManager Singleton;

		[SerializeField]
		private TopPanelManager m_TopPanelManager;
		[SerializeField]
		private CreateJoinRoomManager m_CreateJoinRoomManager;
		[SerializeField]
		private RoomManager m_RoomManager;
		[SerializeField]
		private LobbyManager m_LobbyManager;
		[SerializeField]
		private PasswordLoginManager m_PasswordLoginManager;
		[SerializeField]
		private ConnectingManager m_ConnectingManager;
		[SerializeField]
		private CountdownManager m_CountdownManager;
		[SerializeField]
		private KickedManager m_KickedManager;
		[SerializeField]
		private RoomExistsManager m_RoomExistsManager;
		[SerializeField]
		private string m_LobbyPlayerPrefabName;


		public string VERSION {
			get {
				return "v0.0.1";
			}
		}

		public TopPanelManager TopPanelManager {
			get {
				return m_TopPanelManager;
			}
		}

		public CreateJoinRoomManager CreateJoinRoomManager {
			get {
				return m_CreateJoinRoomManager;
			}
		}

		public RoomManager RoomManager {
			get {
				return m_RoomManager;
			}
		}

		public LobbyManager LobbyManager {
			get {
				return m_LobbyManager;
			}
		}

		public PasswordLoginManager PasswordLoginManager {
			get {
				return m_PasswordLoginManager;
			}
		}

		public ConnectingManager ConnectingManager {
			get {
				return m_ConnectingManager;
			}
		}

		public CountdownManager CountdownManager {
			get {
				return m_CountdownManager;
			}
		}

		public KickedManager KickedManager {
			get {
				return m_KickedManager;
			}
		}

		public RoomExistsManager RoomExistsManager {
			get {
				return m_RoomExistsManager;
			}
		}

		public string LobbyPlayerPrefabName {
			get {
				return m_LobbyPlayerPrefabName;
			}
		}

		/// <summary>
		/// Initialises the class. Connects to the server and joins the default lobby.
		/// </summary>
		void Start()
		{
			if(Singleton) {
				Destroy(this);
			}
			else {
				Singleton = this;
			}
			PhotonNetwork.ConnectUsingSettings(NetworkManager.Singleton.VERSION);
			PhotonNetwork.automaticallySyncScene = true;
		}

	}
}