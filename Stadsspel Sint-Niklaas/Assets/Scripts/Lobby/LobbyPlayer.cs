using UnityEngine;
using Photon;
using UnityEngine.UI;

namespace Stadsspel.Networking
{
	public class LobbyPlayer : PunBehaviour
	{
		[SerializeField]
		private Button m_TeamBtn;
		[SerializeField]
		private InputField m_NameInp;
		[SerializeField]
		private Button m_ReadyBtn;
		[SerializeField]
		private Text m_ReadyTxt;
		[SerializeField]
		private Button m_KickPlayerBtn;
		[SerializeField]
		private Text m_IconTxt;

		private Color m_OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
		private Color m_EvenRowColor = new Color(230.0f / 255.0f, 230.0f / 255.0f, 230.0f / 255.0f, 1.0f);
		private Color m_LocalPlayerColor = new Color(245.0f / 255.0f, 132.0f / 255.0f, 42.0f / 255.0f, 0.3f);

		private Color m_ReadyColor = new Color(245.0f / 255.0f, 132.0f / 255.0f, 42.0f / 255.0f, 1.0f);
		private Color m_NotReadyColor = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 1.0f);

		private const string m_HostIcon = "";
		private const string m_LocalPlayerIcon = "";
		private const string m_OtherPlayerIcon = "";

		private const string m_ReadyIcon = "";
		private const string m_NotReadyIcon = "";

		private bool m_IsLocalPlayer = false;
		private bool m_IsMasterClient = false;

		private bool m_IsReady = false;

		public bool IsReady {
			get {
				return m_IsReady;
			}
		}

		private TeamID m_TeamID = TeamID.NotSet;

		[PunRPC]
		private void NameChanged()
		{
			m_NameInp.text = photonView.owner.NickName;
		}

		[PunRPC]
		private void ReadyChanged(bool newReadyState)
		{
			m_IsReady = newReadyState;
			SetReadyButton(m_IsReady);
			if(PhotonNetwork.player.IsMasterClient) {
				NetworkManager.Singleton.RoomManager.CheckIfReadyToStart();
			}
		}

		[PunRPC]
		private void TeamChanged(TeamID newTeam)
		{
			m_TeamID = newTeam;

			m_TeamBtn.GetComponent<Image>().color = TeamData.GetColor(m_TeamID);
		}

		void Start()
		{
			m_IsLocalPlayer = photonView.owner.IsLocal;
			m_IsMasterClient = photonView.owner.IsMasterClient;

			if(m_IsLocalPlayer) {
				SetupLocalPlayer();
			} else {
				SetupOtherPlayer();
			}
				
			transform.SetParent(NetworkManager.Singleton.RoomManager.LobbyPlayerList, false);
			if(m_IsMasterClient) {
				transform.SetAsFirstSibling();
			} else if(m_IsLocalPlayer) {
				transform.SetSiblingIndex(1);
			}

			SetupStyling();
		}

		private void SetupStyling()
		{
			if(m_IsMasterClient) {
				m_IconTxt.text = m_HostIcon;
			} else if(m_IsLocalPlayer) {
				m_IconTxt.text = m_LocalPlayerIcon;
			} else {
				m_IconTxt.text = m_OtherPlayerIcon;
			}
			if(transform.GetSiblingIndex() % 2 == 0) {
				GetComponent<Image>().color = m_EvenRowColor;
			} else {
				GetComponent<Image>().color = m_OddRowColor;
			}

			if(m_IsLocalPlayer) {
				GetComponent<Image>().color = m_LocalPlayerColor;
			}
		}

		private void SetupLocalPlayer()
		{
			GetComponent<Image>().color = m_LocalPlayerColor;

			PhotonNetwork.playerName = "Speler: " + photonView.ownerId;
			m_NameInp.text = PhotonNetwork.playerName;
			photonView.RPC("NameChanged", PhotonTargets.AllBufferedViaServer);

			m_TeamBtn.interactable = true;
			m_TeamBtn.onClick.AddListener(() => {
				//photonView.RPC("TeamChanged", PhotonTargets.AllBufferedViaServer, TeamData.GetNextTeam());
			});
			m_NameInp.interactable = true;
			m_NameInp.onEndEdit.AddListener(val => {
				PhotonNetwork.playerName = val;
				photonView.RPC("NameChanged", PhotonTargets.AllBufferedViaServer);
			});
			if(m_IsMasterClient) {
				photonView.RPC("ReadyChanged", PhotonTargets.AllBufferedViaServer, true);
			} else {
				m_ReadyBtn.interactable = true;
				SetReadyButton(false);
				m_ReadyTxt.gameObject.SetActive(true);
				m_ReadyBtn.onClick.AddListener(() => {
					photonView.RPC("ReadyChanged", PhotonTargets.AllBufferedViaServer, !m_IsReady);
				});
			}
		}

		private void SetupOtherPlayer()
		{
			m_NameInp.text = photonView.owner.NickName;
			SetReadyButton(m_IsReady);

			if(PhotonNetwork.player.IsMasterClient) {
				m_KickPlayerBtn.gameObject.SetActive(true);
				m_KickPlayerBtn.onClick.AddListener(() => {
					PhotonNetwork.CloseConnection(photonView.owner);
				});

			}
		}

		private void SetReadyButton(bool isReady)
		{
			Color btn, textColor;
			string text;
			if(isReady) {
				btn = m_ReadyColor;
				textColor = m_NotReadyColor;
				text = "GEREED";
				m_ReadyTxt.text = m_ReadyIcon;
			} else {
				btn = m_NotReadyColor;
				textColor = m_ReadyColor;
				text = "...";
				m_ReadyTxt.text = m_NotReadyIcon;
			}
			m_ReadyBtn.GetComponent<Image>().color = btn;


			Text textComponent = m_ReadyBtn.transform.GetChild(1).GetComponent<Text>();
			textComponent.text = text;
			m_ReadyTxt.color = textColor;
			textComponent.color = textColor;
		}

		private void OnMasterClientSwitched()
		{
			if(photonView.owner.IsMasterClient) {
				m_IsMasterClient = true;
				transform.SetAsFirstSibling();
				m_IconTxt.text = m_HostIcon;
				m_ReadyTxt.gameObject.SetActive(false);
				photonView.RPC("ReadyChanged", PhotonTargets.AllBufferedViaServer, true);
			}
		}

		private void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			if(PhotonNetwork.player.IsMasterClient) {
				NetworkManager.Singleton.RoomManager.DisableStartButton();
			}
		}
	}
}