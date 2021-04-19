using EFN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFN {
    public class Graphic_CustomSlot : Graphic_ItemSlot {

        public Action<Graphic_CustomSlot, Graphic_ItemSlot> CustomDropAction = null;

        public override void OnSlotDropDowned(Graphic_ItemSlot fromSlot) {
            CustomDropAction?.Invoke(this, fromSlot);
        }
    }
}