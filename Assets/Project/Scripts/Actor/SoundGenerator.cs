using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace EFN.Game {

	public class SoundGeneratingData {
		private float _radius;
		public float Radius {
			get { return _radius; }
			set { _radius = value; }
		}

		private float _soundAmount;
		public float SoundAmount {
			get { return _soundAmount; }
			set { _soundAmount = value; }
		}

		private float _endTimer = 0.1f;
		public float EndTimer {
			get { return _endTimer; }
			set { _endTimer = value; }
		}
	}

	[RequireComponent(typeof(CircleCollider2D))]
	public class SoundGenerator : MonoBehaviour {

		[SerializeField] private CircleCollider2D _soundCollider = default;

		private SoundGeneratingData _generateData = null;
		public SoundGeneratingData GenerateData { get { return _generateData; } }

		private Coroutine _soundRoutine = null;

		private void Awake() {
			this.tag = Global_Constant.TAG_SOUNDCOLLIDER;
			_soundCollider.enabled = false;
		}

		public void MakeSound(SoundGeneratingData data) {

			this._generateData = data;

			_soundCollider.enabled = true;
			_soundCollider.radius = data.Radius;

			if (null != _soundRoutine) {
				StopCoroutine(_soundRoutine);
			}

			this._soundRoutine = StartCoroutine(SoundEndRoutine(data.EndTimer));
		}

		private IEnumerator SoundEndRoutine(float endTimer) {

			yield return new WaitForSeconds(endTimer);

			this._soundCollider.radius = 0f;
			this._soundCollider.enabled = false;
			_generateData = null;
		}
	}
}