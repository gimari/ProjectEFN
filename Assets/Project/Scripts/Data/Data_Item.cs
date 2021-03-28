using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	public enum eItemType {
		None = 0,

		Armor_6B3TM,
		Weapon_MP443,
	}

	[Flags]
	public enum eItemStatus {
		None = 0,
	}

	public class Data_Item : Data_Storable {

		private eItemStatus _itemStatus = eItemStatus.None;
		private long _price = 0;
		private float _durability = 0;

		private eItemType _itemType;
		public eItemType ItemType {
			get { return this._itemType; }
		}

		private int _slotIndex = 0;
		public int SlotIndex { 
			get { return this._slotIndex; }
			set { this._slotIndex = value; }
		}

		public Data_Item(eItemType itemType) {
			_itemType = itemType;
		}

		public void OnUse() {
			Debug.LogError("I USED.. " + _itemType.ToString() + " / count : " + StackCount);
		}

		public void OnDiscard() { }
	}
}