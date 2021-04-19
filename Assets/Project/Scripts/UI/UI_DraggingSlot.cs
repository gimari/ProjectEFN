using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EFN {
	public class UI_DraggingSlot : Game.UI_Ingame {

		[SerializeField] private Image _pickingImage = default;
		[SerializeField] private Sprite _emptyImage = default;
		[SerializeField] private Panel_ItemModify _modifyPanel = default;

		private Coroutine _pickingRoutine = null;
		private Graphic_ItemSlot _fromSlot = null;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent<Graphic_ItemSlot>(eEventType.TryPickSlot, TryPickSlot);
			Global_UIEvent.RegisterUIEvent(eEventType.EndPickSlot, EndPickSlot);
			Global_UIEvent.RegisterUIEvent(eEventType.OnEndFocus, EndPickSlot);
			Global_UIEvent.RegisterUIEvent<ModifyPanelData>(eEventType.TryModifySlot, TryModifySlot);
		}
		
		public void EndPickSlot() {
			if (null != _pickingRoutine) {
				StopCoroutine(this._pickingRoutine);
				this._pickingRoutine = null;
			}

			this._fromSlot = null;
			this._pickingImage.sprite = _emptyImage;

			EndModifySlot();
		}

		public void TryPickSlot(Graphic_ItemSlot target) {

			EndModifySlot();
			
			// 빈 곳을 눌렀을 때..
			if (null == target.TargetData) {
				target.OnSlotDropDowned(_fromSlot);
				EndPickSlot();

			} else if (null == _fromSlot) {
				if (true == target.BlockAnyDrag) {
					return;
				}

				// 안 비어있는곳을 눌렀는데 현재 집은게 없으면 들고온다.
				this.UpdateImage(target.TargetData);

				// 피킹 루틴도 돌려줌
				if (null == _pickingRoutine) {
					_pickingRoutine = StartCoroutine(this.PickingRoutine());
				}

				// 마지막 슬롯 갱신.
				this._fromSlot = target;

			} else {
				// 빈곳도 아니고 이미 들고있는것도 있다.
				if (true == target.BlockAnyDrag) {
					EndPickSlot();
					return;
				}

				target.OnSlotDropDowned(_fromSlot);
				EndPickSlot();
			}
		}

		public void UpdateImage(Data_Item data) {

			if (null == data) {
				return;
			}

			this._pickingImage.sprite = Global_ResourceContainer.GetSprite(data.ItemType.ToString());
			this._pickingImage.SetNativeSize();
		}


		private IEnumerator PickingRoutine() {
			while (null != this._pickingImage) {
				this._pickingImage.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
				yield return null;
			}
		}

		public void TryModifySlot(ModifyPanelData target) {
			_modifyPanel.SetInfo(target);
		}

		public void EndModifySlot() {
			_modifyPanel.EndInfo();
		}
	}
}