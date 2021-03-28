using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN {
	public class Graphic_Item : MonoBehaviour {

		[SerializeField] private eItemType _debugItemType = default;
		[SerializeField] private Image _uiImage = default;

		private Data_Item _targetData = null;
		public Data_Item TargetData {
			get { return this._targetData; }
		}

		private void Start() {
			Data_Item data = new Data_Item(_debugItemType);
			data.Stackable = false;
			this.SetData(data);
		}

		public void SetData(Data_Item item) {
			_targetData = item;
			UpdateUI();
		}

		/// <summary>
		/// 연결된 UI 가 있다면 업데이트 해준다.
		/// </summary>
		public void UpdateUI() {
			if (null == _uiImage) {
				return;
			}

			_uiImage.sprite = Global_ItemIcon.GetSprite(_targetData.ItemType.ToString());
			_uiImage.SetNativeSize();
		}
	}
}