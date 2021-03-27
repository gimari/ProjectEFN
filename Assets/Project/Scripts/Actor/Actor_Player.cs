using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

namespace EFN.Game {
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

		private void Update() {
			PlayerMovementProcess();
			PlayerLookingProcess();
		}

		protected virtual void PlayerMovementProcess() {
			this.transform.position = (Vector2)this.transform.position + (_movDirection.normalized * Time.deltaTime * 2f);
		}

		protected virtual void PlayerLookingProcess() {
			Graphic.Flip(0 < this._sightDirection.x);

			_playerArmObject.transform.rotation = Quaternion.FromToRotation(Vector2.up, this._sightDirection);
		}

		public virtual void Fire() { }
	}
}