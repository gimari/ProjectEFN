using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Graphic_PopUp : Graphic_SimplePopUp
{
    public enum DimAnimationType { None, Linear }

    [Header("Config")]
    [SerializeField] protected bool _pauseWhenPopup = false;

    [Header("Other")]
    [SerializeField] protected DimAnimationType dimAnimationType = DimAnimationType.Linear;
    [SerializeField] protected Image dimObject = default;
    [SerializeField] protected GameObject inputBlocker = default;

    protected Color originalDimColor = Color.clear;

    protected override void Awake()
    {
        base.Awake();

        if (inputBlocker == null)
        {
            Debug.LogError("애니메이션 도중 입력을 방지하는 오브젝트가 있어야합니다.");
        }
        originalDimColor = dimObject != null ? dimObject.color : Color.clear;
    }

    #region Private
    protected override void ShowAnimation(AnimationEndCallback callback)
    {
        if (dimObject != null)
        {
            dimObject.color = Color.clear;
            dimObject.DOColor(originalDimColor, 0.3f).SetUpdate(true);
        }

        //미스클릭 방지
        inputBlocker.SetActive(true);	

        base.ShowAnimation(() => {
            inputBlocker.SetActive(false);
            callback?.Invoke();
        });
    }

    protected override void HideAnimation(AnimationEndCallback callback) {
        if (dimObject != null) {
            dimObject.DOColor(Color.clear, 0.3f).SetUpdate(true);
        }

        //미스클릭 방지
        inputBlocker.SetActive(true);

        base.HideAnimation(callback);
    }
    #endregion

    #region SoundOverride
    protected override void PlaySound_OnShow() {
        if (useDefaultSound == false) { return; }

        if (popUpAnimationType == PopUpAnimationType.Bounce) {
        }
    }
    protected override void PlaySound_OnHide() {
        if (useDefaultSound == false) { return; }

        if (popUpAnimationType == PopUpAnimationType.Bounce) {
        }
    }
    #endregion
}
