using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EFN {

	public enum eErrorCode {
		Success = 0,				// 단순 성공
		Fail = 1,					// 예외처리가 필요 없는 단순실패. 해당 상황이 절대 일어나면 안되는 경우.

		StackOverflow = 10,
	}

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