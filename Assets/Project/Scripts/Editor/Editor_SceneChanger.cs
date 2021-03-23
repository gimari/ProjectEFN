using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EFN {
	public class Editor_SceneChanger {

		[MenuItem("Scene/SceneIntro")]
		private static void SceneIntro() {
			EditorSceneManager.OpenScene("Assets/Project/Scenes/SceneIntro.unity");
		}

		[MenuItem("Scene/SceneGame")]
		private static void SceneGame() {
			EditorSceneManager.OpenScene("Assets/Project/Scenes/SceneGame.unity");
		}

		[MenuItem("Scene/SceneMain")]
		private static void SceneMain() {
			EditorSceneManager.OpenScene("Assets/Project/Scenes/SceneMain.unity");
		}
	}
}