using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EFN {
	public class UI_DraggingSlot : Game.UI_Ingame {

		private Transform _currentPicking = null;
		private Coroutine _pickingRoutine = null;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent<Transform>(eEventType.TryPickSlot, TryPickSlot);
			Global_UIEvent.RegisterUIEvent(eEventType.EndPickSlot, EndPickSlot);
			Global_UIEvent.RegisterUIEvent(eEventType.OnEndFocus, EndPickSlot);
		}

		public void EndPickSlot() {
			if (null != _pickingRoutine) {
				StopCoroutine(this._pickingRoutine);
			}
		}

		public void TryPickSlot(Transform target) {
			target.SetParent(this.transform);
			_currentPicking = target;

			if (null == _pickingRoutine) {
				_pickingRoutine = StartCoroutine(this.PickingRoutine());
			}
		}

		private IEnumerator PickingRoutine() {
			while (null != this._currentPicking) {
				this._currentPicking.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
				yield return null;
			}
		}
	}
}