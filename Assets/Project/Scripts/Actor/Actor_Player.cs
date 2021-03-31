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

	public class Actor_Player : Actor_Base {

		[Header("Components")]

		/// <summary>
		/// 시야방향에 따라 움직여줄 팔 부분.
		/// </summary>
		[SerializeField] protected GameObject _playerArmObject = default;

		/// <summary>
		/// 총알 실제로 날라갈 총구부분
		/// </summary>
		[SerializeField] protected Transform _muzzle = default;

		/// <summary>
		/// 현재 이 플레이어가 취하고 있는 행동
		/// </summary>
		protected eBehaviourCondition _currentBehaviourCondition;

		private void Update() {
			PlayerMovementProcess();
			PlayerLookingProcess();
		}

		protected virtual void PlayerMovementProcess() {

			// 간단하게 달리기 상태 구현
			float moveSpeed = this._currentBehaviourCondition.HasFlag(eBehaviourCondition.Running) ? 5 : 2;

			this.transform.position = (Vector2)this.transform.position + (_movDirection.normalized * Time.deltaTime * moveSpeed);
		}

		protected virtual void PlayerLookingProcess() {
			Graphic.Flip(0 < this._sightDirection.x);

			_playerArmObject.transform.rotation = Quaternion.FromToRotation(Vector2.up, this._sightDirection);
		}

		public virtual void FireStart() { }
		public virtual void FireEnd() { }
		protected virtual IEnumerator AutoFireRoutine() { yield return null; }
		protected virtual IEnumerator PlayerBehaviourRoutine() { yield return null; }

		public virtual void RunStart() {
			this._currentBehaviourCondition |= eBehaviourCondition.Running;
		}

		public virtual void RunEnd() {
			this._currentBehaviourCondition &= ~eBehaviourCondition.Running;
		}
	}
}