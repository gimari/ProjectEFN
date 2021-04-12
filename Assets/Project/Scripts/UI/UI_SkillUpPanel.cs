using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN.Main {
	public class UI_SkillUpPanel : MonoBehaviour {

		[SerializeField] private Graphic_FadePop _panelUp = default;
		[SerializeField] private Graphic_NumericText _cokeCount = default;
		[SerializeField] private Graphic_LayoutList _skillList = default;
		[SerializeField] private GameObject _levleUpSet = default;
		[SerializeField] private Image _imgSkillIcon = default;
		[SerializeField] private Text _txtSkillExplain = default;

		private eSkillType _currentSkill = eSkillType.None;

		public void Open() {
			_cokeCount.SetNumericTextWithOutAnimation(0, MoneyFormat.JustComma);
			_currentSkill = eSkillType.None;

			_panelUp.gameObject.SetActive(false);
			_panelUp.Hide();

			UpdateSkillList();
		}

		public void UpdateSkillList() {
			_skillList.Init();

			Content_SkillList content;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.Armor);
			content.OnClickFunc = OnClickSkillBtn;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.Health);
			content.OnClickFunc = OnClickSkillBtn;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.Speed);
			content.OnClickFunc = OnClickSkillBtn;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.Sight);
			content.OnClickFunc = OnClickSkillBtn;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.Melee);
			content.OnClickFunc = OnClickSkillBtn;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.Silence);
			content.OnClickFunc = OnClickSkillBtn;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.Recoil);
			content.OnClickFunc = OnClickSkillBtn;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.Critical);
			content.OnClickFunc = OnClickSkillBtn;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.CritDmg);
			content.OnClickFunc = OnClickSkillBtn;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.NormalDmg);
			content.OnClickFunc = OnClickSkillBtn;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.StashSize);
			content.OnClickFunc = OnClickSkillBtn;

			content = _skillList.AddWith<Content_SkillList>();
			content.Init(eSkillType.InvenSize);
			content.OnClickFunc = OnClickSkillBtn;
		}

		public void OnClickSkillBtn(eSkillType skill) {
			if (false == _panelUp.IsPopUpShow) {
				_panelUp.Show();
			}

			_currentSkill = skill;

			Status_Skill status = Status_Skill.GetStatus(_currentSkill);
			int level = Global_SelfPlayerData.SkillInventory.Get(_currentSkill);

			this._imgSkillIcon.sprite = Global_ResourceContainer.GetSprite(_currentSkill);
			this._txtSkillExplain.text = string.Format(status.Explain, status.EffectAmount(level));

			if (status.MaxLevel <= level) {
				_levleUpSet.SetActive(false);
				return;
			}

			_levleUpSet.SetActive(true);
			long cost = status.NextLevelCost(level);

			_cokeCount.NumberTextAnimate(cost, MoneyFormat.JustComma);
		}

		public void OnClickLevelUp() {
			eErrorCode rv = Global_SelfPlayerData.TryAddSkill(_currentSkill);

			switch (rv) {
				case eErrorCode.MaxSkillLevel:
					Global_UIEvent.CallUIEvent(ePermanetEventType.ShowNakMsg, "최고 레벨입니다!");
					break;

				case eErrorCode.NotenoughCoke:
					Global_UIEvent.CallUIEvent(ePermanetEventType.ShowNakMsg, "COKE 가 충분하지 않습니다!");
					break;
			}

			OnClickSkillBtn(_currentSkill);
			UpdateSkillList();
		}
	}
}