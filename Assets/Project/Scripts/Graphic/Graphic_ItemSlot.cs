using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFN {

	[RequireComponent(typeof(Button))]
	public class Graphic_ItemSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler {

		[Header("Config")]
		[SerializeField] private bool _useEmptyImage = true;

		[SerializeField] private int _quickSlotIdx = default;
		public int QuickSlotIdx {
			get { return this._quickSlotIdx; }
			set { this._quickSlotIdx = value; }
		}

		[Header("Components")]
		[SerializeField] private GameObject _emptyImage = default;
		[SerializeField] private Image _itemImage = default;
		[SerializeField] private Text _txtItemCount = default;
		[SerializeField] private Image _raycastTarget = default;
		[SerializeField] private GameObject _blockImage = default;

		private Data_Item _targetData = null;
		public Data_Item TargetData {
			get { return this._targetData; }
		}

		private Action<Graphic_ItemSlot> _onrightClickAction = null;
		public Action<Graphic_ItemSlot> OnRightClickAction {
			set { this._onrightClickAction = value; }
		}

		/// <summary>
		/// 이 슬롯이 들어가있는 인벤토리가 있어야 함..
		/// 데이터가 잇으면 그걸 반환하면 되지만 없다면 지정해주어야 한다.
		/// </summary>
		private Inventory_Item _storedInventory = null;
		public Inventory_Item StoredInventory {
			get {
				if (null != _targetData) {
					return _targetData.StoredInventory;
				} else {
					return _storedInventory;
				}
			}

			set { _storedInventory = value; }
		}

		public bool IsEnable {
			set {
				_blockImage.SetActive(!value);
			}
		}

		public const float DEFAULT_ITEMSLOT_SIZE = 100f;

		public bool IsEmpty() {
			return _targetData == null;
		}

		public virtual void UpdateItem(Data_Item data) {

			this._targetData = data;

			// empty?
			if (null == data) {
				ClearImage();
				return;
			}

			if (null != _emptyImage) {
				this._emptyImage.SetActive(false);
			}

			if (null != _raycastTarget) {
				_raycastTarget.raycastTarget = true;
			}

			this._itemImage.gameObject.SetActive(true);

			this._itemImage.sprite = Global_ResourceContainer.GetSprite(data.ItemType.ToString());
			this._itemImage.SetNativeSize();
			this._itemImage.transform.localPosition = Vector2.zero;
			this._itemImage.transform.localScale = Vector3.one * (this.GetComponent<RectTransform>().rect.height / DEFAULT_ITEMSLOT_SIZE);

			this._txtItemCount.gameObject.SetActive(data.StatusData.DisplayStack);
			this._txtItemCount.text = data.StackCount.ToString() + "/" + data.StatusData.MaxStackSize.ToString();
		}

		public virtual void ClearImage() {
			_targetData = null;
			this._itemImage.gameObject.SetActive(false);

			if (null != _raycastTarget) {
				// _raycastTarget.raycastTarget = false;
			}

			if (null != _emptyImage) {
				_emptyImage.SetActive(true);
			}

			if (null != _txtItemCount) {
				_txtItemCount.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// 슬롯 클릭시..
		/// </summary>
		// public virtual void OnClickSlot() {
			// Global_UIEvent.CallUIEvent(eEventType.TryPickSlot, this);
		// }

		public virtual void OnBeginDrag(PointerEventData eventData) {
			if (null == _targetData) {
				return;
			}

			if (true == _blockImage.activeSelf) {
				return;
			}

			Global_UIEvent.CallUIEvent(eEventType.TryPickSlot, this);
		}

		public virtual void OnEndDrag(PointerEventData eventData) {
			if (null == eventData || null == eventData.pointerEnter) { return; }

			Graphic_ItemSlot endTarget = eventData.pointerEnter.GetComponent<Graphic_ItemSlot>();

			if (null == endTarget) {
				Global_UIEvent.CallUIEvent(eEventType.EndPickSlot);
				return; 
			}

			Global_UIEvent.CallUIEvent(eEventType.TryPickSlot, endTarget);
		}

		public void OnDrag(PointerEventData eventData) { }

		public virtual void OnPointerClick(PointerEventData eventData) {
			if (null == _targetData) {
				return;
			}

			if (true == _blockImage.activeSelf) {
				return;
			}

			if (eventData.button == PointerEventData.InputButton.Right) {
				_onrightClickAction?.Invoke(this);
			}
		}

		/// <summary>
		/// 다른 슬롯이 포함된 상태로 이 슬롯 위에 드롭-다운 되었다.
		/// </summary>
		public virtual void OnSlotDropDowned(Graphic_ItemSlot fromSlot) {
			// 집은게 없으면 나감
			if (null == fromSlot) {
				return;
			}

			// 사실 이 경우가 되면 그대로 바닥에 떨어져야 한다.
			if (null == StoredInventory) {
				Global_Common.LogError("CANT MOVE TO NULL INVEN");
				return;
			}

			// 스왑
			StoredInventory.AddInventory(fromSlot.TargetData, QuickSlotIdx);
		}
	}
}