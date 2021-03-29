using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace EFN {
	public class UI_DraggingSlot : Game.UI_Ingame {

		[SerializeField] private Image _pickingImage = default;
		[SerializeField] private Sprite _emptyImage = default;

		private Coroutine _pickingRoutine = null;
		private Graphic_ItemSlot _fromSlot = null;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent<Graphic_ItemSlot>(eEventType.TryPickSlot, TryPickSlot);
			Global_UIEvent.RegisterUIEvent(eEventType.EndPickSlot, EndPickSlot);
			Global_UIEvent.RegisterUIEvent(eEventType.OnEndFocus, EndPickSlot);
		}
		
		public void EndPickSlot() {
			if (null != _pickingRoutine) {
				StopCoroutine(this._pickingRoutine);
				this._pickingRoutine = null;
			}

			this._fromSlot = null;
			this._pickingImage.sprite = _emptyImage;
		}

		/*
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
		*/

		public void TryPickSlot(Graphic_ItemSlot target) {

			// 빈 곳을 눌렀을 때..
			if (null == target.TargetData) {

				// 집은게 없으면 나감
				if (null == _fromSlot) {
					return;
				}

				// 사실 이 경우가 되면 그대로 바닥에 떨어져야 한다.
				if (null == target.StoredInventory) {
					Global_Common.LogError("CANT MOVE TO NULL INVEN");
					return;
				}

				// 스왑
				target.StoredInventory.AddInventory(_fromSlot.TargetData, target.QuickSlotIdx);
				EndPickSlot();

			} else if (null == _fromSlot) {
				// 안 비어있는곳을 눌렀는데 현재 집은게 없으면 들고온다.
				this.UpdateImage(target.TargetData);

				// 피킹 루틴도 돌려줌
				if (null == _pickingRoutine) {
					_pickingRoutine = StartCoroutine(this.PickingRoutine());
				}

				// 마지막 슬롯 갱신.
				this._fromSlot = target;

			} else {
				// 사실 이 경우가 되면 그대로 바닥에 떨어져야 한다.
				if (null == target.StoredInventory) {
					Global_Common.LogError("CANT MOVE TO NULL INVEN");
					return;
				}

				// 스왑하는데 뭔가 제대로 교체가 안됬으면 아무것도 하지 않음.
				eErrorCode rv = target.StoredInventory.AddInventory(_fromSlot.TargetData, target.QuickSlotIdx);
				if (rv != eErrorCode.Success) {
					return;
				}

				EndPickSlot();
			}
		}

		public void UpdateImage(Data_Item data) {

			if (null == data) {
				return;
			}

			this._pickingImage.sprite = Global_ItemIcon.GetSprite(data.ItemType.ToString());
			this._pickingImage.SetNativeSize();
		}


		private IEnumerator PickingRoutine() {
			while (null != this._pickingImage) {
				this._pickingImage.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
				yield return null;
			}
		}
	}
}