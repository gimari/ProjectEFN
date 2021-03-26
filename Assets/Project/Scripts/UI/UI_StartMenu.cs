using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN.Main {
	public class UI_StartMenu : MonoBehaviour {

		[SerializeField] private GameObject _panel = default;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent(eEventType.OpenStartMenu, this.OpenMenu);
		}

		public void OnClickExit() {
			Global_Common.Quit();
		}

		public void OnClickEnterEscape() {
			Global_Common.LoadScene(eSceneName.SceneGame.ToString());
		}

		public void OnClickTrading() {
			CloseMenu();
			Global_UIEvent.CallUIEvent(eEventType.OpenTradeSelectPanel);
		}

		public void OnClickCharacter() {

		}

		private void CloseMenu() {
			this._panel.SetActive(false);
		}

		private void OpenMenu() {
			this._panel.SetActive(true);
		}
	}
}