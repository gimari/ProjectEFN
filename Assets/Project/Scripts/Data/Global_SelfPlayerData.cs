using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
	/// <summary>
	/// Global_SelfPlayerData 는 DD 에 붙어서 유저 데이터 관련해 많은 일을 해야함.
	/// </summary>
	public class Global_SelfPlayerData : MonoBehaviour, IDontDestroy {

		private Inventory_SelfPlayer _selfInventory = null;
		public static Inventory_SelfPlayer SelfInventory {
			get { return _instance._selfInventory; }
		}

		private Inventory_Item _stashInventory = null;
		private static Global_SelfPlayerData _instance = null;

		public void Init() {
			_instance = this;

			_selfInventory = new Inventory_SelfPlayer();

			Data_Item item1 = new Data_Item(eItemType.Armor_6B3TM);
			item1.Stackable = false;
			item1.StoredInventory = _selfInventory;

			Data_Item item2 = new Data_Item(eItemType.Weapon_MP443);
			item2.Stackable = false;
			item2.StoredInventory = _selfInventory;

			_selfInventory.AddInventory(item1, 4);
			_selfInventory.AddInventory(item2, 3);
		}
	}
}