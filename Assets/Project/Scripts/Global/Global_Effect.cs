using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EFN {

	public enum eEffectType {
		None = 0,
		BulletSpark = 1,
		BulletLine,
		SpikyBurst,
		ExplosionNova,
	}

	public enum eEffectRotateType {
		None = 0,		// 회전 안함
		Normal = 1,		// 노말로 회전
	}

	public struct EffectInstanceInfo {

		// 위치
		private Vector2 _pos;
		public Vector2 Pos {
			get { return _pos; }
			set { _pos = value; }
		}

		// 기간
		private float _duration;
		public float Duration {
			get { return _duration; }
			set { _duration = value; }
		}

		// 끝나는 포지션
		private Vector2 _endPos;
		public Vector2 EndPos {
			get { return _endPos; }
			set { _endPos = value; }
		}

		// 회전각 뭘로 돌릴건지
		private eEffectRotateType _rotateType;
		public eEffectRotateType RotateType {
			get { return _rotateType; }
			set { _rotateType = value; }
		}

		// 회전각 노말벡터
		private Vector2 _targetNormal;
		public Vector2 TargetNormal {
			get { return _targetNormal; }
			set { _targetNormal = value; }
		}

		// 이펙트타입
		private eEffectType _effectType;
		public eEffectType EffectType {
			get { return _effectType; }
		}

		public static implicit operator bool(EffectInstanceInfo info) {
			return info._effectType != eEffectType.None;
		}

		public EffectInstanceInfo(eEffectType targetType) {
			this._effectType = targetType;
			this._pos = default;
			this._duration = 0;
			this._targetNormal = default;
			this._rotateType = eEffectRotateType.None;
			this._endPos = default;
		}
	}

	public class Global_Effect : MonoBehaviour {

		[EnumNamedArray(typeof(eEffectType))]
		[SerializeField]
		private GameObject[] _effectList = default;

		private static Global_Effect _instance;
		private void Awake() {
			_instance = this;
		}

		public static void ShowEffect(EffectInstanceInfo info) {
			if (null == _instance) {
				EFN.Global_Common.LogError("CANNOT SHOW EFFECT : NO GLOBAL INSTNACE");
				return;
			}

			if (false == info) {
				return;
			}

			Effect_Base effect = Instantiate(_instance._effectList[(int)info.EffectType], _instance.transform).GetComponent<Effect_Base>();
			if (null == effect) {
				EFN.Global_Common.LogError("CANNOT SHOW EFFECT : NO ATTACHMENT IN EFFECT PREFAB");
				return;
			}

			effect.SetInfo(info);
		}
	}
}