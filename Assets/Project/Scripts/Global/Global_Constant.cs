using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
	public enum eSceneName {
		SceneIntro,
		SceneMain,
		SceneGame,
	}

	public class Global_Constant {
		public static string GetDirectory {
			get {
				return Application.dataPath + "/../";
			}
		}

		public const int MAX_QUICKSLOT_SIZE = 10;
		public const string TAG_SOUNDCOLLIDER = "SoundCollider";
	}
}