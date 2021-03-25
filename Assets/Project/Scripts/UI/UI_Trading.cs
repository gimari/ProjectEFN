using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Main {
	public class UI_Trading : MonoBehaviour {

		[SerializeField] private GameObject _panel = default;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent<eDealerType>(eEventType.OpenDealerPanel, OpenDealerPanel);
		}

		private void OpenDealerPanel(eDealerType dealerType) {
			this.Open();
		}

		public void OnClickBack() {
			this.Close();
			Global_UIEvent.CallUIEvent(eEventType.OpenTradeSelectPanel);
		}

		private void Open() {
			this._panel.SetActive(true);
		}

		private void Close() {
			this._panel.SetActive(false);
		}
	}
}