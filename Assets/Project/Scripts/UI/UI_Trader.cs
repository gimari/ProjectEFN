using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Main {
	public class UI_Trader : MonoBehaviour {

		[SerializeField] private GameObject _panel = default;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent(eEventType.OpenTradeSelectPanel, OpenTradeSelectPanel);
		}

		public void OpenTradeSelectPanel() {
			this._panel.SetActive(true);
		}

		public void Close() {
			this._panel.SetActive(false);
		}

		public void OnClickBack() {
			this.Close();
			Global_UIEvent.CallUIEvent(eEventType.OpenStartMenu);
		}

		public void OnClickDealer(int dealerType) {
			this.Close();
			Global_UIEvent.CallUIEvent<eDealerType>(eEventType.OpenDealerPanel, (eDealerType)dealerType);
		}
	}
}