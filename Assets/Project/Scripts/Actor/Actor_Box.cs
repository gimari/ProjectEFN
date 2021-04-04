using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace EFN.Game {
	[Serializable]
	public class DropTableInfo {
		public eItemType ItemType;  // 드랍 아이템
		public float DropRate;      // 0 ~ 100
		public int MinCount;        // 최소 개수
		public int MaxCount;        // 최대 개수
	}

	public class Actor_Box : Actor_Base {

		[SerializeField] private DropTableInfo[] _boxDropTable = default;

		protected override void OnAwake() {
			base.OnAwake();

			List<Data_Item> _container = new List<Data_Item>();

			foreach (DropTableInfo info in _boxDropTable) {
				float rand = UnityEngine.Random.Range(0, 100f);

				// 드랍 됬음?
				if (info.DropRate < rand) {
					continue;
				}

				Data_Item dropped = new Data_Item(info.ItemType);

				dropped.StackCount = UnityEngine.Random.Range(info.MinCount, info.MaxCount + 1);

				_container.Add(dropped);
			}

			_actorInventory = new Inventory_Item();
			_actorInventory.MaxDisplayIndex = _container.Count;

			for (int length = _container.Count; 0 < length; length--) {
				int index = UnityEngine.Random.Range(0, length);
				_actorInventory.AddInventory(_container[index]);
				_container.RemoveAt(index);
			}
		}

		protected override void OnTriggerEnter2D(Collider2D other) {
			if (other.name != "Collider_Closechecker") {
				return;
			}

            Global_Actor.Interactable.Add(this.gameObject);
        }

        protected override void OnTriggerExit2D(Collider2D other) {
			if (other.name != "Collider_Closechecker") {
				return;
			}

			Global_Actor.Interactable.Remove(this.gameObject);
        }
    }
}