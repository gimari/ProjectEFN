using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN.Main {
	public class UI_Trading : MonoBehaviour {

		[SerializeField] private GameObject _panel = default;
		[SerializeField] private Graphic_LayoutList _stashList = default;
		[SerializeField] private Graphic_LayoutList _sellSlotLost = default;
		[SerializeField] private Graphic_FadePop _sellPanelPopup = default;
		[SerializeField] private GameObject _btnDeal = default;
		[SerializeField] private Graphic_NumericText _txtDealExpectResult = default;

		private List<Data_Item> _sellReadyList = new List<Data_Item>();
		private eDealerType _currentDealer = eDealerType.None;

		private const int MAX_SELL_SLOT_COUNT = 25;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent<eDealerType>(eEventType.OpenDealerPanel, OpenDealerPanel);
			Global_UIEvent.RegisterUIEvent(eEventType.UpdateUserInventory, UpdateTradingPanel);
		}

		private void OpenDealerPanel(eDealerType dealerType) {
			_currentDealer = dealerType;
			_sellPanelPopup.gameObject.SetActive(false);

			this.Open();
			InitTradingPanel();
		}

		public void InitTradingPanel() {

			_btnDeal.SetActive(false);
			UpdateTradingPanel();
			_txtDealExpectResult.SetNumericTextWithOutAnimation(0, MoneyFormat.JustComma);

			// 판매 대기 리스트 초기화
			_sellSlotLost.Init();
			for (int sellSlotIdx = 0; sellSlotIdx < MAX_SELL_SLOT_COUNT; sellSlotIdx++) {
				Graphic_CustomSlot slot = _sellSlotLost.AddWith<Graphic_CustomSlot>();
				slot.CustomDropAction = SellSlotDropAction;
				slot.ClearImage();
			}
		}

		public void UpdateTradingPanel() {

			// 유저 스태시 리스트 초기화
			_stashList.Init();
			for (int otherInvenIdx = 0; otherInvenIdx < Global_SelfPlayerData.StashInventory.MaxDisplayIndex; otherInvenIdx++) {
				_stashList.AddWith<Content_ItemSlot>().UpdateItem(Global_SelfPlayerData.StashInventory.Get(otherInvenIdx), otherInvenIdx, Global_SelfPlayerData.StashInventory);
			}
		}

		/// <summary>
		/// sell 패널에 아이템이 올라오면 한개씩 값을 계산해준다.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="slot"></param>
		private void SellSlotDropAction(Graphic_CustomSlot from, Graphic_ItemSlot slot) {
			if (null == from || null == slot) {
				return;
			}

			long resultMoney = 0;
			Status_Dealer dealer = Status_Dealer.GetStatus(_currentDealer);

			slot.IsEnable = false;
			from.UpdateItem(slot.TargetData);
			_sellReadyList.Add(slot.TargetData);

			// 예상되는 판매값 갱신
			foreach (Data_Item item in _sellReadyList) {
				resultMoney += dealer.GetDefaultCost(item.ItemType);
			}

			_txtDealExpectResult.NumberTextAnimate(resultMoney, MoneyFormat.JustComma);
			_btnDeal.SetActive(0 < _sellReadyList.Count);
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

		/// <summary>
		/// DEAL 버튼 눌러서 거래를 시도함.
		/// </summary>
		public void TryDeal() {

			long resultMoney = 0;
			Status_Dealer dealer = Status_Dealer.GetStatus(_currentDealer);

			// 아이템값을 전부 계산해서 더해주자.
			foreach (Data_Item item in _sellReadyList) {
				item.OnDiscard();
				resultMoney += dealer.GetDefaultCost(item.ItemType);
			}

			Global_SelfPlayerData.CokeAmount += resultMoney;

			UpdateTradingPanel();
			OnClickClearDealTable();
		}

		/// <summary>
		/// SELL 패널 밑의 clear 버튼 눌러서 지금 sell table 에 올라가 있는거 전부 없앰
		/// </summary>
		public void OnClickClearDealTable() {
			_sellReadyList.Clear();
			_btnDeal.SetActive(false);
			_txtDealExpectResult.NumberTextAnimate(0, MoneyFormat.JustComma);

			foreach (Transform child in _sellSlotLost) {
				child.GetComponent<Graphic_CustomSlot>().ClearImage();
			}

			foreach (Transform child in _stashList) {
				child.GetComponent<Content_ItemSlot>().IsEnable = true;
			}
		}
	}
}