using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Main {
    public class UI_MainInventory : MonoBehaviour {

		[SerializeField] private GameObject _panel = default;
		[SerializeField] private GameObject _interactPanel = default;

		// 특정한 인덱스가 정해져 있는 슬롯 리스트.
		[SerializeField] private Content_ItemSlot[] _indexedSlotList = null;

		// 유저 백팩 슬롯 리스트.
		[SerializeField] private Graphic_LayoutList _backpackSlotList = default;

		// 루팅 슬롯 리스트.
		[SerializeField] private Graphic_LayoutList _lootSlotList = default;

		private Inventory_Item _interactingInven = null;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent(eEventType.UpdateUserInventory, UpdateUserInventory);
			Global_UIEvent.RegisterUIEvent(eEventType.OpenMainInven, OpenMainInven);
		}

		/// <summary>
		/// 메인화면에서 그냥 열면 유저 스태시랑 같이연다
		/// </summary>
		public void OpenMainInven() {
			_interactingInven = Global_SelfPlayerData.StashInventory;
			this.Open();
		}
		
		/// <summary>
		/// 유저 인벤토리 기반으로 업데이트 쳐줌.
		/// </summary>
		public void UpdateUserInventory() {

			// 일단 초기화
			this._interactPanel.SetActive(false);

			this._backpackSlotList.Init();
			foreach (Content_ItemSlot slot in _indexedSlotList) {
				slot.ClearImage();
				slot.gameObject.SetActive(false);
			}

			// selfplayer 가 잇으면?
			int selfinvenCount = Global_SelfPlayerData.SelfInventory.MaxDisplayIndex;
			int idx = (int)ePlayerSlotType.PrimeWeapon;

			// 퀵슬롯 부분부터 갱신
			for (; idx < selfinvenCount && idx < (int)ePlayerSlotType.BackpackSlotStart; idx++) {
				_indexedSlotList[idx - (int)ePlayerSlotType.PrimeWeapon].gameObject.SetActive(true);

				_indexedSlotList[idx - (int)ePlayerSlotType.PrimeWeapon].UpdateItem(
					Global_SelfPlayerData.SelfInventory.Get(idx), idx, Global_SelfPlayerData.SelfInventory);
			}

			// 퀵슬롯 다음 부분 갱신
			for (; idx < selfinvenCount; idx++) {
				Content_ItemSlot content = _backpackSlotList.AddWith<Content_ItemSlot>();
				content.UpdateItem(Global_SelfPlayerData.SelfInventory.Get(idx), idx, Global_SelfPlayerData.SelfInventory);
			}

			// 인터렉션중인 inven 이 잇으면?
			if (null != _interactingInven) {
				this._interactPanel.SetActive(true);

				_lootSlotList.Init();

				for (int otherInvenIdx = 0; otherInvenIdx < _interactingInven.MaxDisplayIndex; otherInvenIdx++) {
					_lootSlotList.AddWith<Content_ItemSlot>().UpdateItem(_interactingInven.Get(otherInvenIdx), otherInvenIdx, _interactingInven);
				}
			}
		}

		public void Close() {
			this._panel.SetActive(false);
		}

		public void Open() {
			this.UpdateUserInventory();
			this._panel.SetActive(true);
		}

		public void OnClickBack() {
			this.Close();
			Global_UIEvent.CallUIEvent(eEventType.OpenStartMenu);
		}
	}
}