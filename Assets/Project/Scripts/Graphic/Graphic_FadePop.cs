using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Graphic_FadePop : Graphic_SimplePopUp {

    protected override void ShowAnimation(AnimationEndCallback callback) {
        animationTarget.DOKill();
        PlaySound_OnShow();

        animationTarget.GetComponent<CanvasGroup>().alpha = 0;
        animationTarget.GetComponent<CanvasGroup>().DOFade(1f, 0.2f).SetUpdate(true).OnComplete(() => {
            callback?.Invoke();
        });
    }

    protected override void HideAnimation(AnimationEndCallback callback) {
        PlaySound_OnHide();

        animationTarget.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetUpdate(true).OnComplete(() => {
            this.gameObject.SetActive(false);
            callback?.Invoke();
        });
    }
}
