using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
	public class Graphic_ItemSlot : MonoBehaviour {

		[SerializeField] private GameObject _emptyImage = default;

		[SerializeField] private Transform _slotImage = default;
		public Transform SlotImage {
			get { return this._slotImage; }
		}

		public void OnClickSlot() {
			if (null != _slotImage) {
				Global_UIEvent.CallUIEvent(eEventType.TryPickSlot, _slotImage);
			} else {
				Global_UIEvent.CallUIEvent(eEventType.EndPickSlot);
			}
		}
	}
}