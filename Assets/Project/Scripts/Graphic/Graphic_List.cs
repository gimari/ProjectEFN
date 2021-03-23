using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 특정 UI 오브젝트를 손쉽게 늘어놓기 위해 만들어진 스크립트다.
/// 
/// 1. 원하는 오브젝트를 target object 에 할당
/// 2. 추가를 원하는 경우 Add
/// 3. 리스트 안 모든 친구들을 업데이트 하고싶으면 foreach 를 돌리면 됨.
/// </summary>
[RequireComponent (typeof (VerticalLayoutGroup))]
public class Graphic_List : Graphic_ListTemplate {

	private float _spacing = 0;
	private VerticalLayoutGroup _layoutGroup;

	protected override void OnAwake() {

		_layoutGroup = GetComponent<VerticalLayoutGroup>();

		_spacing = GetComponent<VerticalLayoutGroup>().spacing;

		base.OnAwake();
	}

	protected override void CalculateSize() {

		if (null == _layoutGroup) { return; }

		float currentExpectSize = ((_spacing + _childSize.y) * _currentCount) - _spacing + _layoutGroup.padding.top + _layoutGroup.padding.bottom;

		// 목표치보다 크면 새로 할당.
		if (this._startSize.y < currentExpectSize) {
			GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, currentExpectSize);
		} else {
			GetComponent<RectTransform>().sizeDelta = _startSize;
		}
	}

	public override void Remove(int index) {

		if (transform.childCount < index) {
			return;
		}

		transform.GetChild(index).gameObject.SetActive(false);

		_currentCount--;
		CalculateSize();
	}
}
