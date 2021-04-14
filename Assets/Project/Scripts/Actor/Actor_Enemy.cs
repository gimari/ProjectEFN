using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

namespace EFN.Game {

	/// <summary>
	/// 적군은 크게 쫒기 / 수색 / 로밍 3개의 행동 방식을 취한다.
	/// </summary>
	public enum eEnemyBehaviourStatus {
		None = 0,
		Chasing = 1,
		Searching,
		Roaming,
	}

    public class Actor_Enemy : Actor_Player {

		[Header("Enemy Components")]
		[SerializeField] protected NavMeshAgent _enemyAgent = default;
		[SerializeField] private SoundReceiver _soundReceiver = default;

		[Header("Scanning settings")]
		[Tooltip("The angle of the forward of the view cone. 0 is forward of the sprite, 90 is up, 180 behind etc.")]
		[Range(0.0f, 360.0f)]
		public float _viewFov;
		public float _viewDistance;

		[Tooltip("Time in seconds without the target in the view cone before the target is considered lost from sight")]
		public float _timeBeforeTargetLost = 3.0f;

		/// <summary>
		/// 추격하다 놓쳤을 때 제자리 서칭시간.
		/// </summary>
		[SerializeField] private float _searchingTime = 10;
		[SerializeField] protected float _walkingSpeed = 2;
		[SerializeField] private float _runningSpeed = 3.5f;

		[Header("Enemy Status")]
		[SerializeField] private eItemType _wearingArmor = default;
		public eItemType WearingArmor { get { return this._wearingArmor; } }

		[SerializeField] private eItemType _usingBullet = default;
		public eItemType UsingBullet { get { return this._usingBullet; } }

		[SerializeField] private GameObject _corpseObject = default;

		protected Actor_Player _targetPlayer = null;
		protected float _lostChaingTargetTimer = 0;

		protected Vector2 _randTarget = default;
		protected eEnemyBehaviourStatus _enemyBehaviour = eEnemyBehaviourStatus.Roaming;

		protected Vector2 _searchingRandTarget = default;
		protected float _attackDuration = 1f;
		protected float _searchingTimer = 0;

		protected override void OnAwake() {
			base.OnAwake();
			StartCoroutine(StopAndFiringRandomTargetRoutine());
			StartCoroutine(RandomSearchingRoutine());

			this._soundReceiver.OnHearSound = this.OnHearSound;
		}

		protected override void OnStart() {
			base.OnStart();

			// 맞은곳에 탄흔 이펙트
			EffectInstanceInfo info = new EffectInstanceInfo(eEffectType.RoundExplosion);
			info.Pos = this.Graphic.transform.position;
			info.Duration = 1f;

			Global_Effect.ShowEffect(info);
		}

		protected virtual void FixedUpdate() {
			ScanForPlayerProcess();

			// 간단하게 애니메이션 함..
			if (null != _enemyAgent && _enemyAgent.velocity.magnitude == 0) {
				Graphic.PlayIdle();
			} else if (_enemyBehaviour == eEnemyBehaviourStatus.Chasing) {
				Graphic.PlayRun();
			} else {
				Graphic.PlayWalk();
			}

			switch (_enemyBehaviour) {
				case eEnemyBehaviourStatus.Chasing:
					ChaseForPlayerProcess();
					break;

				case eEnemyBehaviourStatus.Searching:
					SearchingProcess();
					break;

				case eEnemyBehaviourStatus.Roaming:
					RoamingProcess();
					break;

				default:
					break;
			}
		}

		/// <summary>
		/// 아무것도 없으면 사전에 지정된 로밍 포인트를 랜덤하게 뱅글뱅글 돈다
		/// </summary>
		protected void RoamingProcess() {

			_enemyAgent.speed = _walkingSpeed;
			_sightDirection = _enemyAgent.velocity.normalized;

			if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance) {
				Transform pos = Global_Environment.GetRandomPatrolPosition();

				// 사실 이런 경우는 없어야 하지만..
				if (null == pos) {
					OnEndChasing();
				}

				_enemyAgent.SetDestination(pos.position);
			}
		}

		/// <summary>
		/// 타겟잡은놈이 있다면 그놈을 끝까지 쫒는다.
		/// 무기 사정거리 안에 들어왔다면 주위를 빙빙 돌면서 쏜다
		/// </summary>
		protected void ChaseForPlayerProcess() {

			if (null == _enemyAgent) {
				return;
			}

			_enemyAgent.isStopped = _targetPlayer == null;
			_enemyAgent.speed = _runningSpeed;

			// 여기서 추격 타이머를 갱신해서 끝낼지 말지를 결정한다.
			if (_lostChaingTargetTimer < 0) {
				OnEndChasing();
				return;
			}
			
			_targetPlayer = Global_Actor.SelfPlayer;

			// self 가 없으면 searching.
			if (null == _targetPlayer) {
				OnEndChasing();
				return;
			}

			Vector3 dir = _targetPlayer.Graphic.transform.position - this.Graphic.transform.position;
			
			// 타겟 잡고 움직인다.
			_sightDirection = dir.normalized;
			_enemyAgent.SetDestination(_targetPlayer.transform.position);

			Global_Common.DrawLine(this.Graphic.transform.position, _targetPlayer.Graphic.transform.position);

			// 너무 가까이 붙으면 자리를 살짝씩 옮김
			if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance) {
				_enemyAgent.velocity = _randTarget.normalized * _enemyAgent.speed;
			}

			// 공격 사정거리 안에 들어오면 공격 시도
			if (_enemyAgent.remainingDistance <= _viewDistance) {
				EnemyShootProcess();
			}
		}

		/// <summary>
		/// attack duration 을 체크하여 공격 할 수 있으면 공격해버리는 부분
		/// </summary>
		protected void EnemyShootProcess() {
			_attackDuration -= Time.deltaTime;

			if (0 < _attackDuration) {
				return;
			}

			// duration 이 돌아오면 공격할 수 있다
			_attackDuration = Random.Range(0.2f, 0.5f);

			// 떄려야하는 타겟들
			int targetLayer = (1 << (int)eLayerMask.UserHitbox) + (1 << (int)eLayerMask.OtherHittable);
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

					Status_Base firedStatus = Status_Base.GetStatus(this._usingBullet);
					if (null == firedStatus) {
						Global_Common.LogError("I DONT KNOW WHAT ITEM IS : " + this._usingBullet);
						return;
					}

					Global_SelfPlayerData.GetSkillAmount(eSkillType.Critical);

					DamageInfo damage = new DamageInfo();

					damage.Damage = Mathf.Max(0, firedStatus.DmgAmount - dmgable.Armor);
					damage.HittedActor = this;
					damage.Pos = dmgable.transform.position;

					dmgable.Hit(damage);
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

			// 소리도 지름
			EnemyFocusScream();
		}

		/// <summary>
		/// 일정 시간동안 현재 위치에서 랜덤하게 주변을 왓다갓다 거리는 부분
		/// </summary>
		protected virtual void SearchingProcess() {

			// 서칭 시간이 끝나면 로밍을 진입한다.
			if (_searchingTimer < 0) {
				OnEndSearching();
				return;
			}

			_searchingTimer -= Time.deltaTime;

			if (_searchingRandTarget != Vector2.zero) {
				_sightDirection = _searchingRandTarget.normalized;
			}

			_enemyAgent.velocity = _searchingRandTarget.normalized * _walkingSpeed;
		}

		/// <summary>
		/// 추격이 끝나서 대상이 없어짐
		/// 일단 일정 시간동안 수색모드에 돌입
		/// </summary>
		private void OnEndChasing() {
			_enemyBehaviour = eEnemyBehaviourStatus.Searching;
			_searchingTimer = _searchingTime;
			_targetPlayer = null;
		}

		protected virtual void OnEndSearching() {
			_enemyBehaviour = eEnemyBehaviourStatus.Roaming;
		}

		/// <summary>
		/// 랜덤하게 타겟잡은 놈 주위를 빙빙 돌 수 있게 randtarget 갱신해주는 루틴
		/// </summary>
		private IEnumerator StopAndFiringRandomTargetRoutine() {
			while (this.gameObject != null) {

				if (null == _targetPlayer) {
					yield return null;
					continue;
				}

				// 그냥 돌지 않고 현재 위치에서 타겟잡은 놈 주위를 40도 안쪽으로 돌아준다
				_randTarget = Vector2Extension.Rotate((this.transform.position - _targetPlayer.transform.position), Random.Range(-40, 40));
				_randTarget = ((_randTarget.normalized * (_enemyAgent.stoppingDistance)) + (Vector2)_targetPlayer.transform.position) - (Vector2)this.transform.position;
				yield return new WaitForSeconds(Random.Range(0.8f, 1f));

				_randTarget = Vector2.zero;
				yield return new WaitForSeconds(Random.Range(1f, 2f));
			}
		}

		/// <summary>
		/// 주변 자리를 돌면서 왓다갓다 하는 로직.
		/// _serchingrandtarget 을 계속 갱신해준다.
		/// </summary>
		private IEnumerator RandomSearchingRoutine() {
			while (this.gameObject != null) {
				if (_enemyBehaviour != eEnemyBehaviourStatus.Searching) {
					yield return null;
					continue;
				}

				float currentAngle = 0;
				float movTarget = Random.Range(-180, 180);
				float anglingTimer = Random.Range(0.8f, 1f);
				Vector2 startTarget = _sightDirection;

				while (0 < anglingTimer) {
					anglingTimer -= Time.deltaTime;
					currentAngle = Mathf.Lerp(currentAngle, movTarget, Time.deltaTime * 5);

					_searchingRandTarget = Vector2Extension.Rotate(startTarget, currentAngle);
					yield return null;
				}

				_searchingRandTarget = Vector2.zero;
				yield return new WaitForSeconds(Random.Range(2f, 4f));
			}
		}

		/// <summary>
		/// 목표와 너무 멀리 떨어져 잇을 때.
		/// </summary>
		private void OnTooFarDistance() {
			_lostChaingTargetTimer -= Time.deltaTime;
		}

		/// <summary>
		/// 목표가 시야각에 안 들어와 있을 때.
		/// </summary>
		private void OnNotInViewFov() {
			_lostChaingTargetTimer -= Time.deltaTime;
		}

		public void ScanForPlayerProcess() {
			if (null == Global_Actor.SelfPlayer) {
				_targetPlayer = null;
				return;
			}

			Vector3 dir = Global_Actor.SelfPlayer.Graphic.transform.position - this.Graphic.transform.position;
			
			// 너무 멀리 떨어져 있을 때는 타겟을 잃은 것
			if (dir.sqrMagnitude > _viewDistance * _viewDistance) {
				OnTooFarDistance();
				return;
			}

			float angle = Vector2.Angle(_sightDirection.normalized, dir);

			// 시야각을 벗어나 잇으면 타겟을 잃은 것
			if (angle > _viewFov * 0.5f) {
				OnNotInViewFov();
				return;
			}

			// AI 는 시야를 방해하는 물건 혹은 벽 등을 사이에 끼면 못 봐야 한다
			int targetLayer = (1 << (int)eLayerMask.ShowBlockable) + (1 << (int)eLayerMask.OtherHittable) + (1 << (int)eLayerMask.UserHitbox);
			RaycastHit2D rays = Physics2D.Raycast(this.Graphic.transform.position, dir, dir.magnitude, targetLayer);

			// 레이를 쏴서 시야를 가리는놈이 있는지 체크. 인식하고 있다면 시야 가리는건 의미가 없음.
			if (rays && rays.transform.gameObject.layer != (int)eLayerMask.UserHitbox) {
				return;
			}

			OnStartChasing();
		}

		/// <summary>
		/// 사운드 들렸을때 간단히 플레이어를 타겟으로 잡아주는 부분
		/// </summary>
		private void OnHearSound() {
			OnStartChasing();
		}

		/// <summary>
		/// chasing 상태로 최초 진입한다.
		/// </summary>
		private void OnStartChasing() {

			// 최초 진입할때는 육성도 같이 내야한다.
			EnemyFocusScream(true);

			_enemyBehaviour = eEnemyBehaviourStatus.Chasing;

			// 기존 생각하던건 아래의 변수를 인지 게이지처럼 활용해서 임계점을 넘어가면 완전인식하고 하는걸 구현하려 햇음
			// 일단 나중에 변환할 수 있을 만큼만 해논다.
			_lostChaingTargetTimer = _timeBeforeTargetLost;
		}

		/// <summary>
		/// 뭔가 발견해서 고함쳐가지고 주변사람들 부름
		/// </summary>
		private void EnemyFocusScream(bool withScream = false) {
			SoundGeneratingData sgd = new SoundGeneratingData();
			sgd.Radius = 7f;
			this._soundGenerator.MakeSound(sgd);
		}

		protected override void OnReceiveDamage(DamageInfo hitinfo) {
			base.OnReceiveDamage(hitinfo);

			EnemyFocusScream();
		}

		protected override void OnDieInAction(DamageInfo hitinfo) {

			if (null == hitinfo) {
				return;
			}

			if (null != this._corpseObject) {

				// 맞은곳에 탄흔 이펙트
				EffectInstanceInfo info = new EffectInstanceInfo(eEffectType.ExplosionNova);
				info.Pos = this.Graphic.transform.position;
				info.Duration = 1f;

				Global_Effect.ShowEffect(info);

				Actor_Corpse corpse = Instantiate(this._corpseObject).GetComponent<Actor_Corpse>();
				corpse.transform.position = this.transform.position;
				corpse.RigidBody.AddForce((this.transform.position - hitinfo.HittedActor.transform.position).normalized * Random.Range(80, 200), ForceMode2D.Impulse);
				corpse.Graphic.Flip(this.Graphic.IsFlip());
			}

			base.OnDieInAction(hitinfo);
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