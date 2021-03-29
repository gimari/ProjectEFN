using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace EFN.Game {

    public class Actor_SelfPlayer : Actor_Player {

		[Header("* Actor_SelfPlayer ---------------")]
		/// <summary>
		/// 현재 맵 시간, 날씨에 따라 실제 유저의 시야 밝기를 조절해줄 light
		/// </summary>
		[SerializeField] private Light2D _playerEnvironmentLight = default;

		protected override void OnAwake() {
			Global_Actor.SelfPlayer = this;

			// 공통데이터랑 연동해줌
			_actorInventory = Global_SelfPlayerData.SelfInventory;

			base.OnAwake();
		}

		protected override void PlayerMovementProcess() {
			base.PlayerMovementProcess();
			Graphic_GameCamera.UserTrackProcess(this._sightDirection, Graphic.Pos);
		}

		protected override void PlayerLookingProcess() {
			base.PlayerLookingProcess();
			Graphic_GameCamera.UserTrackProcess(this._sightDirection, Graphic.Pos);
		}

		public override void Fire() {
			base.Fire();

			RaycastHit2D rays = Physics2D.Raycast(_muzzle.position, _sightDirection, 10, 1 << (int)eLayerMask.Wall);

			if (rays) {

				EffectInstanceInfo info = new EffectInstanceInfo(eEffectType.BulletSpark);
				info.Pos = rays.point;
				info.RotateType = eEffectRotateType.Normal;
				info.TargetNormal = rays.normal;
				info.Duration = 1f;

				Global_Effect.ShowEffect(info);
			}
		}
	}
}