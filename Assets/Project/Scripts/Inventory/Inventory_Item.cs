using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace EFN {
    public class Inventory_Item {

        protected SortedDictionary<int, Data_Item> _inventoryList = new SortedDictionary<int, Data_Item>();

        public virtual void AddInventory(Data_Item item) {
            int firstIdx = GetFirstIdx();

            this._inventoryList.Add(firstIdx, item);
            item.SlotIndex = firstIdx;
        }

        public virtual int GetFirstIdx() {
            int rv = 0;

            while(true == _inventoryList.ContainsKey(rv)) {
                rv++;
            }

            return rv;
        }

		/// <summary>
		/// 같은 인벤토리 내부에서 특정 아이템을 다른 슬롯으로 옮길 때!!
		/// </summary>
        public virtual eErrorCode AddInventory(Data_Item fromItem, int targetIdx) {

			int fromIdx = fromItem.SlotIndex;

			Data_Item target = null;
            this._inventoryList.TryGetValue(targetIdx, out target);

			// 들어가려 하는 슬롯에 없으면 null 로 처리
            if (null == target) {
                this._inventoryList.Add(targetIdx, fromItem);
                fromItem.SlotIndex = targetIdx;

				// 성공적으로 자리 바꿈. 기존거에서 지우고 나감
				this._inventoryList.Remove(fromIdx);
				return eErrorCode.Success;
            }

			// 키가 다르거나, 이미 있는애가 스택이 안되거나, 스택이 꽉차있으면 서로 교체
            if (target.Key != fromItem.Key || target.Stackable == false || target.IsFullStack == true) {
				int swapIndex = target.SlotIndex;

				this._inventoryList[fromItem.SlotIndex] = target;
				target.SlotIndex = fromItem.SlotIndex;

				this._inventoryList[swapIndex] = fromItem;
				fromItem.SlotIndex = swapIndex;

				// 교체 성공.
				return eErrorCode.Success;
            }
			
            eErrorCode rv = target.AddStack(fromItem);

			// 성공시에만 기존 자리에서 지운다.
            if (rv == eErrorCode.Success) {
				this._inventoryList.Remove(fromIdx);
			}

			return rv;
		}

		/// <summary>
		/// 각기 다른 두개의 인벤토리에서 슬롯을 서로 옮길 때!!
		/// </summary>
		public virtual eErrorCode AddInventory(Data_Item item, int slotIdx, Inventory_Item externalInven) {
			if (externalInven == this) {
				return AddInventory(item, slotIdx);
			}

			return eErrorCode.Success;
		}

		public virtual void Use(int slotIdx) {
			if (false == this._inventoryList.ContainsKey(slotIdx)) {
				return;
			}

			this._inventoryList[slotIdx].OnUse();
		}
    }
}