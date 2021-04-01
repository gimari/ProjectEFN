using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EFN.Game {
    public class Actor_Enemy : Actor_Player {

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

		private void Update() {
			ScanForPlayer();
		}

		public void ScanForPlayer() {
			if (null == Global_Actor.SelfPlayer) {
				return;
			}

			Vector3 dir = Global_Actor.SelfPlayer.transform.position - transform.position;

			if (dir.sqrMagnitude > _viewDistance * _viewDistance) {
				return;
			}

			float angle = Vector2.Angle(_sightDirection.normalized, dir);

			if (angle > _viewFov * 0.5f) {
				return;
			}

			Debug.LogError("I CAN SEE");

			// m_Target = PlayerCharacter.PlayerInstance.transform;
			// m_TimeSinceLastTargetView = _timeBeforeTargetLost;

			// m_Animator.SetTrigger(m_HashSpottedPara);
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
			
			Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, _viewFov * 0.5f) * forward);

			Handles.color = new Color(0, 1.0f, 0, 0.2f);
			Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, _viewFov, _viewDistance);
			
		}
#endif
	}
}