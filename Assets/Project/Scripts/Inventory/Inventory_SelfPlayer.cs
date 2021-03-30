using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	public enum ePlayerSlotType {
		PrimeWeapon = 1,
		SecondWeapon = 2,
		Holster = 3,
		Head = 4,
		Armor = 5,
		Knife = 6,

		Rig = 7,
		Backpack = 8,

		QuickSlotStart = 9,
		BackpackSlotStart = 16,
	}

	[Serializable]
	public class Inventory_SelfPlayer : Inventory_Item {

		public void UseQuickSlot(int pressedKey) {

			switch (pressedKey) {
				case 1:
					Use((int)ePlayerSlotType.PrimeWeapon);
					break;

				case 2:
					Use((int)ePlayerSlotType.SecondWeapon);
					break;

				case 3:
					Use((int)ePlayerSlotType.Holster);
					break;

				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					Use((int)ePlayerSlotType.QuickSlotStart + pressedKey - 4);
					break;

				case 0:
					Use((int)ePlayerSlotType.QuickSlotStart + 6);
					break;
			}
		}

		public override eErrorCode TryFire(int idx) {
			Data_Item targetFireItem = Get(idx);

			if (null == targetFireItem) {
				return eErrorCode.Fail;
			}

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
						if (quickItem.DecreaseItem() == eErrorCode.Success) {
							return eErrorCode.Success;
						}
					}
				}
			}

			// 여기까지오면 실패
			return base.TryFire(idx);
		}
	}
}