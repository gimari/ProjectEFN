using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace EFN {
    public class UI_NakMessage : MonoBehaviour {

        [SerializeField] private Text _txtNak = default;

        public void Awake() {
            Global_UIEvent.RegisterUIEvent<string>(ePermanetEventType.ShowNakMsg, ShowNakMsg);
        }

        private void ShowNakMsg(string message) {
            _txtNak.transform.localScale = Vector2.zero;
            _txtNak.gameObject.SetActive(true);
            _txtNak.DOKill();

            _txtNak.transform.DOScale(Vector2.one, 0.2f).SetEase(Ease.OutBack);

            _txtNak.DOFade(1f, 0.2f).OnComplete(() => {
                _txtNak.DOFade(0f, 2f).OnComplete(() => {
                    _txtNak.gameObject.SetActive(false);
                });
            });

            _txtNak.text = message;
        }
    }
}