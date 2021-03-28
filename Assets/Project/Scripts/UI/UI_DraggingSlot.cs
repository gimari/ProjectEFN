using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EFN {
	public class UI_DraggingSlot : Game.UI_Ingame {

		private Transform _currentPicking = null;
		private Coroutine _pickingRoutine = null;
		private Graphic_ItemSlot _fromSlot = null;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent<Graphic_ItemSlot>(eEventType.TryPickSlot, TryPickSlot);
			Global_UIEvent.RegisterUIEvent(eEventType.EndPickSlot, EndPickSlot);
			Global_UIEvent.RegisterUIEvent(eEventType.OnEndFocus, UndoPickSlot);
		}
		
		public void EndPickSlot() {
			if (null != _pickingRoutine) {
				StopCoroutine(this._pickingRoutine);
				this._pickingRoutine = null;
			}

			this._currentPicking = null;
		}

		/// <summary>
		/// 픽을 끝내는데 현재 들고 있는 놈을 마지막 슬롯으로 되돌려 보낸다.
		/// </summary>
		public void UndoPickSlot() {
			// 만약 아직 들고잇는게 남아있다면 원래 있던 슬롯으로 다시 줘야 한다.
			if (null != _fromSlot && null != this._currentPicking) {
				this._fromSlot.SetImage(this._currentPicking);
			}

			this._fromSlot = null;

			this.EndPickSlot();
		}
		
		public void TryPickSlot(Graphic_ItemSlot target) {

			// 빈 곳을 눌렀을 때..
			if (null == target.SlotImage) {

				// 집은게 없으면 나감
				if (null == _currentPicking) {
					return;
				}

				target.SetImage(_currentPicking);
				EndPickSlot();

				// 마지막 슬롯 갱신.
				this._fromSlot = target;

			} else if (null == _currentPicking) {
				// 안 비어있는곳을 눌렀는데 현재 집은게 없으면 들고온다.
				_currentPicking = target.SlotImage;
				_currentPicking.SetParent(this.transform);

				target.ClearImage();

				// 피킹 루틴도 돌려줌
				if (null == _pickingRoutine) {
					_pickingRoutine = StartCoroutine(this.PickingRoutine());
				}

				// 마지막 슬롯 갱신.
				this._fromSlot = target;

			} else {
				// 안비었는데 집은게 잇으면 바꿔줘야 한다.
				Transform targetImage = target.SlotImage;

				// 뭔가 제대로 교체가 안됬으면 아무것도 하지 않음.
				eErrorCode rv = target.SetImage(_currentPicking);
				if (rv != eErrorCode.Success) {
					return;
				}

				_currentPicking = targetImage;
				_currentPicking.SetParent(this.transform);
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