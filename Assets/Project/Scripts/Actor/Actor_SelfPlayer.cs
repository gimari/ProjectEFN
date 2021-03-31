﻿using System.Collections;
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

		private Coroutine _fireRoutine = null;
		private bool _fireEndWaitFlag = false;

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

		public override void FireStart() {
			base.FireStart();

			this._fireEndWaitFlag = false;

			// 발사 안하고 있을때만 돌려주자.
			if (false == this._currentBehaviourCondition.HasFlag(eBehaviourCondition.Firing)) {
				StartCoroutine(AutoFireRoutine());
			}
		}

		public override void FireEnd() {
			base.FireEnd();

			this._fireEndWaitFlag = true;
		}

		protected override IEnumerator AutoFireRoutine() {

			// 현재 장착중인 무기
			ePlayerSlotType currentEquipSlot = ePlayerSlotType.Holster;

			Data_Item fireTarget = this.ActorInventory.Get((int)currentEquipSlot);

			// 무기가 없음
			if (null == fireTarget) {
				yield break;
			}

			_currentBehaviourCondition |= eBehaviourCondition.Firing;

			while (null != fireTarget) {

				// 먼저 발사 가능한지 체크.
#if EFN_DEBUG
				// 무한총알 치트 체크
				if (eErrorCode.Fail == this.ActorInventory.TryFire((int)currentEquipSlot) && false == Global_DebugConfig.InfiniteBullet) {
					break;
				}
#else
				if (eErrorCode.Fail == this.ActorInventory.TryFire((int)currentEquipSlot)) {
					break;
				}
#endif

				RaycastHit2D rays = Physics2D.Raycast(_muzzle.position, _sightDirection, 10, 1 << (int)eLayerMask.Wall);

				if (rays) {

					EffectInstanceInfo info = new EffectInstanceInfo(eEffectType.BulletSpark);
					info.Pos = rays.point;
					info.RotateType = eEffectRotateType.Normal;
					info.TargetNormal = rays.normal;
					info.Duration = 1f;

					Global_Effect.ShowEffect(info);
				}

				Graphic_GameCamera.Shake(5);

				yield return new WaitForSeconds(fireTarget.StatusData.UseCoolTime);

				if (null == fireTarget) {
					break;
				}

				if (false == fireTarget.StatusData.ContinuousFire) {
					break;
				}

				if (true == _fireEndWaitFlag) {
					break;
				}
			}

			_currentBehaviourCondition &= ~eBehaviourCondition.Firing;
		}
	}
}