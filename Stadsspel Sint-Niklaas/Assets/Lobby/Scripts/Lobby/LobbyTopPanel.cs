using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
	public class LobbyTopPanel : MonoBehaviour
	{
		public bool isInGame = false;
		protected bool isDisplayed = true;

		void Update()
		{
			if (!isInGame)
				return;

			if (Input.GetKeyDown(KeyCode.Escape)) {
				ToggleVisibility(!isDisplayed);
			}

		}

		public void ToggleVisibility(bool visible)
		{
			gameObject.SetActive(visible);
			isDisplayed = visible;
		}
	}
}