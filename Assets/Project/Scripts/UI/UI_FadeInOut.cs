using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace EFN {
    public class UI_FadeInOut : MonoBehaviour {

        [SerializeField] private Image _fade = default;

        private void Awake() {
            Global_UIEvent.RegisterUIEvent<Action>(ePermanetEventType.ShowFade, ShowFade);
            Global_UIEvent.RegisterUIEvent(ePermanetEventType.HideFade, HideFade);
            Global_UIEvent.RegisterUIEvent<string>(ePermanetEventType.TryChangeScene, TryChangeScene);
        }

        private void TryChangeScene(string sceneName) {

            ShowFade(() => {
                Resources.UnloadUnusedAssets();
                GC.Collect();

                AsyncOperation ao = Global_Common.LoadSceneAsync(sceneName);
                ao.completed += (asyncOperation) => {
                    HideFade();
                };
            });
        }

        private void HideFade() {
            _fade.gameObject.SetActive(true);
            _fade.DOKill();

            _fade.DOFade(0, 0.4f).OnComplete(() => {
                _fade.gameObject.SetActive(false);
            });
        }

        private void ShowFade(Action onEndFunc) {
            _fade.gameObject.SetActive(true);
            _fade.DOKill();

            _fade.color = Color.clear;
            _fade.DOFade(1, 0.4f).OnComplete(() => {
                onEndFunc?.Invoke(); 
            });
        }
    }
}