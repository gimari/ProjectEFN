using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace EFN.Game {
    public class ActorUI_BehaviourBar : MonoBehaviour {

        [SerializeField] private Slider _slider = default;

		private ActorUI_Base _uibase = null;

		private void Awake() {
			_uibase = transform.parent.GetComponentInParent<ActorUI_Base>();
			if (null == _uibase) {
				Global_Common.LogError("CANNOT FIND UI BASE");
				return;
			}

			_uibase.RegisterUIEvent<float>(eActorUIType.StartBehaviour, OnStartBehaviour);
			_uibase.RegisterUIEvent(eActorUIType.EndBehaviour, OnEndBehaviour);
		}

		public void OnStartBehaviour(float timer) {
			_slider.gameObject.SetActive(true);

			_slider.DOValue(0, 0);
			_slider.DOValue(1, timer).OnComplete(() => { this._slider.gameObject.SetActive(false); }).SetEase(Ease.Linear);
		}

		public void OnEndBehaviour() {
			this._slider.gameObject.SetActive(false);
			_slider.value = 0;
			_slider.DOKill();
		}
	}
}