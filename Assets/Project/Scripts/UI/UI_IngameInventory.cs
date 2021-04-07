using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class UI_IngameInventory : UI_Ingame {

		[SerializeField] private Graphic_FadePop _panel = default;
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
			Global_UIEvent.RegisterUIEvent(eEventType.ToggleIngameInven, ToggleIngameInven);
			Global_UIEvent.RegisterUIEvent<Actor_Base>(eEventType.TryInteractWith, TryInteractWith);
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
			if (null != Global_Actor.SelfPlayer) {
				Global_Actor.SelfPlayer.RefreshEquipItem();

				int selfinvenCount = Global_Actor.SelfPlayer.ActorInventory.MaxDisplayIndex;
				int idx = (int)ePlayerSlotType.PrimeWeapon;

				// 퀵슬롯 부분부터 갱신
				for (; idx < selfinvenCount && idx < (int)ePlayerSlotType.BackpackSlotStart; idx++) {
					Content_ItemSlot slot = _indexedSlotList[idx - (int)ePlayerSlotType.PrimeWeapon];

					slot.gameObject.SetActive(true);
					slot.TargetSlot.QuickSlotIdx = idx;
					slot.TargetSlot.UpdateItem(Global_Actor.SelfPlayer.ActorInventory.Get(idx));
					slot.TargetSlot.StoredInventory = Global_Actor.SelfPlayer.ActorInventory;
					slot.TargetSlot.OnRightClickAction = OnRightClickUserInven;
				}

				// 퀵슬롯 다음 부분 갱신
				for (; idx < selfinvenCount; idx++) {
					Content_ItemSlot slot = _backpackSlotList.AddWith<Content_ItemSlot>();

					slot.TargetSlot.QuickSlotIdx = idx;
					slot.TargetSlot.UpdateItem(Global_Actor.SelfPlayer.ActorInventory.Get(idx));
					slot.TargetSlot.StoredInventory = Global_Actor.SelfPlayer.ActorInventory;
					slot.TargetSlot.OnRightClickAction = OnRightClickUserInven;
				}
			}

			// 인터렉션중인 inven 이 잇으면?
			if (null != _interactingInven) {
				this._interactPanel.SetActive(true);

				_lootSlotList.Init();

				for (int idx = 0; idx < _interactingInven.MaxDisplayIndex; idx++) {
					Content_ItemSlot slot = _lootSlotList.AddWith<Content_ItemSlot>();

					slot.TargetSlot.QuickSlotIdx = idx;
					slot.TargetSlot.UpdateItem(_interactingInven.Get(idx));
					slot.TargetSlot.StoredInventory = _interactingInven;
					slot.TargetSlot.OnRightClickAction = OnRightClickInteractInven;
				}
			}
		}

		/// <summary>
		/// 유저 인게임 인벤토리에서 우클릭을 한다면 
		/// DISCARD : 아이템 부수기
		/// 의 행동이 가능해야 한다.
		/// </summary>
		public void OnRightClickUserInven(Graphic_ItemSlot clickedSlot) {
			ModifyPanelData mpd = new ModifyPanelData();

			ModifyPanelInfo info = new ModifyPanelInfo();
			info.BtnName = "DISCARD";
			info.OnClickAction = () => {
				clickedSlot.TargetData.OnDiscard();
			};

			mpd.InfoList.Add(info);

			Global_UIEvent.CallUIEvent(eEventType.TryModifySlot, mpd);
		}

		/// <summary>
		/// 다른 인벤과 같이 열었을 때 다른 인벤에서 우클릭을 한다면
		/// COLLECT : 내 인벤으로 획득
		/// 의 행동이 가능해야 한다.
		/// </summary>
		public void OnRightClickInteractInven(Graphic_ItemSlot clickedSlot) {
			ModifyPanelData mpd = new ModifyPanelData();

			ModifyPanelInfo info = new ModifyPanelInfo();
			info.BtnName = "COLLECT";
			info.OnClickAction = () => {
				Global_Actor.SelfPlayer.ActorInventory.AddInventory(clickedSlot.TargetData);
			};

			mpd.InfoList.Add(info);

			Global_UIEvent.CallUIEvent(eEventType.TryModifySlot, mpd);
		}

		public void TryInteractWith(Actor_Base targetActor) {
			_interactingInven = targetActor.ActorInventory;
			Open();
		}

		public void ToggleIngameInven() {
			if (true == this._panel.gameObject.activeSelf) {
				Close();
			} else {
				Open();
			}
		}

		public void Close() {
			_interactingInven = null;
			this._panel.Hide();
			this.EndFocus();
		}

		public void Open() {
			this.OnFocus();
			this.UpdateUserInventory();

			this._panel.Show();
		}
	}
}