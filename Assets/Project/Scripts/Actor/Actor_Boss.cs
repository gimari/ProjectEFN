using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class Actor_Boss : Actor_Enemy {
		protected override void OnAwake() {
			base.OnAwake();
			_enemyBehaviour = eEnemyBehaviourStatus.Searching;
		}

		protected override void SearchingProcess() {
			_searchingTimer -= Time.deltaTime;

			if (_searchingRandTarget != Vector2.zero) {
				_sightDirection = _searchingRandTarget.normalized;
			}

			_enemyAgent.velocity = _searchingRandTarget.normalized * _walkingSpeed;
		}
	}
}
