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

        public void Display(string info, string submitButtonInfo, string backButtonInfo, UnityEngine.Events.UnityAction buttonClbk)
        {
            infoText.text = info;

            backButtonText.text = backButtonInfo;

            submitButton.onClick.RemoveAllListeners();

            if (buttonClbk != null)
            {
                submitButton.onClick.AddListener(buttonClbk);
            }

            backButton.onClick.AddListener(() => { gameObject.SetActive(false); });

            gameObject.SetActive(true);
        }
    }
}
