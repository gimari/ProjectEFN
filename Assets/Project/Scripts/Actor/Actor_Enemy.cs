using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace EFN.Game {
    public class Actor_Enemy : Actor_Player {

		[Header("Enemy Components")]
		[SerializeField]
		private NavMeshAgent _enemyAgent = default;

		[Header("Scanning settings")]
		[Tooltip("The angle of the forward of the view cone. 0 is forward of the sprite, 90 is up, 180 behind etc.")]
		[Range(0.0f, 360.0f)]
		public float _viewFov;
		public float _viewDistance;
		[Tooltip("Time in seconds without the target in the view cone before the target is considered lost from sight")]
		public float _timeBeforeTargetLost = 3.0f;

		[Header("Enemy Status")]
		[SerializeField] private eItemType _wearingArmor = default;
		public eItemType WearingArmor { get { return this._wearingArmor; } }

		[SerializeField] private eItemType _usingBullet = default;
		public eItemType UsingBullet { get { return this._usingBullet; } }

		private Actor_Player _targetPlayer = null;

		protected override void OnAwake() {
			base.OnAwake();

			StartCoroutine(StopAndFiringRandomTargetRoutine());
		}


		private void FixedUpdate() {
			ScanForPlayer();
			ChaseForPlayer();
		}

		protected void ChaseForPlayer() {
			_enemyAgent.isStopped = _targetPlayer == null;

			if (null ==_targetPlayer) {
				return;
			}

			_enemyAgent.SetDestination(_targetPlayer.transform.position);

			if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance) {
				_enemyAgent.velocity = _randTarget;
			}
		}

		private Vector2 _randTarget;
		private IEnumerator StopAndFiringRandomTargetRoutine() {
			while (this.gameObject != null) {

				if (null == _targetPlayer) {
					yield return null;
					continue;
				}

				_randTarget = Vector2Extension.Rotate((this.transform.position - _targetPlayer.transform.position), Random.Range(-40, 40));
				_randTarget = ((_randTarget.normalized * (_enemyAgent.stoppingDistance)) + (Vector2)_targetPlayer.transform.position) - (Vector2)this.transform.position;
				yield return new WaitForSeconds(Random.Range(0.8f, 1f));

				_randTarget = Vector2.zero;
				yield return new WaitForSeconds(Random.Range(1f, 2f));
			}
		}

		public void ScanForPlayer() {
			if (null == Global_Actor.SelfPlayer) {
				_targetPlayer = null;
				return;
			}

			Vector3 dir = Global_Actor.SelfPlayer.Graphic.transform.position - this.Graphic.transform.position;

			if (dir.sqrMagnitude > _viewDistance * _viewDistance) {
				_targetPlayer = null;
				return;
			}

			float angle = Vector2.Angle(_sightDirection.normalized, dir);

			if (angle > _viewFov * 0.5f) {
				_targetPlayer = null;
				return;
			}

			// AI 는 시야를 방해하는 물건 혹은 벽 등을 사이에 끼면 못 봐야 한다
			int targetLayer = (1 << (int)eLayerMask.ShowBlockable) + (1 << (int)eLayerMask.OtherHittable) + (1 << (int)eLayerMask.UserHitbox);

			RaycastHit2D rays = Physics2D.Raycast(this.Graphic.transform.position, dir, dir.magnitude, targetLayer);

			Global_Common.DrawLine(Global_Actor.SelfPlayer.Graphic.transform.position, this.Graphic.transform.position);

			// 레이를 쏴서 시야를 가리는놈이 있는지 체크
			if (rays && rays.transform.gameObject.layer != (int)eLayerMask.UserHitbox) {
				_targetPlayer = null;
				return;
			}

			_sightDirection = dir.normalized;
			_targetPlayer = Global_Actor.SelfPlayer;
		}


#if UNITY_EDITOR
		private void OnDrawGizmosSelected() {

			//draw the cone of view
			Vector3 forward;

			if (_sightDirection == default(Vector2)) {
				forward = Vector2.right;
			} else {
				forward = _sightDirection;
			}
			
			Vector3 endpoint = this.Graphic.transform.position + (Quaternion.Euler(0, 0, _viewFov * 0.5f) * forward);

			Handles.color = new Color(0, 1.0f, 0, 0.2f);
			Handles.DrawSolidArc(this.Graphic.transform.position, -Vector3.forward, (endpoint - this.Graphic.transform.position).normalized, _viewFov, _viewDistance);
			
		}
#endif
	}
}