using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
    public class Content_ItemSlot : MonoBehaviour {

        [SerializeField] private Graphic_ItemSlot _slot = default;
        public Graphic_ItemSlot TargetSlot { get { return this._slot; } }

        public void UpdateItem(Data_Item data, int slotIdx, Inventory_Item inven) {
            _slot.QuickSlotIdx = slotIdx;
            _slot.UpdateItem(data);
            _slot.StoredInventory = inven;
        }

		public void ClearImage() {
			_slot.ClearImage();
		}

        public bool IsEnable {
            set {
                _slot.IsEnable = value;
            }
        }
    }
}