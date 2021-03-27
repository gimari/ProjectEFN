using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace EFN.Game {

	[RequireComponent(typeof(Camera))]
	public class Graphic_GameCamera : MonoBehaviour {

		[Header("Config")]
		[SerializeField] private int _cameraDepth = 10;
		[SerializeField] private float _cameraTrakingSpeed = 5f;

		private static Graphic_GameCamera _mainCamera = null;

		private Vector3 _cameraTarget = Vector3.zero;
		private float _targetShakeAmount = 0;
		private float _shakeTimeAcc = 0;

		private void Awake() {
			_mainCamera = this;
		}

		public static void Shake(float amount) {
			if (null == _mainCamera) {
				return;
			}

			_mainCamera._targetShakeAmount = amount;
			_mainCamera._shakeTimeAcc = 0.05f;
		}

		public static void UserTrackProcess(Vector2 targetPos) {
			if (null == _mainCamera) {
				return;
			}

			_mainCamera._cameraTarget.x = targetPos.x;
			_mainCamera._cameraTarget.y = targetPos.y;
		}

		private void Update() {

			if (0 < _shakeTimeAcc) {
				_cameraTarget = _cameraTarget + Random.onUnitSphere * _targetShakeAmount;
				_shakeTimeAcc -= Time.deltaTime;
			}

			_cameraTarget.z = _cameraDepth;
			_mainCamera.transform.localPosition = Vector3.Lerp(this.transform.position, _cameraTarget, Time.deltaTime * _cameraTrakingSpeed);
		}
	}
}