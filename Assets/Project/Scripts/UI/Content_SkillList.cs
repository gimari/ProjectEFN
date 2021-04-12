using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN {
	public class Content_SkillList : MonoBehaviour {

		[SerializeField] private Image _imgSkillIcon = default;
		[SerializeField] private Text _txtSkillLevel = default;

		private eSkillType _itemType = eSkillType.None;

		private Action<eSkillType> _onClickFunc = null;
		public Action<eSkillType> OnClickFunc { set { this._onClickFunc = value; } }

		public void Init(eSkillType itemType) {
			this._itemType = itemType;

			this._txtSkillLevel.text = Global_SelfPlayerData.SkillInventory.Get(_itemType).ToString();
			this._imgSkillIcon.sprite = Global_ResourceContainer.GetSprite(itemType);
		}

		public void OnClickSkill() {
			this._onClickFunc?.Invoke(_itemType);
		}
	}
}