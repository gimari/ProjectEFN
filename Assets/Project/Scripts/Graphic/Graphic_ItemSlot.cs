using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN {

	[RequireComponent(typeof(Button))]
	public class Graphic_ItemSlot : MonoBehaviour {

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

		private Data_Item _targetData = null;
		public Data_Item TargetData {
			get { return this._targetData; }
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

			this._emptyImage.SetActive(false);
			this._itemImage.gameObject.SetActive(true);

			this._itemImage.sprite = Global_ItemIcon.GetSprite(data.ItemType.ToString());
			this._itemImage.SetNativeSize();
			this._itemImage.transform.localPosition = Vector2.zero;
			this._itemImage.transform.localScale = Vector3.one * (this.GetComponent<RectTransform>().rect.height / DEFAULT_ITEMSLOT_SIZE);
		}

		public virtual void ClearImage() {
			_targetData = null;
			this._itemImage.gameObject.SetActive(false);

			if (null != _emptyImage) {
				_emptyImage.SetActive(true);
			}
		}

		/// <summary>
		/// 슬롯 클릭시..
		/// </summary>
		public virtual void OnClickSlot() {
			Global_UIEvent.CallUIEvent(eEventType.TryPickSlot, this);
		}
	}
}