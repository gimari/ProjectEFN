using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN.Main {
	public class UI_Trading : MonoBehaviour {

		[SerializeField] private GameObject _panel = default;

		[SerializeField] private Graphic_LayoutList _stashList = default;
		[SerializeField] private Graphic_LayoutList _sellSlotLost = default;
		[SerializeField] private Graphic_LayoutList _dealerList = default;

		[SerializeField] private Graphic_FadePop _sellPanelPopup = default;
		[SerializeField] private GameObject _btnDeal = default;
		[SerializeField] private Graphic_NumericText _txtDealExpectResult = default;

		[Header("BuyPanel")]
		[SerializeField] private Graphic_FadePop _buyPanelPopup = default;
		[SerializeField] private Graphic_ItemSlot _buyTargetSlot = default;
		[SerializeField] private Graphic_NumericText _buyCokeCount = default;
		[SerializeField] private InputField _txtBuyCount = default;

		[Header("DealerInfo")]

		[EnumNamedArray(typeof(eDealerType))]
		[SerializeField] private GameObject[] _dealerInfoObject = default;

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
			_buyPanelPopup.gameObject.SetActive(false);

			for (int idx = 0; idx < _dealerInfoObject.Length; idx++) {
				if (null != _dealerInfoObject[idx]) {
					_dealerInfoObject[idx].SetActive(idx == (int)dealerType);
				}
			}

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

			Status_Dealer dealer = Status_Dealer.GetStatus(_currentDealer);

			// 딜러 리스트 초기화
			_dealerList.Init();
			for (int dealerIdx = 0; dealerIdx < dealer.DealerInven.MaxDisplayIndex; dealerIdx++) {
				Content_ItemSlot slot = _dealerList.AddWith<Content_ItemSlot>();

				slot.TargetSlot.QuickSlotIdx = dealerIdx;
				slot.TargetSlot.UpdateItem(dealer.DealerInven.Get(dealerIdx));
				slot.TargetSlot.StoredInventory = dealer.DealerInven;
				slot.TargetSlot.OnRightClickAction = OnRightClickDealerInven;
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
				resultMoney += item.StackCount * dealer.GetDefaultCost(item.ItemType);
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
				resultMoney += item.StackCount * dealer.GetDefaultCost(item.ItemType);
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

		public void OnClickBuyBtn(Data_Item item) {
			_buyPanelPopup.Show();

			_buyTargetSlot.UpdateItem(item);

			_txtBuyCount.text = "1";
			OnModifyCount("1");
		}

		public void OnClickCloseBuyPanel() {
			_buyPanelPopup.Hide();
		}

		/// <summary>
		/// buy 패널에서 confirm 버튼을 눌러가지고 지금 올라간 아이템을 살려고 시도함
		/// </summary>
		public void OnClickConfirmBuy() {

			int count = 0;
			int.TryParse(_txtBuyCount.text, out count);

			// 개수가 0임..
			if (0 == count) {
				_buyPanelPopup.Hide();
				return;
			}

			// 살려는게 없음..
			if (null == this._buyTargetSlot.TargetData) {
				_buyPanelPopup.Hide();
				return;
			}

			// 너무 많음..
			if (Global_SelfPlayerData.StashInventory.GetRemainEmptySlotCount() < count) {
				Global_UIEvent.CallUIEvent(ePermanetEventType.ShowNakMsg, "스태시에 빈 자리가 없습니다!");
				return;
			}

			// 실제 돈을 차감해보고 정상적으로 반영되었는지 확인
			Status_Dealer dealer = Status_Dealer.GetStatus(_currentDealer);
			long resultMoney = count * this._buyTargetSlot.TargetData.StackCount * this._buyTargetSlot.TargetData.StatusData.DefaultPrice;
			if (Global_SelfPlayerData.ConsumeCoke(resultMoney) != eErrorCode.Success) {
				Global_UIEvent.CallUIEvent(ePermanetEventType.ShowNakMsg, "COKE 가 충분하지 않습니다!");
				return;
			}

			// 여기까지 하면 모든 처리가 끝났음. 아이템 실제 생성해주고 추가한다.
			for (; 0 < count; count--) {
				Data_Item addedItem = new Data_Item(this._buyTargetSlot.TargetData.ItemType);
				addedItem.StackCount = this._buyTargetSlot.TargetData.StackCount;

				Global_SelfPlayerData.StashInventory.AddInventory(addedItem);
			}

			_buyPanelPopup.Hide();
		}

		/// <summary>
		/// 딜러 인벤토리에서 우클릭을한다면
		/// BUY : 구매
		/// 의 행동이 가능해야 한다.
		/// </summary>
		public void OnRightClickDealerInven(Graphic_ItemSlot clickedSlot) {
			ModifyPanelData mpd = new ModifyPanelData();

			ModifyPanelInfo info = new ModifyPanelInfo();
			info.BtnName = "BUY";
			info.OnClickAction = () => {
				OnClickBuyBtn(clickedSlot.TargetData);
			};

			mpd.InfoList.Add(info);

			Global_UIEvent.CallUIEvent(eEventType.TryModifySlot, mpd);
		}

		public void OnModifyCount(string input) {

			int count = 0;
			int.TryParse(input, out count);

			if (null == this._buyTargetSlot.TargetData) {
				return;
			}

			Status_Dealer dealer = Status_Dealer.GetStatus(_currentDealer);
			long resultMoney = count * this._buyTargetSlot.TargetData.StackCount * this._buyTargetSlot.TargetData.StatusData.DefaultPrice;

			_buyCokeCount.NumberTextAnimate(resultMoney, MoneyFormat.JustComma);
		}
	}
}