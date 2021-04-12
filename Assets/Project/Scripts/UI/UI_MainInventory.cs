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
				Content_ItemSlot slot = _indexedSlotList[idx - (int)ePlayerSlotType.PrimeWeapon];

				slot.gameObject.SetActive(true);
				slot.TargetSlot.QuickSlotIdx = idx;
				slot.TargetSlot.UpdateItem(Global_SelfPlayerData.SelfInventory.Get(idx));
				slot.TargetSlot.StoredInventory = Global_SelfPlayerData.SelfInventory;
				slot.TargetSlot.OnRightClickAction = OnRightClickUserInven;
			}

			// 퀵슬롯 다음 부분 갱신
			for (; idx < selfinvenCount; idx++) {
				Content_ItemSlot slot = _backpackSlotList.AddWith<Content_ItemSlot>();

				slot.TargetSlot.QuickSlotIdx = idx;
				slot.TargetSlot.UpdateItem(Global_SelfPlayerData.SelfInventory.Get(idx));
				slot.TargetSlot.StoredInventory = Global_SelfPlayerData.SelfInventory;
				slot.TargetSlot.OnRightClickAction = OnRightClickUserInven;
			}

			// 인터렉션중인 inven 이 잇으면?
			if (null != _interactingInven) {
				this._interactPanel.SetActive(true);

				_lootSlotList.Init();

				for (int otherInvenIdx = 0; otherInvenIdx < _interactingInven.MaxDisplayIndex; otherInvenIdx++) {
					Content_ItemSlot slot = _lootSlotList.AddWith<Content_ItemSlot>();

					slot.TargetSlot.QuickSlotIdx = otherInvenIdx;
					slot.TargetSlot.UpdateItem(_interactingInven.Get(otherInvenIdx));
					slot.TargetSlot.StoredInventory = _interactingInven;
					slot.TargetSlot.OnRightClickAction = OnRightClickInteractInven;
				}
			}
		}

		/// <summary>
		/// 유저 장착 인벤토리에서 우클릭을 한다면 
		/// DISCARD : 아이템 부수기
		/// TO STASH : stash 로 이동
		/// 의 행동이 가능해야 한다.
		/// </summary>
		public void OnRightClickUserInven(Graphic_ItemSlot clickedSlot) {
			ModifyPanelData mpd = new ModifyPanelData();

			ModifyPanelInfo info = new ModifyPanelInfo();
			info.BtnName = "DISCARD";
			info.OnClickAction = () => {
				clickedSlot.TargetData.OnDiscard();
			};

			ModifyPanelInfo info1 = new ModifyPanelInfo();
			info1.BtnName = "TO STASH";
			info1.OnClickAction = () => {
				Global_SelfPlayerData.StashInventory.AddInventory(clickedSlot.TargetData);
			};

			mpd.InfoList.Add(info1);
			mpd.InfoList.Add(info);

			Global_UIEvent.CallUIEvent(eEventType.TryModifySlot, mpd);
		}

		/// <summary>
		/// 다른 인벤과 같이 열었을 때 다른 인벤에서 우클릭을 한다면
		/// DISCARD : 아이템 부수기
		/// TO Gear : 내 인벤으로 획득
		/// 의 행동이 가능해야 한다.
		/// </summary>
		public void OnRightClickInteractInven(Graphic_ItemSlot clickedSlot) {
			ModifyPanelData mpd = new ModifyPanelData();

			ModifyPanelInfo info0 = new ModifyPanelInfo();
			info0.BtnName = "DISCARD";
			info0.OnClickAction = () => {
				clickedSlot.TargetData.OnDiscard();
			};

			ModifyPanelInfo info1 = new ModifyPanelInfo();
			info1.BtnName = "TO GEAR";
			info1.OnClickAction = () => {
				Global_SelfPlayerData.SelfInventory.AddInventory(clickedSlot.TargetData);
			};

			mpd.InfoList.Add(info1);
			mpd.InfoList.Add(info0);

			Global_UIEvent.CallUIEvent(eEventType.TryModifySlot, mpd);
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

		public void OnCLickTest() {
			Debug.Log(Global_SelfPlayerData.TryAddSkill(eSkillType.Armor));
		}
	}
}