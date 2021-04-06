using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Main {
	public class UI_Trading : MonoBehaviour {

		[SerializeField] private GameObject _panel = default;
		[SerializeField] private Graphic_LayoutList _stashList = default;
		[SerializeField] private Graphic_LayoutList _sellSlotLost = default;
		[SerializeField] private Graphic_FadePop _sellPanelPopup = default;

		private List<Data_Item> _sellReadyList = new List<Data_Item>();
		private const int MAX_SELL_SLOT_COUNT = 25;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent<eDealerType>(eEventType.OpenDealerPanel, OpenDealerPanel);
			Global_UIEvent.RegisterUIEvent(eEventType.UpdateUserInventory, UpdateTradingPanel);
		}

		private void OpenDealerPanel(eDealerType dealerType) {
			this.Open();
			UpdateTradingPanel();
			OnClickBuy();
		}

		public void UpdateTradingPanel() {

			// 유저 스태시 리스트 초기화
			_stashList.Init();
			for (int otherInvenIdx = 0; otherInvenIdx < Global_SelfPlayerData.StashInventory.MaxDisplayIndex; otherInvenIdx++) {
				_stashList.AddWith<Content_ItemSlot>().UpdateItem(Global_SelfPlayerData.StashInventory.Get(otherInvenIdx), otherInvenIdx, Global_SelfPlayerData.StashInventory);
			}

			// 판매 대기 리스트 초기화
			_sellSlotLost.Init();
			for (int sellSlotIdx = 0; sellSlotIdx < MAX_SELL_SLOT_COUNT; sellSlotIdx++) {
				_sellSlotLost.AddWith<Content_ItemSlot>().UpdateItem(null, sellSlotIdx, null);
			}
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

		public void OnClickSell() {
			_sellPanelPopup.Show();
		}

		public void OnClickBuy() {
			_sellPanelPopup.Hide();
		}

		public void TryDeal() {

		}
	}
}