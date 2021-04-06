//Author : WJ Choi
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public delegate void AnimationEndCallback();
public class Graphic_SimplePopUp : MonoBehaviour
{
    [Header("Sound Control")]
    [SerializeField]
    protected bool useDefaultSound = true;

    public enum PopUpAnimationType { None, Bounce, CuteBounce }
    [Header("PopUp Animation")][SerializeField]
    protected PopUpAnimationType popUpAnimationType = PopUpAnimationType.Bounce;

    [SerializeField]
    protected Transform animationTarget;
    public Transform AnimationTarget { get { return this.animationTarget; } }

    protected bool _isPopUpShow = false;
    public bool IsPopUpShow { get { return _isPopUpShow; } }

    protected virtual void Awake() {
        if (animationTarget == null) {
            Debug.LogError("팝업 애니메이션 대상을 지정해야 합니다.");
        }
    }

    #region Interface
    public virtual void SetTargetPos(Vector2 pos) {
        animationTarget.transform.position = pos;
    }

    public void Show(AnimationEndCallback callback = null) {
        _isPopUpShow = true;
        this.gameObject.SetActive(true);

        ShowAnimation(callback);
    }

    public void Toggle() {
        if (false == _isPopUpShow) { 
            Show(); 
        } else { 
            Hide(); 
        }
    }

    public void Hide(AnimationEndCallback callback = null) {
        if (false == _isPopUpShow) {
            return;
        }

        _isPopUpShow = false;

        HideAnimation(callback);
    }
    #endregion


    #region Private
    protected virtual void ShowAnimation(AnimationEndCallback callback) {
        animationTarget.DOKill();
        PlaySound_OnShow();

        switch (popUpAnimationType) {
            case PopUpAnimationType.None:
                animationTarget.localScale = Vector3.one;
                callback?.Invoke();
                break;
            case PopUpAnimationType.Bounce:
                animationTarget.localScale = Vector3.zero;
                animationTarget.DOScale(1.1f, 0.2f).SetUpdate(true).OnComplete(() => {
                    animationTarget.DOScale(1f, 0.1f).SetUpdate(true).OnComplete(() => {
                        callback?.Invoke();
                    });
                });
                break;
            case PopUpAnimationType.CuteBounce:
                animationTarget.localScale = Vector3.zero;
                animationTarget.DOScale(1.1f, 0.2f).SetUpdate(true).OnComplete(() => {
                    animationTarget.DOScale(1f, 0.1f).SetUpdate(true);
                });
                animationTarget.rotation = Quaternion.identity;
                animationTarget.Rotate(0, 0, -30);
                animationTarget.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutElastic).SetUpdate(true).OnComplete(() => {
                    callback?.Invoke();
                }); ;
                break;
        }
    }

    protected virtual void HideAnimation(AnimationEndCallback callback) {
        PlaySound_OnHide();

        switch (popUpAnimationType) {
            case PopUpAnimationType.None:
                animationTarget.localScale = Vector3.zero;
                callback?.Invoke();
                break;
            case PopUpAnimationType.Bounce:
            case PopUpAnimationType.CuteBounce:
                animationTarget.DOScale(1.1f, 0.1f).SetUpdate(true).OnComplete(() => {
                    animationTarget.DOScale(0f, 0.2f).SetUpdate(true).OnComplete(() => {
                        callback?.Invoke();
                    });
                });
                break;
        }
    }
    #endregion

    #region SoundOverride
    protected virtual void PlaySound_OnShow() {
    }
    protected virtual void PlaySound_OnHide() {
    }
    #endregion
}
