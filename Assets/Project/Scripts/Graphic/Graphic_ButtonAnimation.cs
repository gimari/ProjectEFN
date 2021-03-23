using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LNP {
    [RequireComponent(typeof(Selectable))]
    public class Graphic_ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        public enum AnimationType { ScaleChange, JustSound }

        [SerializeField] Transform _targetGraphic;
        [SerializeField] AnimationType _buttonAnimationType;
        [SerializeField] Vector3 _pushedScale;

        [SerializeField] bool _useDefaultScale = false;

        private Selectable _targetButton;
        private Vector3 _originalScale;

        void Awake() {
            if (true == _useDefaultScale) {
                _originalScale = Vector3.one;
            } else {
                _originalScale = _targetGraphic.localScale;
            }

            _targetButton = GetComponent<Selectable>();
        }


        private bool isPlayDownSound = false;
        public void OnPointerDown(PointerEventData eventData) {
            if (_targetButton.IsInteractable() == false) { return; }
            
            switch (_buttonAnimationType) {
                case AnimationType.ScaleChange:
                    
                    if (true == _useDefaultScale) {
                        _originalScale = Vector3.one;
                    } else {
                        _originalScale = _targetGraphic.localScale;
                    }

                    _targetGraphic.localScale = _pushedScale;
                    break;
            }

            isPlayDownSound = true;
            PlaySound_Down();
        }

        public void OnPointerUp(PointerEventData eventData) {
            switch (_buttonAnimationType) {
                case AnimationType.ScaleChange:
                    _targetGraphic.localScale = _originalScale;

                    break;
            }

            if (isPlayDownSound) {
                PlaySound_Up();
                isPlayDownSound = false;
            }
        }

        protected virtual void PlaySound_Down() {
        }

        protected virtual void PlaySound_Up() {
        }
    }
}