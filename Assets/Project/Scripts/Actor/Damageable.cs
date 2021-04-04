using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EFN.Game {

	public class DamageInfo {
		public float Damage;
		public Actor_Base HittedActor;
		public Vector2 Pos;
	}

    public class Damageable : MonoBehaviour {

        [SerializeField] private float _maxHitPoint = 0;
		[SerializeField] private bool _showFloatingDamage = true;

        public Action<DamageInfo> OnDieInAction = null;
        public Action<DamageInfo> OnReceiveDamage = null;
        public Action OnOnResetDamageDieInAction = null;
        public Action OnResetDamage = null;

        private float _currentHitPoint = 100;
        public float CurrentHitPoint {
            get { return this._currentHitPoint; }
        }

        private Actor_Player _basePlayer = null;

        public void Init(Actor_Player basePlayer) {
            this._basePlayer = basePlayer;
            ResetDamage();
        }

        public void ResetDamage() {
            _currentHitPoint = _maxHitPoint;
            OnResetDamage?.Invoke();
        }

        public void Hit(eItemType firedItem, Actor_Base hittedActor) {

            Status_Base firedStatus = Status_Base.GetStatus(firedItem);
            if (null == firedStatus) {
                Global_Common.LogError("I DONT KNOW WHAT ITEM IS : " + firedItem);
                return;
            }

            _currentHitPoint = _currentHitPoint - firedStatus.DmgAmount;

			DamageInfo info = new DamageInfo();
			info.Damage = firedStatus.DmgAmount;
			info.HittedActor = hittedActor;
			info.Pos = this.transform.position;

			if (true == _showFloatingDamage) {
				Global_UIEvent.CallUIEvent<DamageInfo>(eEventType.ShowFloatingDamage, info);
			}

			// 이번 일격으로 죽는다면 dia 만 호출된다.
			if (_currentHitPoint <= 0) {
				this.OnDieInAction?.Invoke(info);
				return;
            }

			// 안죽는다면 아프다고 알려줌.
			this.OnReceiveDamage?.Invoke(info);
        }
    }
}