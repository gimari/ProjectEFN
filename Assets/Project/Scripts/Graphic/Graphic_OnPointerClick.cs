using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Graphic_OnPointerClick : MonoBehaviour, IPointerClickHandler {

	public UnityEvent _onClickAction = default;

	public void OnPointerClick(PointerEventData eventData) {
		_onClickAction?.Invoke();
	}
}