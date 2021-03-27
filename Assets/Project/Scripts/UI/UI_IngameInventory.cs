using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class UI_IngameInventory : UI_Ingame {

		[SerializeField] private GameObject _panel = default;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent(eEventType.ToggleIngameInven, ToggleIngameInven);
		}

		public void ToggleIngameInven() {
			if (true == this._panel.activeSelf) {
				Close();
			} else {
				Open();
			}
		}

		public void Close() {
			this._panel.SetActive(false);
			this.EndFocus();
		}

		public void Open() {
			this.OnFocus();
			this._panel.SetActive(true);
		}
	}
}