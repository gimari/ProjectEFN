using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class Graphic_LayoutList : Graphic_ListTemplate {

	protected GridLayoutGroup _layoutGroup;

	protected override void OnAwake() {

		// 변수 세팅
		_layoutGroup = GetComponent<GridLayoutGroup>();

		base.OnAwake();

		_childSize = _layoutGroup.cellSize;
	}

	protected override void CalculateSize() {

		if (null == _layoutGroup) { return; }

		if (_layoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount) {

			// 고정 열
			float currentExpectSize = ((_layoutGroup.spacing.y + _childSize.y) * ((Mathf.Max(0, _currentCount - 1) / _layoutGroup.constraintCount) + 1)) 
				- _layoutGroup.spacing.y + _layoutGroup.padding.top + _layoutGroup.padding.bottom;

			// 변하면 크기를 바꿔주자.
			if (this._startSize.y < currentExpectSize) {
				GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, currentExpectSize);
			}
			else {
				GetComponent<RectTransform>().sizeDelta = _startSize;
			}

		} else if (_layoutGroup.constraint == GridLayoutGroup.Constraint.FixedRowCount) {

			// 고정 행
			float currentExpectSize = ((_layoutGroup.spacing.x + _childSize.x) * ( Mathf.Ceil((float)_currentCount / (float)_layoutGroup.constraintCount))) 
				- _layoutGroup.spacing.x + _layoutGroup.padding.left + _layoutGroup.padding.right;

			// 변하면 크기를 바꿔주자.
			if (this._startSize.x < currentExpectSize) {
				GetComponent<RectTransform>().sizeDelta = new Vector2(currentExpectSize, GetComponent<RectTransform>().sizeDelta.y);
			} else {
				GetComponent<RectTransform>().sizeDelta = _startSize;
			}

		} else {
			Debug.LogError("아직 자유 형태의 Layout 은 지원하지 않습니다!! Constraint 를 무언가 Fixed 로 지정해야 합니다.");

			GetComponent<RectTransform>().sizeDelta = _startSize;
		}
	}
}
