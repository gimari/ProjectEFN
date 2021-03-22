using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
	public class Master_Intro : MonoBehaviour {
		private void Start() {
			this.IntroProcess();
		}

		private void IntroProcess() {
			Global_Common.LoadScene(eSceneName.SceneMain.ToString());
		}
	}
}