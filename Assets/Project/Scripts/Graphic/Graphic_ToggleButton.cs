using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class Graphic_ToggleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	[SerializeField] private GameObject _targetObj = default;
	[SerializeField] private float _fadeTarget = 1f;

	private MaskableGraphic[] _graphicTargetList = null;

	private void Awake() {
		this._graphicTargetList = _targetObj.GetComponentsInChildren<MaskableGraphic>();
	}
	
	public void OnPointerEnter(PointerEventData eventData) {
		_targetObj.SetActive(true);

		foreach (MaskableGraphic graphic in this._graphicTargetList) {
			graphic.DOFade(0, 0);
			graphic.DOFade(_fadeTarget, 0.2f);
		}
	}

	private void OnDisable() {
		_targetObj.SetActive(false);
	}

	public void OnPointerExit(PointerEventData eventData) {
		foreach (MaskableGraphic graphic in this._graphicTargetList) {
			graphic.DOFade(0, 0.2f);
		}
	}
}