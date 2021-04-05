using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
    public class Content_ItemSlot : MonoBehaviour {

        [SerializeField] private Graphic_ItemSlot _slot = default;

        public void UpdateItem(Data_Item data, int slotIdx, Inventory_Item inven) {
            _slot.QuickSlotIdx = slotIdx;
            _slot.UpdateItem(data);
            _slot.StoredInventory = inven;
        }

		public void ClearImage() {
			_slot.ClearImage();
		}
    }
}