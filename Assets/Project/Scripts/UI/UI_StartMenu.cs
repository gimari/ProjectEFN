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
			Global_UIEvent.CallUIEvent(eEventType.OnShowDeploy);
		}

		public void OnClickTrading() {
			CloseMenu();
			Global_UIEvent.CallUIEvent(eEventType.OpenTradeSelectPanel);
		}

		public void OnClickCharacter() {
			CloseMenu();
			Global_UIEvent.CallUIEvent(eEventType.OpenMainInven);
		}

		public void OnClickSetting() {
			CloseMenu();
			Global_UIEvent.CallUIEvent(eEventType.OpenSetting);
		}

		private void CloseMenu() {
			this._panel.Hide();
		}

		private void OpenMenu() {
			this._panel.Show();
		}

		public void OnClickResetData() {
			MessageData data = new MessageData();
			data.Context = "기록된 모든 데이터가 초기화됩니다!! 정말 계속하시겠습니까?";
			data.OnClickOkFunc = OnResetData;

			Global_UIEvent.CallUIEvent(ePermanetEventType.ShowMessage, data);
		}

		public void OnResetData() {
			PlayerPrefs.DeleteAll();
			Global_Common.Quit();
		}
	}
}