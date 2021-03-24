using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class Graphic_ToggleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	[SerializeField] private GameObject _targetObj = default;

	private MaskableGraphic[] _graphicTargetList = null;

	private void Awake() {
		this._graphicTargetList = _targetObj.GetComponentsInChildren<MaskableGraphic>();
	}
	
	public void OnPointerDown(PointerEventData eventData) {
		_targetObj.SetActive(true);

		foreach (MaskableGraphic graphic in this._graphicTargetList) {
			graphic.DOFade(0, 0);
			graphic.DOFade(1, 0.2f);
		}
	}

	public void OnPointerUp(PointerEventData eventData) {
		foreach (MaskableGraphic graphic in this._graphicTargetList) {
			graphic.DOFade(0, 0.2f);
		}
	}

}