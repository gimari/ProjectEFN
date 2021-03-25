using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace EFN {
    public class Inventory_Item {

        protected SortedDictionary<int, Data_Item> _inventoryList = new SortedDictionary<int, Data_Item>();

        public void AddInventory(Data_Item item) {
            int firstIdx = GetFirstIdx();

            this._inventoryList.Add(firstIdx, item);
            item.SlotIndex = firstIdx;
        }

        public int GetFirstIdx() {
            int rv = 0;

            while(true == _inventoryList.ContainsKey(rv)) {
                rv++;
            }

            return rv;
        }

        public void AddInventory(Data_Item item, int slotIdx) {

            if (item.Stackable == false) {
                return;
            }

            Data_Item target = null;
            this._inventoryList.TryGetValue(slotIdx, out target);
            if (null == target) {
                this._inventoryList.Add(slotIdx, item);
                item.SlotIndex = slotIdx;
                return;
            }

            if (target.Key != item.Key) {
                return;
            }

            if (target.Stackable == false) {
                return;
            }

            if (target.IsFullStack == true) {
                return;
            }

            eErrorCode rv = target.AddStack(item);
            if (rv == eErrorCode.StackOverflow) {
                // 스택이 초과되서 못 들어갈 때 처리
            }
        }
    }
}