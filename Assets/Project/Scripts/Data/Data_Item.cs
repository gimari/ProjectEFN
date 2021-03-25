using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	public enum eItemType {

	}

	[Flags]
	public enum eItemStatus {
		None = 0,
	}

	public class Data_Item : Data_Storable {

		private eItemStatus _itemStatus = eItemStatus.None;
		private long _price = 0;
		private float _durability = 0;

		private int _slotIndex = 0;
		public int SlotIndex { 
			get { return this._slotIndex; }
			set { this._slotIndex = value; }
		}
	}
}