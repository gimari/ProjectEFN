using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	public enum ePlayerSlotType {
		None = 0,
		PrimeWeapon = 1,
		SecondWeapon = 2,
		Holster = 3,
		Head = 4,
		Armor = 5,
		Knife = 6,

		QuickSlotStart = 7,
		BackpackSlotStart = 14,
	}

	/// <summary>
	/// 플레이어 착용 슬롯 전용. 아래의 4가지 밖에 없으며 기본적으로 ePlayerSlotType 과 할당된 숫자는 같아야 한다.
	/// 이걸 왜 따로 뒀냐면 ePlayerSlotType 에는 None 을 0 으로 두기 애매한데 -1 로 할순 없어서 그럼
	/// </summary>
	public enum ePlayerEquipSlot {
		None = 0,
		PrimeWeapon = 1,
		SecondWeapon = 2,
		Holster = 3,
		Knife = 6,
	}

	[Serializable]
	public class Inventory_SelfPlayer : Inventory_Item {

		public static int ConvertQuickSlotIndexToSlotIndex(int QuickIndex) {

			switch (QuickIndex) {
				case 1:
					return (int)ePlayerSlotType.PrimeWeapon;

				case 2:
					return (int)ePlayerSlotType.SecondWeapon;

				case 3:
					return (int)ePlayerSlotType.Holster;

				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					return (int)ePlayerSlotType.QuickSlotStart + QuickIndex - 4;

				case 0:
					return (int)ePlayerSlotType.QuickSlotStart + 6;

				default:
					return QuickIndex;
			}
		}

		public override eErrorCode TryReload(int idx) {
			Data_Item targetReloadItem = Get(idx);

			if (null == targetReloadItem) {
				return eErrorCode.Fail;
			}

			// 칼이면 아무것도 안한다.
			if (true == targetReloadItem.StatusData.IsKnifeWeapon) {
				return eErrorCode.Fail;
			}

			eItemType[] requireList = targetReloadItem.StatusData.RequireItem;

			// 필요한게 없으면 아무것도 안한다.
			if (null == requireList) {
				return eErrorCode.Fail;
			}

			// 뭐든지 사용할려면 퀵슬롯에 잇는 애들만 대상이다.
			for (int quickIdx = (int)ePlayerSlotType.QuickSlotStart; quickIdx < (int)ePlayerSlotType.BackpackSlotStart; quickIdx++) {
				Data_Item quickItem = null;

				_inventoryList.TryGetValue(quickIdx, out quickItem);
				if (null == quickItem) {
					continue;
				}

				foreach (eItemType itemType in requireList) {

					// 필요한 아이템이 있으면 소비체크하고 성공 처리
					if (itemType == quickItem.ItemType) {

						int usedCount = 0;
						int roundCount = targetReloadItem.StatusData.MaxRoundAmount - targetReloadItem.FireModule.AmmoCount;

						// 충전해줘야하는 용량만큼 사용해준다.
						if (quickItem.DecreaseItem(roundCount, out usedCount) == eErrorCode.Success) {
							targetReloadItem.FireModule.Reload(quickItem.ItemType, usedCount);

							// 콜백 ㅎㅎ
							Global_UIEvent.CallUIEvent(eEventType.UpdateUserInventory);
							return eErrorCode.Success;
						}
					}
				}
			}

			// 여기까지오면 실패
			return eErrorCode.Fail;
		}

		public override eErrorCode TryFire(int idx, out eItemType firedItem) {
			Data_Item targetFireItem = Get(idx);
			firedItem = eItemType.None;

			if (null == targetFireItem) {
				return eErrorCode.Fail;
			}

			eErrorCode rv = targetFireItem.TryFire();

			if (rv == eErrorCode.Success) {
				firedItem = targetFireItem.FireModule.LoadedAmmo;
			}

			return rv;

			/*
			eItemType[] requireList = targetFireItem.StatusData.RequireItem;

			// 필요한게 없으면 나간다.. fire 인데 필요한게 없으면 무한총알인가?
			if (null == requireList) {
				return eErrorCode.Success;
			}

			// 뭐든지 사용할려면 퀵슬롯에 잇는 애들만 대상이다.
			for (int quickIdx = (int)ePlayerSlotType.QuickSlotStart; quickIdx < (int)ePlayerSlotType.BackpackSlotStart; quickIdx++) {
				Data_Item quickItem = null;

				_inventoryList.TryGetValue(quickIdx, out quickItem);
				if (null == quickItem) {
					continue;
				}

				foreach (eItemType itemType in requireList) {

					// 필요한 아이템이 있으면 소비체크하고 성공 처리
					if (itemType == quickItem.ItemType) {

#if EFN_DEBUG
						if (true == Global_DebugConfig.InfiniteBullet || quickItem.DecreaseItem() == eErrorCode.Success) {
#else
						if (quickItem.DecreaseItem() == eErrorCode.Success) {
#endif
							firedItem = quickItem.ItemType;
							return eErrorCode.Success;
						}
					}
				}
			}

			// 여기까지오면 실패
			return base.TryFire(idx, out firedItem);*/
		}

		protected override bool CheckSlotIndex(Data_Item fromItem, int targetIdx) {
			// ㅋㅋ
			if (targetIdx != (int)ePlayerSlotType.Head &&
				targetIdx != (int)ePlayerSlotType.Holster &&
				targetIdx != (int)ePlayerSlotType.Armor &&
				targetIdx != (int)ePlayerSlotType.Knife &&
				targetIdx != (int)ePlayerSlotType.PrimeWeapon &&
				targetIdx != (int)ePlayerSlotType.SecondWeapon) {
				return true;
			}

			if (fromItem.StatusData.TargetEquipSlot == ePlayerSlotType.None) {
				return false;
			}

			if (fromItem.StatusData.TargetEquipSlot == ePlayerSlotType.PrimeWeapon && targetIdx == (int)ePlayerSlotType.SecondWeapon) {
				return true;
			}

			if ((int)fromItem.StatusData.TargetEquipSlot != targetIdx) {
				return false;
			}

			return true;
		}
	}
}