using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace Prototype.NetworkLobby
{
    public class LobbyInputPanel : MonoBehaviour
    {
        public Text infoText;
        public Text submitButtonText;
        public Text backButtonText;
        public InputField inputField;
        public Button submitButton;
        public Button backButton;

        private LobbyServerEntry _lobbyServerEntry;

        public void Display(string info, string submitButtonInfo, string backButtonInfo, UnityEngine.Events.UnityAction buttonClbk, LobbyServerEntry lobbyServerEntry)
        {
            infoText.text = info;

            submitButtonText.text = submitButtonInfo;
            backButtonText.text = backButtonInfo;

            _lobbyServerEntry = lobbyServerEntry;

            submitButton.onClick.RemoveAllListeners();
            backButton.onClick.RemoveAllListeners();

            if (buttonClbk != null)
            {
                backButton.onClick.AddListener(buttonClbk);
            }
            submitButton.onClick.AddListener( () => SendInputData() );
            backButton.onClick.AddListener(() => { gameObject.SetActive(false); });

            gameObject.SetActive(true);
        }

        private void SendInputData()
        {
            _lobbyServerEntry.JoinMatch(inputField.text);
        }
    }
}
