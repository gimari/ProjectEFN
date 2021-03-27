using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EFN {

	public enum eEffect {
		None = 0,
		BulletSpark = 1,
	}

	public struct EffectInstanceInfo {

		// 위치
		private Vector2 _pos;

		// 기간
		private float _duration;

		// 회전각 노말벡터
		private Vector2 _targetNormal;
	}

	public class Global_Effect : MonoBehaviour {


		private static Global_Effect _instance;
		private void Awake() {
			_instance = this;
		}

		public static void ShowEffect(EffectInstanceInfo info) {
			if (null == _instance) {
				EFN.Global_Common.LogError("CANNOT SHOW EFFECT : NO GLOBAL INSTNACE");
				return;
			}


		}
	}
}