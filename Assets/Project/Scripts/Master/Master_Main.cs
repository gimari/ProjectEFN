using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Main {
    public class Master_Main : MonoBehaviour {

		private void Awake() {
#if !UNITY_EDITOR
			Cursor.lockState = CursorLockMode.None;
#endif
		}

		private void Start() {
			if (null == Global_SelfPlayerData.GameEndData) {
				Global_UIEvent.CallUIEvent(eEventType.OpenStartMenu);
			} else {
				Global_UIEvent.CallUIEvent(eEventType.OpenGameResult);
			}
		}
	}
}