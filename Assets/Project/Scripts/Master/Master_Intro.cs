using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
	public class Master_Intro : MonoBehaviour {
		
		private void Start() {
			StartCoroutine(this.IntroProcess());
		}

		private IEnumerator IntroProcess() {
			Global_UIEvent.CallUIEvent(ePermanetEventType.HideFade);
			yield return new WaitForSeconds(4);

			Global_UIEvent.CallUIEvent<string>(ePermanetEventType.TryChangeScene, eSceneName.SceneMain.ToString());
		}
	}
}