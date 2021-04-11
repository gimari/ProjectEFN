using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Main {
	public class UI_GameResult : MonoBehaviour {

		[SerializeField] private Graphic_FadePop _panel = default;
		[SerializeField] private GameObject _panelKIA = default;
		[SerializeField] private GameObject _panelEscape = default;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent(eEventType.OpenGameResult, this.OpenMenu);
		}

		public void CloseMenu() {
			this._panel.Hide();
			Global_UIEvent.CallUIEvent(eEventType.OpenStartMenu);
		}

		public void OpenMenu() {

			if (null == Global_SelfPlayerData.GameEndData) {
				Global_UIEvent.CallUIEvent(eEventType.OpenStartMenu);
				return;
			}

			_panelKIA.SetActive(Global_SelfPlayerData.GameEndData.IsKIA);
			_panelEscape.SetActive(!Global_SelfPlayerData.GameEndData.IsKIA);

			this._panel.Show();
		}
	}
}