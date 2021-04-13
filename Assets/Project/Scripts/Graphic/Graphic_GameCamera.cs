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
		[SerializeField] private float _defaultSize = 5f;

		private static Graphic_GameCamera _mainCamera = null;
		private Camera _camera = null;

		private Vector2 _sightTarget = Vector2.zero;
		private Vector2 _actorTarget = Vector2.zero;

		private float _targetShakeAmount = 0;
		private float _shakeTimeAcc = 0;
		private float _targetCameraSize = 5;
		private bool _isZoom = false;

		public float GetCameraSize() {
			return this._defaultSize * Global_SelfPlayerData.GetSkillAmount(eSkillType.Sight);
		}

		private void Awake() {
			_mainCamera = this;
			_camera = this.GetComponent<Camera>();
		}

		public static void Shake(float amount) {
			if (null == _mainCamera) {
				return;
			}

			_mainCamera._targetShakeAmount = amount;
			_mainCamera._shakeTimeAcc = 0.05f;
		}

		public static void Zoom() {
			if (null == _mainCamera) {
				return;
			}

			_mainCamera._isZoom = !_mainCamera._isZoom;
		}

		public static void UserTrackProcess(Vector2 sightPos, Vector2 targetPos) {
			if (null == _mainCamera) {
				return;
			}

			_mainCamera._sightTarget = sightPos;
			_mainCamera._actorTarget = targetPos;
		}

		private void Update() {

			Vector3 cameraTarget;

			// 줌일때는 기존보다 약간 더 sight 방향으로 밀고 카메라 크기를 줄인다.
			// bool 로 대충 때움
			if (true == _isZoom) {
				cameraTarget = _sightTarget * 5 + _actorTarget;
				_camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, GetCameraSize() - 1.5f, Time.deltaTime * _cameraTrakingSpeed);
			} else {
				cameraTarget = _sightTarget + _actorTarget;
				_camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, GetCameraSize(), Time.deltaTime * _cameraTrakingSpeed);
			}

			// Shake 필요함?
			if (0 < _shakeTimeAcc) {
				cameraTarget = cameraTarget + Random.onUnitSphere * _targetShakeAmount;
				_shakeTimeAcc -= Time.deltaTime;
			}

			// 최종 결산
			// _camera.orthographicSize = _targetCameraSize;
			cameraTarget.z = _cameraDepth;
			_mainCamera.transform.localPosition = Vector3.Lerp(this.transform.position, cameraTarget, Time.deltaTime * _cameraTrakingSpeed);
		}
	}
}