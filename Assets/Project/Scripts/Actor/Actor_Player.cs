using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

namespace EFN.Game {

	[Flags]
	public enum eBehaviourCondition {
		None = 0,				// 아무것도 안함
		Running = 1 << 0,		// 달리고 있음
		Firing = 1 << 1,		// 발사하고 있음
		Damaging = 1 << 2,		// 데미지를 입고 있음
		Walking = 1 << 3,		// 걷고 있음
	}

	public class PlayerBehaviourData {
		public float UseCoolTime = 0;
		public eBehaviourCondition CancelCondition = eBehaviourCondition.None;
		public Action OnSuccessBehaviour = null;
	}

	public class Actor_Player : Actor_Base {

		[Header("Components")]

		/// <summary>
		/// 시야방향에 따라 움직여줄 팔 부분.
		/// </summary>
		[SerializeField] protected Graphic_PlayerArm _playerArmObject = default;
		public Graphic_PlayerArm PlayerArmObject { get { return _playerArmObject; } }

		/// <summary>
		/// HAT 탈부탁 해주는 부분.
		/// </summary>
		[SerializeField] protected Graphic_PlayerHat _playerHatObject = default;
		
		/// <summary>
		/// 이 액터에 붙어있는 액터UI
		/// </summary>
		[SerializeField] protected ActorUI_Base _actorui = default;

		/// <summary>
		/// 이 액터의 체력을 관장해줄 컴포넌트
		/// </summary>
		[SerializeField] protected Damageable _dmgable = default;
		public Damageable Dmgable { get { return _dmgable; } }

		/// <summary>
		/// 액터가 소리를 낼 때 주변에 전파해줄 generator
		/// </summary>
		[SerializeField] protected SoundGenerator _soundGenerator = default;

		protected Coroutine _behaviourRoutine = null;

		/// <summary>
		/// 현재 이 플레이어가 취하고 있는 행동
		/// </summary>
		protected eBehaviourCondition _currentBehaviourCondition = eBehaviourCondition.None;
		public eBehaviourCondition CurrentBehaviourCondition {
			get { return _currentBehaviourCondition; }
			set { _currentBehaviourCondition = value; } 
		}

		protected override void OnAwake() {
			base.OnAwake();

			_dmgable.Init(this);
			_dmgable.OnDieInAction = this.OnDieInAction;
			_dmgable.OnReceiveDamage = this.OnReceiveDamage;
			_dmgable.OnReceiveHeal = this.OnReceiveHeal;
		}

		protected virtual void Update() {
			PlayerMovementProcess();
			PlayerLookingProcess();
		}

		protected virtual void OnDieInAction(DamageInfo hitinfo) {
			Destroy(this.gameObject);
		}

		protected virtual void OnReceiveDamage(DamageInfo hitinfo) { }

		protected virtual void OnReceiveHeal() { }

		protected virtual void PlayerMovementProcess() {

			// 간단하게 달리기 상태 구현
			float moveSpeed = this._currentBehaviourCondition.HasFlag(eBehaviourCondition.Running) ? 5 : 3;

			this.transform.position = (Vector2)this.transform.position + (_movDirection.normalized * Time.deltaTime * moveSpeed);
		}

		protected virtual void PlayerLookingProcess() {
			Graphic.Flip(0 < this._sightDirection.x);

			_playerArmObject.transform.rotation = Quaternion.FromToRotation(Vector2.up, this._sightDirection);
		}

		public virtual void FireStart() { }
		public virtual void FireEnd() { }
		protected virtual IEnumerator AutoFireRoutine() { yield return null; }

		public virtual void BehaviourStart(Data_Item item) {

			if (null == item) {
				return;
			}

			// 원래 하던거 멈춤!!
			BehaviourStop();

			PlayerBehaviourData behaviourData = new PlayerBehaviourData();
			behaviourData.UseCoolTime = item.StatusData.UseCoolTime;
			behaviourData.CancelCondition = item.StatusData.CancelCondition;
			behaviourData.OnSuccessBehaviour = () => {
				// 아이템 사용이 정상적으로 이루어 지면 관련 처리 해준다.
				if (true == item.UseValidate(this)) {
					item.StatusData.OnEndItemUsed(this, item);
					ChangeEquipSlotOnBehaviourEnd(item.SlotIndex);
				}
			};

			this._behaviourRoutine = StartCoroutine(PlayerBehaviourRoutine(behaviourData));
		}

		public virtual void BehaviourStop() {
			if (null != _behaviourRoutine) {
				_actorui.CallUIEvent(eActorUIType.EndBehaviour);
				StopCoroutine(_behaviourRoutine);
			}
		}

		protected virtual IEnumerator PlayerBehaviourRoutine(PlayerBehaviourData behaviourData) {

			if (null == behaviourData) {
				yield break;
			}

			if (null != _actorui) {
				_actorui.CallUIEvent(eActorUIType.StartBehaviour, behaviourData.UseCoolTime);
			}

			float targetTimer = behaviourData.UseCoolTime;

			while (null != behaviourData && null != this.gameObject) {

				// 현재 행동이 취소될만한 행동이면 나감
				if (eBehaviourCondition.None != (behaviourData.CancelCondition & _currentBehaviourCondition)) {
					break;
				}

				targetTimer -= Time.deltaTime;

				// 정상적으로 타이머가 다 돌았을 때..
				if (targetTimer < 0) {
					behaviourData.OnSuccessBehaviour?.Invoke();
					break;
				}

				yield return null;
			}

			if (null != _actorui) {
				_actorui.CallUIEvent(eActorUIType.EndBehaviour);
			}
		}

		public virtual void RunStart() {
			this._currentBehaviourCondition |= eBehaviourCondition.Running;
		}

		public virtual void RunEnd() {
			this._currentBehaviourCondition &= ~eBehaviourCondition.Running;
		}

		/// <summary>
		/// 행동이 끝날때 행동 대상이 되는 아이템의 슬롯으로 equipslot 를 맞춰주는거
		/// 사실상 selfplayer 말곤 쓸일이 없다
		/// </summary>
		public virtual void ChangeEquipSlotOnBehaviourEnd(int slotType) { }

		public virtual int ArmorAmount() { return 0; }
		public virtual int MaxHealthPoint() { return 100; }
	}
}