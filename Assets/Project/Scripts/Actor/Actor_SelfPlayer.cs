using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace EFN.Game {

    [RequireComponent(typeof(PlayerInput))]
    public class Actor_SelfPlayer : Actor_Player {

		protected override void OnAwake() {
			Global_Actor.SelfPlayer = this;
			base.OnAwake();
		}

		[Header("* Actor_SelfPlayer ---------------")]
		/// <summary>
		/// 현재 맵 시간, 날씨에 따라 실제 유저의 시야 밝기를 조절해줄 light
		/// </summary>
		[SerializeField] private Light2D _playerEnvironmentLight = default;

		[SerializeField] private Camera _playerCamera = default;

		public override void SetSightDirection(Vector3 vec) {
			base.SetSightDirection(vec);

			_playerCamera.transform.localPosition = (new Vector3(this._sightDirection.x, this._sightDirection.y, -10));
		}
	}
}