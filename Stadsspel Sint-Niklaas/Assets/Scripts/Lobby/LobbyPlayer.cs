using ExitGames.Client.Photon;
using Photon;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace Stadsspel.Networking
{
	public class LobbyPlayer : MonoBehaviour
	{
		[SerializeField]
		private Button m_TeamBtn;
		[SerializeField]
		private RectTransform m_NameInpRect;
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

		public Text m_playerNameText;

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

		/// <summary>
		///  Updates the lobbyplayer name text field.
		/// </summary>
		//private void NameChanged(string name)
		//{
		//	m_Name.text = name;
		//}

		/// <summary>
		///  Updates the player's ready state, ready button and checks if the game can start.
		/// </summary>
		private void ReadyChanged(bool newReadyState)
		{
			m_IsReady = newReadyState;
			SetReadyButton(m_IsReady);
		}

		/// <summary>
		/// Sets up the ui of the lobbyplayer depending on the type.
		/// </summary>
		private void SetupStyling()
		{
			if (m_IsMasterClient)
			{
				m_IconTxt.text = m_HostIcon;
			} else if (m_IsLocalPlayer)
			{
				m_IconTxt.text = m_LocalPlayerIcon;
			} else
			{
				m_IconTxt.text = m_OtherPlayerIcon;
			}
			if (transform.GetSiblingIndex() % 2 == 0)
			{
				GetComponent<Image>().color = m_EvenRowColor;
			} else
			{
				GetComponent<Image>().color = m_OddRowColor;
			}

			if (m_IsLocalPlayer)
			{
				GetComponent<Image>().color = m_LocalPlayerColor;
			}
		}

		public void Initialise(ServerTeam team, ServerPlayer player)
		{
			if (player.clientID.Equals(CurrentGame.Instance.LocalPlayer.clientID))
			{
				SetupLocalPlayer(team, player);
				m_IsLocalPlayer = true;
			} else
			{
				SetupOtherPlayer(team, player);
				m_IsLocalPlayer = false;
			}

			m_IsMasterClient = CurrentGame.Instance.isHost && CurrentGame.Instance.LocalPlayer.ClientId.Equals(player.ClientId);

			if (m_IsMasterClient)
			{
				transform.SetAsFirstSibling();
			} else if (m_IsLocalPlayer)
			{
				transform.SetSiblingIndex(1);
			}

			SetupStyling();
		}

		public string LoadEncodedName()
		{
			string path = Application.persistentDataPath;
			try
			{
				return File.ReadAllText(path + "/" + "StadsspelSpelerNaam" + ".txt");
			}
			catch (System.Exception)
			{
				return "";
				throw;
			}
		}

		/// <summary>
		/// Gets called when the player is the local player. Sets up the lobbyplayer behaviour.
		/// </summary>
		private void SetupLocalPlayer(ServerTeam team, ServerPlayer player)
		{
			Color c = new Color(0, 0, 0);
			ColorUtility.TryParseHtmlString(team.customColor, out c);

			m_TeamBtn.GetComponent<Image>().color = c;

			GetComponent<Image>().color = m_LocalPlayerColor;

			string namePlayer = LoadEncodedName();
			m_playerNameText.text = namePlayer;
			player.name = namePlayer;

			m_TeamBtn.interactable = true;
			m_TeamBtn.onClick.AddListener(() =>
			{
				ServerTeam nextTeam = CurrentGame.Instance.getNextTeam(team);
				CurrentGame.Instance.Ws.SendPlayerTeamUpdate(nextTeam.teamName);
			});

			//m_Name.interactable = true;
			//m_Name.onEndEdit.AddListener(val =>
			//{
			//	CurrentGame.Instance.Ws.SendPlayerNameUpdate(val);
			//});

			m_KickPlayerBtn.interactable = true;
			m_KickPlayerBtn.onClick.AddListener(() => {
				//todo change ready state G: is this needed in server?
			});

		}

		/// <summary>
		/// Gets called when the player is not a local player. Sets up the lobbyplayer behaviour.
		/// </summary>
		private void SetupOtherPlayer(ServerTeam team, ServerPlayer player)
		{
			Color c = new Color(0, 0, 0);
			ColorUtility.TryParseHtmlString(team.customColor, out c);

			m_TeamBtn.GetComponent<Image>().color = c;
			m_playerNameText.text = player.Name;
			SetReadyButton(m_IsReady);
			
			if (CurrentGame.Instance.isHost)
			{
				m_NameInpRect.offsetMax = new Vector2(-150, m_NameInpRect.offsetMax.y);
				m_KickPlayerBtn.gameObject.SetActive(true);
				m_KickPlayerBtn.onClick.AddListener(() =>
				{
					Rest.KickPlayer(CurrentGame.Instance.GameId, CurrentGame.Instance.HostingLoginToken, player.clientID);
				});
			}
		}

		/// <summary>
		/// Handles the styling of the ready button when toggled on or off based on passed parameter.
		/// </summary>
		private void SetReadyButton(bool isReady)
		{
			Color btn, textColor;
			string text;
			if (isReady)
			{
				btn = m_ReadyColor;
				textColor = m_NotReadyColor;
				text = "GEREED";
				m_ReadyTxt.text = m_ReadyIcon;
			} else
			{
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
	}
}
