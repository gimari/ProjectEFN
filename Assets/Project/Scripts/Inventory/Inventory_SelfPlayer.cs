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
	}
}