using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EFN {

	public enum eErrorCode {
		Success = 0,				// 단순 성공
		Fail = 1,					// 예외처리가 필요 없는 단순실패. 해당 상황이 절대 일어나면 안되는 경우.

		StackOverflow,
		InventoryFull,				// 인벤 꽉참
		NotenoughCoke,				// 돈없음
	}

	public enum eLayerMask {
		Default = 0,
		UI = 5,
		Interactable = 8,
		OtherHittable = 9,			// 유저 말고 다른 때릴곳 (벽, AI 등)
		UserHitbox = 10,			// 유저 히트박스
		ShowBlockable = 11,			// 때릴수는 없는데 시야를 방해할 수 있는 것 (풀, 바위 등)
		EnemyHittable = 12,			// 적군
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

		[Conditional("EFN_DEBUG")]
		public static void DrawLine(Vector3 start, Vector3 end) {
			UnityEngine.Debug.DrawLine(start, end, Color.red, 0.2f);
		}

		public static AsyncOperation LoadSceneAsync(string sceneName) {
			Global_UIEvent.ClearUIEvent();
			return SceneManager.LoadSceneAsync(sceneName);
		}

		public static void LoadScene(string sceneName) {
			Global_UIEvent.ClearUIEvent();
			SceneManager.LoadScene(sceneName);
		}

		public static void Quit() {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}

	public static class Vector2Extension {
		public static Vector2 Rotate(this Vector2 v, float degrees) {
			float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
			float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

			float tx = v.x;
			float ty = v.y;
			v.x = (cos * tx) - (sin * ty);
			v.y = (sin * tx) + (cos * ty);
			return v;
		}
	}
}