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

		private ePlayerEquipSlot _currentEquipSlot = ePlayerEquipSlot.None;
		public ePlayerEquipSlot CurrentEquipSlot { get { return this._currentEquipSlot; } }

		private Data_Item _currentEquipItem = null;

		protected override void OnAwake() {
			Global_Actor.SelfPlayer = this;

			// 공통데이터랑 연동해줌
			_actorInventory = Global_SelfPlayerData.SelfInventory;

			base.OnAwake();
		}

		/// <summary>
		/// 현재 장착중 슬롯 교체
		/// 퀵슬롯 그냥 사용과 다르게 현재 착용중인 슬롯과 같으면 아무것도 하지 않아야 한다.
		/// </summary>
		public void SetCurrentEquipSlot(int slotType) {
			int targetSlot = Inventory_SelfPlayer.ConvertQuickSlotIndexToSlotIndex(slotType);

			// 슬롯타입이 같고 아이템도 같으면 아무것도 안함.
			if (_currentEquipSlot == (ePlayerEquipSlot)slotType && _currentEquipItem == ActorInventory.Get(targetSlot)) {
				return;
			}

			// 발사부터 멈춤
			FireEnd();

			// 장착 슬롯 교체할때는 slottype 으로 바꿔줘야 한다.
			_currentEquipSlot = (ePlayerEquipSlot)slotType;
			_currentEquipItem = null;

			Data_Item targetItem = ActorInventory.Get(targetSlot);

			// target item 을 넘겨주면 알아서 체크해가지고 맨손처리 할것
			_playerArmObject.SetItem(targetItem);

			// 교체할 아이템이 없으면 맨손으로
			if (null == targetItem || false == targetItem.UseValidate(this)) {
				return;
			}

			BehaviourStart(targetItem);
		}

		/// <summary>
		/// 특정한 인덱스의 퀵슬롯을 사용한다.
		/// </summary>
		public void UseQuickSlot(int index) {

			int targetSlot = Inventory_SelfPlayer.ConvertQuickSlotIndexToSlotIndex(index);

			// 일단 아이템이 있는지부터 알아야..
			Data_Item targetItem = ActorInventory.Get(targetSlot);
			if (null == targetItem || false == targetItem.UseValidate(this)) {
				return;
			}

			// 발사부터 멈춤
			FireEnd();

			// 교체
			_currentEquipSlot = ePlayerEquipSlot.None;
			_currentEquipItem = null;

			// 사용 아이템은 UseValidate 를 한번 꼭 거쳐야 하기 때문에 arm object 에 item 세팅 부분을 이렇게 처리한다.
			if (null == targetItem || false == targetItem.UseValidate(this)) {
				_playerArmObject.SetBareHand();
				return;
			}

			// target item 을 넘겨주면 알아서 체크해가지고 맨손처리 할것
			_playerArmObject.SetItem(targetItem);

			BehaviourStart(targetItem);
		}

		public override void ChangeEquipSlotOnBehaviourEnd(int slotType) {
			ePlayerEquipSlot equipSlot = (ePlayerEquipSlot)slotType;

			// ㅋㅋㅋㅋㅋ
			if (equipSlot != ePlayerEquipSlot.Holster && 
				equipSlot != ePlayerEquipSlot.Knife &&
				equipSlot != ePlayerEquipSlot.PrimeWeapon &&
				equipSlot != ePlayerEquipSlot.SecondWeapon) {
				return;
			}

			_currentEquipItem = ActorInventory.Get(slotType);
			_currentEquipSlot = equipSlot;
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
			Data_Item fireTarget = _currentEquipItem;

			// 무기가 없음
			if (null == fireTarget || false == fireTarget.StatusData.Fireable) {
				yield break;
			}

			int currentEquipSlot = (int)_currentEquipSlot;
			_currentBehaviourCondition |= eBehaviourCondition.Firing;

			while (null != fireTarget) {

				// 달리는 중이라면 나감
				if (true == _currentBehaviourCondition.HasFlag(eBehaviourCondition.Running)) {
					break;
				}

				// 먼저 발사 가능한지 체크.
				eItemType firedItem = eItemType.None;
				if (eErrorCode.Fail == this.ActorInventory.TryFire((int)currentEquipSlot, out firedItem)) {
					break;
				}

				// 떄려야하는 타겟들
				int targetLayer = (1 << (int)eLayerMask.EnemyHittable) + (1 << (int)eLayerMask.OtherHittable);
				RaycastHit2D rays = Physics2D.Raycast(_playerArmObject.GetMuzzlePos, _sightDirection, 10, targetLayer);

				// 총알 날라가는 이펙트 부터..
				EffectInstanceInfo lineinfo = new EffectInstanceInfo(eEffectType.BulletLine);
				lineinfo.Pos = _playerArmObject.GetMuzzlePos;
				lineinfo.Duration = 0.1f;
				lineinfo.EndPos = lineinfo.Pos + ((Vector2)_sightDirection.normalized * 10);

				if (rays) {

					// dmgable 을 때리면 Hit 이후에 죽을 수가 있으니 처리에 조심하자.
					Damageable dmgable = rays.transform.GetComponent<Damageable>();
					if (null != dmgable) {
						dmgable.Hit(firedItem);
					}

					// 맞은곳에 탄흔 이펙트
					EffectInstanceInfo info = new EffectInstanceInfo(eEffectType.BulletSpark);
					info.Pos = rays.point;
					info.RotateType = eEffectRotateType.Normal;
					info.TargetNormal = rays.normal;
					info.Duration = 1f;

					Global_Effect.ShowEffect(info);

					// 총알 라인이펙트 목적지를 맞은곳으로 수정해준다.
					lineinfo.EndPos = rays.point;
				}

				// 총알 라인이펙트
				Global_Effect.ShowEffect(lineinfo);

				_playerArmObject.Fire();
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

			// 발사도 멈춰
			FireEnd();

			this._currentBehaviourCondition = eBehaviourCondition.None;
		}

		public void RefreshEquipItem() {
			SetCurrentEquipSlot((int)_currentEquipSlot);
		}
	}
}