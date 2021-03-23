using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

namespace EFN.Game {
	public class Actor_Player : Actor_Base {

		/// <summary>
		/// 현재 맵 시간, 날씨에 따라 실제 유저의 시야 밝기를 조절해줄 light
		/// </summary>
		[SerializeField] private Light2D _playerEnvironmentLight = default;

		/// <summary>
		/// 시야방향에 따라 움직여줄 팔 부분.
		/// </summary>
		[SerializeField] protected GameObject _playerArmObject = default;

		private void Update() {

			/*
			// 이 블럭은 추후 다른곳으로 빠질 예정.
			{
				_movDirection = Vector2.zero;

				if (Input.GetKey(KeyCode.W)) {
					_movDirection += Vector2.up;
				}

				if (Input.GetKey(KeyCode.D)) {
					_movDirection += Vector2.right;
				}

				if (Input.GetKey(KeyCode.A)) {
					_movDirection += Vector2.left;
				}

				if (Input.GetKey(KeyCode.S)) {
					_movDirection += Vector2.down;
				}

				this._sightDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Graphic.Pos).normalized;
			}
			*/

			PlayerMovementProcess();
			PlayerLookingProcess();
		}

		public void Move(InputAction.CallbackContext context) {
			this._movDirection = context.ReadValue<Vector2>();
		}

		public void View(InputAction.CallbackContext context) {
			this._sightDirection = (Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()) - Graphic.Pos).normalized;
		}

		private void PlayerMovementProcess() {
			this.transform.position = (Vector2)this.transform.position + (_movDirection.normalized * Time.deltaTime * 2f);
		}

		private void PlayerLookingProcess() {
			_playerArmObject.transform.rotation = Quaternion.FromToRotation(Vector2.up, this._sightDirection);
		}
	}
}