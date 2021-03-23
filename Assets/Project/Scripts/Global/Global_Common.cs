using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EFN {
	public class Global_Common {

		[Conditional("EFN_DEBUG")]
		public static void Log(string str) {
			UnityEngine.Debug.Log(str);
		}

		[Conditional("EFN_DEBUG")]
		public static void LogError(string str) {
			UnityEngine.Debug.LogError(str);
		}

		public static AsyncOperation LoadSceneAsync(string sceneName) {
			Global_UIEvent.ClearUIEvent();
			return SceneManager.LoadSceneAsync(sceneName);
		}

		public static void LoadScene(string sceneName) {
			Global_UIEvent.ClearUIEvent();
			SceneManager.LoadScene(sceneName);
		}
	}
}