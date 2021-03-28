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
		}

		[Header("Components")]
		[SerializeField] private GameObject _emptyImage = default;

		[SerializeField] private Transform _slotImage = default;
		public Transform SlotImage {
			get { return _slotImage; }
		}

		public const float DEFAULT_ITEMSLOT_SIZE = 100f;

		private void Awake() {
			this.ImageResize();
		}
		
		/// <summary>
		/// 지금 슬롯 사이즈에 맞게 이미지 크기를 같이 조정해줘야함
		/// </summary>
		private void ImageResize() {
			
			if (null != _emptyImage) {
				_emptyImage.SetActive(null == _slotImage);
			}

			if (null == _slotImage) {
				return;
			}

			_slotImage.transform.localPosition = Vector2.zero;
			_slotImage.localScale = Vector3.one * (this.GetComponent<RectTransform>().rect.height / DEFAULT_ITEMSLOT_SIZE);
		}

		public virtual void ClearImage() {
			_slotImage = null;

			if (null != _emptyImage) {
				_emptyImage.SetActive(true);
			}
		}

		/// <summary>
		/// 이 슬롯에 특정 아이템을 등록해준다.
		/// </summary>
		/// <param name="image"></param>
		public virtual eErrorCode SetImage(Transform image) {
			// 이거 그냥 이렇게 seflplayer 에다가 직접하는거는 그냥 임시임. main 에서도 써야하니깐 일케하지말고 케이스 갈라서 따로해야함
			if (null == Game.Global_Actor.SelfPlayer) {
				return eErrorCode.Fail;
			}

			Graphic_Item target = image.GetComponent<Graphic_Item>();

			// 인벤토리가 정상 반영 콜백.
			eErrorCode rv = Game.Global_Actor.SelfPlayer.ActorInventory.AddInventory(target.TargetData, this._quickSlotIdx);
			if (rv != eErrorCode.Success) {
				return rv;
			}

			this._slotImage = image;
			image.SetParent(this.transform);

			this.ImageResize();

			return rv;
		}

		/// <summary>
		/// 슬롯 클릭시..
		/// </summary>
		public virtual void OnClickSlot() {
			Global_UIEvent.CallUIEvent(eEventType.TryPickSlot, this);
		}
	}
}