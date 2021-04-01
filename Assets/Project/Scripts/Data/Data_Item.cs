using EFN.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	public enum eItemType {
		None = 0,

		Armor_6B3TM,
		Weapon_MP443,
		AMMO_9X19AP,
		WEAPON_ASVAL,
		AMMO_9X39SP5,
		CONS_FIRSTAID,
	}

	[Flags]
	public enum eItemStatus {
		None = 0,
	}

	[Serializable]
	public class Data_Item : Data_Storable {

		private long _price = 0;
		private float _durability = 0;

		/// <summary>
		/// 이게 저장되어 있는 부모 인벤토리를 지정해줘야 한다.
		/// 부모 인벤토리가 없을 수도 있음.
		/// </summary>
		[NonSerialized] private Inventory_Item _storedInventory = null;
		public Inventory_Item StoredInventory {
			get { return this._storedInventory; }
			set { this._storedInventory = value; }
		}

		[SerializeField] private eItemType _itemType;
		public eItemType ItemType {
			get { return this._itemType; }
		}

		[SerializeField] private int _slotIndex = 0;
		public int SlotIndex { 
			get { return this._slotIndex; }
			set { this._slotIndex = value; }
		}

		public Data_Item(eItemType itemType) : base() {
			_itemType = itemType;
			InitStatusData();
		}

		protected override void InitStatusData() {
			base.InitStatusData();
			_statusData = Status_Base.GetStatus(_itemType);
			_key = (int)_itemType;
		}

		public override void OnDiscard() {
			_storedInventory.Remove(this.SlotIndex);
			base.OnDiscard();
		}

		public virtual eErrorCode OnUse() {
			return eErrorCode.Success;
		}

		public virtual eErrorCode Fire() {
			return eErrorCode.Fail;
		}
		
		/// <summary>
		/// 이 아이템을 사용했을 때 문제가 없는지 체크하는 로직.
		/// </summary>
		public virtual bool UseValidate(Actor_Player actor) {
			if (null == StatusData) {
				return false;
			}

			if (false == StatusData.Useable) {
				return false;
			}

			// 이제 담겨있는 실제 인벤토리랑 사용 요청한 놈의 인벤토리랑 달라도 못 씀.
			if (this.StoredInventory != actor.ActorInventory) {
				return false;
			}

			return true;
		}
	}
}