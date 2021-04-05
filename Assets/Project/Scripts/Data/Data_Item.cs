using EFN.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	[Flags]
	public enum eItemStatus {
		None = 0,
	}

	[Serializable]
	public class Data_Item : Data_Storable {

		private long _price = 0;
		private float _durability = 0;

		private ItemFireModule _fireModule = null;
		public ItemFireModule FireModule { get { return this._fireModule; } }

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

		/// <summary>
		/// 고정정보 관련 데이타를 여기서 모두 초기화해준다.
		/// 특별히 아이템과 연관된 다른 모듈 (fire module) 같은게 잇으면 여기서 또한 달아준다.
		/// </summary>
		protected override void InitStatusData() {
			base.InitStatusData();
			_statusData = Status_Base.GetStatus(_itemType);
			_key = (int)_itemType;

			// 쏠수 있는거면 fire module
			if (null != _statusData && true == _statusData.Fireable) {
				_fireModule = new ItemFireModule();
			}
		}

		public override void OnDiscard() {
			_storedInventory.Remove(this.SlotIndex);
			base.OnDiscard();
		}

		public virtual eErrorCode OnUse() {
			return eErrorCode.Success;
		}

		public virtual eErrorCode TryFire() {
			if (false == _statusData.Fireable) {
				return eErrorCode.Fail;
			}

			// 근접무기는 그냥 발사됨
			if (true == _statusData.IsKnifeWeapon) {
				return eErrorCode.Success;
			}

			if (null == _fireModule) {
				Global_Common.LogError("발사할 수 있는데 발사 모듈이 없음!!");
				return eErrorCode.Fail;
			}

			return _fireModule.TryFire();
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

	public class ItemFireModule {

		private int _ammoCount = 0;
		public int AmmoCount { get { return _ammoCount; } }

		private eItemType _loadedAmmo = eItemType.None;
		public eItemType LoadedAmmo { get { return _loadedAmmo; } }

		public void Reload(eItemType loadedAmmo, int ammoCount) {
			_loadedAmmo = loadedAmmo;
			_ammoCount = ammoCount;
		}

		public eErrorCode TryFire() {
			if (_loadedAmmo == eItemType.None) {
				return eErrorCode.Fail;
			}

#if EFN_DEBUG
			// 총알무한이면 그냥 success.
			if (true == Global_DebugConfig.InfiniteBullet) {
				return eErrorCode.Success;
			}
#endif

			if (_ammoCount <= 0) {
				return eErrorCode.Fail;
			}

			_ammoCount--;
			return eErrorCode.Success;
		}
	}
}