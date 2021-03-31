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

		private ePlayerSlotType _currentEquipSlot = ePlayerSlotType.PrimeWeapon;
		public ePlayerSlotType CurrentEquipSlot { get { return this._currentEquipSlot; } }

		protected override void OnAwake() {
			Global_Actor.SelfPlayer = this;

			// 공통데이터랑 연동해줌
			_actorInventory = Global_SelfPlayerData.SelfInventory;

			base.OnAwake();
		}

		/// <summary>
		/// 현재 장착중 슬롯 교체
		/// </summary>
		public void SetCurrentEquipSlot(int slotType) {

			// 교체하면 발사하는거 끝내줘야 한다.
			FireEnd();

			// 교체
			_currentEquipSlot = (ePlayerSlotType)slotType;

			Data_Item equipItem = _actorInventory.Get((int)_currentEquipSlot);

			// 교체 쿨타임으로 usecooltime 을 사용한다.
			if (null != equipItem) {
				_actorui.CallUIEvent(eActorUIType.StartBehaviour, equipItem.StatusData.UseCoolTime);
			}
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

			// 발사 안하고 있을때만 돌려주자.
			if (false == this._currentBehaviourCondition.HasFlag(eBehaviourCondition.Firing)) {
				StartCoroutine(AutoFireRoutine());
			}
		}

		public override void FireEnd() {
			base.FireEnd();

			_currentBehaviourCondition &= ~eBehaviourCondition.Firing;
		}

		protected override IEnumerator AutoFireRoutine() {

			// 현재 장착중인 무기
			ePlayerSlotType currentEquipSlot = _currentEquipSlot;

			Data_Item fireTarget = this.ActorInventory.Get((int)currentEquipSlot);

			// 무기가 없음
			if (null == fireTarget || false == fireTarget.StatusData.Fireable) {
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

				yield return new WaitForSeconds(fireTarget.StatusData.FireRate);

				if (null == fireTarget) {
					break;
				}

				if (false == fireTarget.StatusData.ContinuousFire) {
					break;
				}

				// 발사 상태가 아니면 나감.
				if (false == _currentBehaviourCondition.HasFlag(eBehaviourCondition.Firing)) {
					break;
				}
			}

			_currentBehaviourCondition &= ~eBehaviourCondition.Firing;
		}

		public override void Stop() {
			base.Stop();
			this._currentBehaviourCondition = eBehaviourCondition.None;
		}
	}
}