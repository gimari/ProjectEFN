using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class UI_IngameInventory : UI_Ingame {

		[SerializeField] private GameObject _panel = default;
		[SerializeField] private GameObject _interactPanel = default;

		// 특정한 인덱스가 정해져 있는 슬롯 리스트.
		[SerializeField] private Graphic_ItemSlot[] _indexedSlotList = null;

		// 유저 백팩 슬롯 리스트.
		[SerializeField] private Graphic_LayoutList _backpackSlotList = default;

		// 루팅 슬롯 리스트.
		[SerializeField] private Graphic_LayoutList _lootSlotList = default;

		private Inventory_Item _interactingInven = null;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent(eEventType.UpdateUserInventory, UpdateUserInventory);
			Global_UIEvent.RegisterUIEvent(eEventType.ToggleIngameInven, ToggleIngameInven);
			Global_UIEvent.RegisterUIEvent<Actor_Base>(eEventType.TryInteractWith, TryInteractWith);
		}

		/// <summary>
		/// 유저 인벤토리 기반으로 업데이트 쳐줌.
		/// </summary>
		public void UpdateUserInventory() {

			// 일단 초기화
			this._interactPanel.SetActive(false);

			foreach (Graphic_ItemSlot slot in _indexedSlotList) {
				slot.ClearImage();
			}

			// selfplayer 가 잇으면?
			if (null != Global_Actor.SelfPlayer) {
				Global_Actor.SelfPlayer.RefreshEquipItem();

				foreach (Graphic_ItemSlot slot in _indexedSlotList) {
					slot.StoredInventory = Global_Actor.SelfPlayer.ActorInventory;
					slot.UpdateItem(Global_Actor.SelfPlayer.ActorInventory.Get(slot.QuickSlotIdx));
				}
			}

			// 인터렉션중인 inven 이 잇으면?
			if (null != _interactingInven) {
				this._interactPanel.SetActive(true);

				_lootSlotList.Init();

				for (int idx = 0; idx < _interactingInven.MaxDisplayIndex; idx++) {
					_lootSlotList.AddWith<Content_ItemSlot>().UpdateItem(_interactingInven.Get(idx), idx, _interactingInven);
				}
			}
		}

		public void TryInteractWith(Actor_Base targetActor) {
			_interactingInven = targetActor.ActorInventory;
			Open();
		}

		public void ToggleIngameInven() {
			if (true == this._panel.activeSelf) {
				Close();
			} else {
				Open();
			}
		}

		public void Close() {
			_interactingInven = null;
			this._panel.SetActive(false);
			this.EndFocus();
		}

		public void Open() {
			this.OnFocus();
			this.UpdateUserInventory();

			this._panel.SetActive(true);
		}
	}
}