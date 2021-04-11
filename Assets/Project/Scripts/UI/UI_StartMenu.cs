using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN.Main {
	public class UI_StartMenu : MonoBehaviour {

		[SerializeField] private Graphic_FadePop _panel = default;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent(eEventType.OpenStartMenu, this.OpenMenu);
		}

		public void OnClickExit() {
			Global_Common.Quit();

			Global_SelfPlayerData.Save();
		}

		public void OnClickEnterEscape() {
			Global_UIEvent.CallUIEvent<string>(ePermanetEventType.TryChangeScene, eSceneName.SceneGame.ToString());
		}

		public void OnClickTrading() {
			CloseMenu();
			Global_UIEvent.CallUIEvent(eEventType.OpenTradeSelectPanel);
		}

		public void OnClickCharacter() {
			CloseMenu();
			Global_UIEvent.CallUIEvent(eEventType.OpenMainInven);
		}

		private void CloseMenu() {
			this._panel.Hide();
		}

		private void OpenMenu() {
			this._panel.Show();
		}
	}
}