using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace EFN {
    public delegate void OnOneSecondLater_WithRemainTime(TimeSpan remainTime);
    public delegate void OnOneSecondLater();
    public class Global_Time : MonoBehaviour {

        public static class TimeScaler {
            private static Dictionary<int, bool> _popupRecorder = new Dictionary<int, bool>();

            public static void AddTime(int popupHash) {
                if (false == _popupRecorder.ContainsKey(popupHash)) {
                    _popupRecorder.Add(popupHash, true);
                }

                if (0 < _popupRecorder.Count) {
                    Time.timeScale = 0;
                }
            }

            public static void RemoveTime(int popupHash) {
                _popupRecorder.Remove(popupHash);

                if (_popupRecorder.Count <= 0) {
                    Time.timeScale = 1;
                }
            }

            public static void Clear() {
                _popupRecorder.Clear();
                Time.timeScale = 1;
            }
        }

        private static Global_Time _instance;
        public static Global_Time Instance {
            get {
                if (null == _instance) {
                    Debug.LogError("Global_Time 가 초기화되기 전에 호출했습니다! 치명적입니다!");
                }

                return _instance;
            }
        }

        public static DateTime CurrentTime { get { return DateTime.UtcNow; } }

        private void Awake() {
            _instance = this;
            Timer.StartTimerRoutine();
        }

#if UNITY_EDITOR
        /*
        void Update() {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                Time.timeScale *= 1.5f;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                Time.timeScale *= 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                Time.timeScale = 1f;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                Time.timeScale = 1f;
            }
        }
        */
#endif

        public static class Timer {
            private static List<IUseTimer> oneSecTimerObjects = new List<IUseTimer>();
            private static float timeAcc = 0;

            public static void StartTimerRoutine() {
                _instance.StartCoroutine(OneSecondCountRoutine());
            }

            public static void AddTimerObject_OneSecondRefresh(IUseTimer target) {
                oneSecTimerObjects.Add(target);
            }
            public static void RemoveTimerObject_OneSecondRefresh(IUseTimer target) {
                oneSecTimerObjects.Remove(target);
            }

            public static void Manual_Refresh() {
                timeAcc = 0;
                RefreshAll();
            }

            private static IEnumerator OneSecondCountRoutine() {
                timeAcc = 0;

                while (true) {
                    timeAcc += Time.unscaledDeltaTime;
                    //1초마다 발생
                    if (timeAcc >= 1f) {
                        RefreshAll();
                        timeAcc -= 1f;
                    }
                    yield return null;
                }
            }

            private static void RefreshAll() {
                for (int i = oneSecTimerObjects.Count - 1; i >= 0; i--) {
                    if (oneSecTimerObjects[i] == null) {
                        oneSecTimerObjects.RemoveAt(i);

                    } else if (oneSecTimerObjects[i].GameObject() == null) {
                        oneSecTimerObjects.RemoveAt(i);

                    } else if (oneSecTimerObjects[i].GameObject().activeSelf == true) {
                        oneSecTimerObjects[i].RefreshTime();
                    }
                }
            }
        }

        private void OnApplicationPause(bool pause) {
            if (pause == false) {    //On Resume
                Timer.Manual_Refresh();
            }
        }
    }

    public class EventPerSecond {
        public bool useBackgroundTimer; //액션 타겟이 없어져도 시간을 기록
        private OnOneSecondLater_WithRemainTime actionWithRemainTime;
        private OnOneSecondLater action;
        private bool useRemainTime;
        private GameObject referencedObject;

        public TimeSpan remainTime;

        public void SubtractSecond(int second) {
            remainTime -= TimeSpan.FromSeconds(second);
        }

        public EventPerSecond(bool useBackgroundTimer, OnOneSecondLater action) {
            useRemainTime = false;

            this.useBackgroundTimer = useBackgroundTimer;
            this.action = action;
        }

        public EventPerSecond(bool useBackgroundTimer, TimeSpan remainTime, OnOneSecondLater_WithRemainTime actionWithRemainTime) {
            useRemainTime = true;

            this.useBackgroundTimer = useBackgroundTimer;
            this.remainTime = remainTime;
            this.actionWithRemainTime = actionWithRemainTime;
        }

        public void CallEvent() {
            if (useRemainTime) {
                actionWithRemainTime(remainTime);
            } else {
                action();
            }
        }

        public void SetReferenceObejct(GameObject referencedObject) {
            this.referencedObject = referencedObject;
        }

        public bool IsAlive() {
            return referencedObject != null;
        }
    }

    public interface IUseTimer {
        void RefreshTime();
        GameObject GameObject();
    }
}