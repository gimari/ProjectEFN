using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

namespace EFN.Game {

    [RequireComponent(typeof(PlayerInput))]
    public class Actor_SelfPlayer : Actor_Player {

		[Header("* Actor_SelfPlayer ---------------")]
		/// <summary>
		/// 현재 맵 시간, 날씨에 따라 실제 유저의 시야 밝기를 조절해줄 light
		/// </summary>
		[SerializeField] private Light2D _playerEnvironmentLight = default;

		public void Move(InputAction.CallbackContext context) {
			this._movDirection = context.ReadValue<Vector2>();
		}

		public void View(InputAction.CallbackContext context) {
			this._sightDirection = (Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()) - Graphic.Pos).normalized;
		}
	}
}