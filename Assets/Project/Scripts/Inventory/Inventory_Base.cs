using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace EFN {
    public class Inventory_Base<T> where T : Data_Storable {

        protected List<T> _inventoryList = new List<T>();

        protected int _inventorySize = 0;
        public int InventorySize {
            get { return this._inventorySize; }
        }

        public Inventory_Base() { 
        }

        public void AddInventory(T data) {
            if (false == data.Stackable) {
                this._inventoryList.Add(data);
                return;
            }

            var target = from storable in _inventoryList
                         where storable.Key == data.Key
                         select storable;

            if (null == target || target.Count() == 0) {
                this._inventoryList.Add(data);
                return;
            }

            eErrorCode rv = target.First().AddStack(data as Data_Storable);

            if (rv == eErrorCode.StackOverflow) {
                this._inventoryList.Add(data);
            }

            Debug.Log("result : " + data.StackCount);
        }

        public void AddInventory(T data, int slotIdx) {
            if (false == data.Stackable) {
                this._inventoryList.Add(data);
                return;
            }

            var target = _inventoryList.ElementAtOrDefault(slotIdx);
            if (null == target) {
                this._inventoryList[slotIdx] = data;
                return;
            }

            eErrorCode rv = target.AddStack(data as Data_Storable);

            if (rv == eErrorCode.StackOverflow) {
                this._inventoryList.Add(data);
            }

            Debug.Log("result : " + data.StackCount);
        }
    }
}