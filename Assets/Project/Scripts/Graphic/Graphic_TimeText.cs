using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace EFN {
    public class Graphic_TimeText : MonoBehaviour, IUseTimer {
        [SerializeField] protected TimeFormat _timeFormat;
        [SerializeField] protected Text _refText;
        protected DateTime _endTimeUtc;
        protected UnityAction _onTimeEndCallback;

        void Awake() {
            _endTimeUtc = DateTime.FromBinary(0);
        }

        //끝나는시간과, 시간이 다됐을때의 콜백을 전달.
        //시간이 끝나기전에 씬이동 등으로 인해 대상오브젝트가 사라지면
        public void Init(DateTime endTimeUtc, UnityAction onTimeEndCallback) {
            _endTimeUtc = endTimeUtc;
            _onTimeEndCallback = onTimeEndCallback;

            RefreshTime();

            Global_Time.Timer.AddTimerObject_OneSecondRefresh(this);
        }

        public virtual void RefreshTime() {
            TimeSpan remainTime = _endTimeUtc - Global_Time.CurrentTime;

            try {
                _refText.text = remainTime.ToString(_timeFormat);
            } catch (Exception) {
                // ㅈㅅ 이렇게함
            }

            if(remainTime.Seconds < 0) {
                Global_Time.Timer.RemoveTimerObject_OneSecondRefresh(this);
                _onTimeEndCallback?.Invoke();
                _onTimeEndCallback = null;
            }
        }

        public GameObject GameObject() {
            if (null == this) { 
                return null; 
            }

            return this.gameObject;
        }
    }
}