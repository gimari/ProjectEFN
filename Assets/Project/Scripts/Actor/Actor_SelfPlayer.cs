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

			UpdateHatObject();

			base.OnAwake();
		}

		protected override void Update() {
			base.Update();

			this.transform.rotation = Quaternion.Euler(90, 0, 0);
		}

		protected void UpdateHatObject() {
			// 모자 갱신
			_playerHatObject.SetItem(_actorInventory.Get((int)ePlayerSlotType.Head));
		}

		/// <summary>
		/// 현재 장착중 슬롯 교체
		/// 퀵슬롯 그냥 사용과 다르게 현재 착용중인 슬롯과 같으면 아무것도 하지 않아야 한다.
		/// </summary>
		public void SetCurrentEquipSlot(int slotType) {

			if (0 == slotType) {
				return;
			}

			// 슬롯타입이 같고 아이템도 같으면 아무것도 안함.
			if (_currentEquipSlot == (ePlayerEquipSlot)slotType && _currentEquipItem == ActorInventory.Get(slotType)) {
				return;
			}

			// 원래 하던거 멈춤!!
			BehaviourStop();

			// 발사부터 멈춤
			FireEnd();

			// 장착 슬롯 교체할때는 slottype 으로 바꿔줘야 한다.
			_currentEquipSlot = (ePlayerEquipSlot)slotType;
			_currentEquipItem = null;

			Data_Item targetItem = ActorInventory.Get(slotType);

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
			Global_UIEvent.CallUIEvent(eEventType.OnPlayerSwapWeapon);
		}

		protected override void PlayerMovementProcess() {
			base.PlayerMovementProcess();

			Graphic_GameCamera.UserTrackProcess(this._sightDirection, Graphic.Pos);

			// 간단하게 애니메이션 함..
			if (_movDirection == Vector2.zero) {
				Graphic.PlayIdle();
			} else if (true == _currentBehaviourCondition.HasFlag(eBehaviourCondition.Running)) {
				Graphic.PlayRun();
			} else {
				Graphic.PlayWalk();
			}

			if (true == _currentBehaviourCondition.HasFlag(eBehaviourCondition.Running)) {
				SoundGeneratingData sgd = new SoundGeneratingData();
				sgd.Radius = 5f;
				this._soundGenerator.MakeSound(sgd);
			}
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

		public void ReloadEquipWeapon() {

			// 현재 장착중인 무기
			Data_Item fireTarget = _currentEquipItem;

			// 무기가 없음
			if (null == fireTarget) {
				return;
			}

			// 꽉 차있으면 안함
			if (fireTarget.StatusData.MaxRoundAmount <= fireTarget.FireModule.AmmoCount) {
				return;
			}

			// 원래 하던거 멈춤!!
			BehaviourStop();

			PlayerBehaviourData behaviourData = new PlayerBehaviourData();
			behaviourData.UseCoolTime = fireTarget.StatusData.ReloadTime;
			behaviourData.OnSuccessBehaviour = () => {
				this.ActorInventory.TryReload(fireTarget.SlotIndex);
			};

			this._behaviourRoutine = StartCoroutine(PlayerBehaviourRoutine(behaviourData));
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

				Global_UIEvent.CallUIEvent(eEventType.OnPlayerShoot);

				switch (fireTarget.StatusData.WeaponType) {
					case eWeaponCategory.Common:
						this.ShootGunWeapon(firedItem);
						break;
					case eWeaponCategory.Knife:
						this.ShootKnifeWeapon(fireTarget.ItemType);
						break;
					case eWeaponCategory.ShotGun:
						this.ShootShotGunWeapon(fireTarget.StatusData, firedItem);
						break;
				}
				
				// arm 에서 발사 애니메이션 재생해야함
				_playerArmObject.Fire();

				// 연사속도 기다린다.
				yield return new WaitForSeconds(fireTarget.StatusData.FireRate);

				// 지금 쏠 총이 없으면 나감
				if (null == fireTarget) {
					break;
				}

				// 총이 자동발사 안되면 나감
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

		/// <summary>
		/// 총알 안쓰는 무기를 발사한다!
		/// </summary>
		private void ShootKnifeWeapon(eItemType firedItem) {

			// 떄려야하는 타겟들
			int targetLayer = (1 << (int)eLayerMask.EnemyHittable) + (1 << (int)eLayerMask.OtherHittable);
			RaycastHit2D rays = Physics2D.Raycast(_playerArmObject.GetMuzzlePos, _sightDirection, 1.5f, targetLayer);
			
			// ray 를 쏴서 맞을 놈이 있는지 검사.
			if (rays) {

				// dmgable 을 때리면 Hit 이후에 죽을 수가 있으니 처리에 조심하자.
				Damageable dmgable = rays.transform.GetComponent<Damageable>();
				if (null != dmgable) {
					dmgable.Hit(firedItem, this);
				}

				// 맞은곳에 탄흔 이펙트
				EffectInstanceInfo info = new EffectInstanceInfo(eEffectType.BulletSpark);
				info.Pos = rays.point;
				info.RotateType = eEffectRotateType.Normal;
				info.TargetNormal = rays.normal;
				info.Duration = 1f;

				Global_Effect.ShowEffect(info);
			}
			
			// 카메라 반동
			Graphic_GameCamera.Shake(3);
		}

		/// <summary>
		/// 총알 쓰는 무기를 발사한다!!
		/// </summary>
		private void ShootGunWeapon(eItemType firedItem) {

			// 떄려야하는 타겟들
			int targetLayer = (1 << (int)eLayerMask.EnemyHittable) + (1 << (int)eLayerMask.OtherHittable);
			RaycastHit2D rays = Physics2D.Raycast(_playerArmObject.GetMuzzlePos, _sightDirection, 10, targetLayer);

			// 총알 날라가는 이펙트 부터 우선 생성
			EffectInstanceInfo lineinfo = new EffectInstanceInfo(eEffectType.BulletLine);
			lineinfo.Pos = _playerArmObject.GetMuzzlePos;
			lineinfo.Duration = 0.1f;
			lineinfo.EndPos = lineinfo.Pos + ((Vector2)_sightDirection.normalized * 10);

			// ray 를 쏴서 맞을 놈이 있는지 검사.
			if (rays) {

				// dmgable 을 때리면 Hit 이후에 죽을 수가 있으니 처리에 조심하자.
				Damageable dmgable = rays.transform.GetComponent<Damageable>();
				if (null != dmgable) {
					dmgable.Hit(firedItem, this);
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

			// 총 사운드
			SoundGeneratingData sgd = new SoundGeneratingData();
			sgd.Radius = 10f;
			sgd.EndTimer = 0.1f;
			_soundGenerator.MakeSound(sgd);

			// 카메라 반동
			Graphic_GameCamera.Shake(5);
		}

		/// <summary>
		/// 샷건 발사!!
		/// </summary>
		private void ShootShotGunWeapon(Status_Base gunStatus, eItemType firedItem) {

			// 떄려야하는 타겟들
			int targetLayer = (1 << (int)eLayerMask.EnemyHittable) + (1 << (int)eLayerMask.OtherHittable);

			// 샷건은 한번에 여러발을 쏴야 한다.
			for (int idx = 0; idx < gunStatus.FireRoundsInSingle; idx++) {

				Vector2 rayDir = Vector2Extension.Rotate(_sightDirection, Random.Range(-15f, 15f));
				RaycastHit2D rays = Physics2D.Raycast(_playerArmObject.GetMuzzlePos, rayDir, 10, targetLayer);

				// 총알 날라가는 이펙트 부터 우선 생성
				EffectInstanceInfo lineinfo = new EffectInstanceInfo(eEffectType.BulletLine);
				lineinfo.Pos = _playerArmObject.GetMuzzlePos;
				lineinfo.Duration = 0.1f;
				lineinfo.EndPos = lineinfo.Pos + (rayDir.normalized * 10);

				// ray 를 쏴서 맞을 놈이 있는지 검사.
				if (rays) {

					// dmgable 을 때리면 Hit 이후에 죽을 수가 있으니 처리에 조심하자.
					Damageable dmgable = rays.transform.GetComponent<Damageable>();
					if (null != dmgable) {
						dmgable.Hit(firedItem, this);
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
			}

			// 총 사운드
			SoundGeneratingData sgd = new SoundGeneratingData();
			sgd.Radius = 10f;
			sgd.EndTimer = 0.1f;
			_soundGenerator.MakeSound(sgd);

			// 카메라 반동
			Graphic_GameCamera.Shake(5);
		}

		/// <summary>
		/// selfplayer의 죽음
		/// </summary>
		protected override void OnDieInAction(DamageInfo hitinfo) {

#if EFN_DEBUG
			if (true == Global_DebugConfig.Invinsible) {
				return;
			}
#endif
			
			// 할거 다하고 죽는다.
			Destroy(this.gameObject);

			Global_SelfPlayerData.SetKilledInAction();
		}

		public override void Stop() {
			base.Stop();

			// 발사도 멈춰
			FireEnd();

			// idle 재생
			Graphic.PlayIdle();

			this._currentBehaviourCondition = eBehaviourCondition.None;
		}

		/// <summary>
		/// 현재 이큅슬롯으로 현재 장착중인 아이템을 갱신해준다.
		/// </summary>
		public void RefreshEquipItem() {
			SetCurrentEquipSlot((int)_currentEquipSlot);

			// 모자도 갱신해준다.
			UpdateHatObject();
		}

		protected override void OnReceiveDamage(DamageInfo hitinfo) {
			base.OnReceiveDamage(hitinfo);
			Global_UIEvent.CallUIEvent(eEventType.OnPlayerDamageTaken);
		}

		protected override void OnReceiveHeal() {
			base.OnReceiveHeal();
			Global_UIEvent.CallUIEvent(eEventType.OnPlayerDamageTaken);
		}

		public override int ArmorAmount() {
			return (int)Global_SelfPlayerData.GetSkillAmount(eSkillType.Armor);
		}

		public override int MaxHealthPoint() {
			return 100 + (int)Global_SelfPlayerData.GetSkillAmount(eSkillType.Health);
		}
	}
}