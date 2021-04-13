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
		public bool DamagedByCrit;
	}

    public class Damageable : MonoBehaviour {

		[SerializeField] private bool _showFloatingDamage = true;

		public Action<DamageInfo> OnDieInAction = null;
        public Action<DamageInfo> OnReceiveDamage = null;
        public Action OnOnResetDamageDieInAction = null;
        public Action OnResetDamage = null;
		public Action OnReceiveHeal = null;

		private float _maxHitPoint = 0;
		public float MaxHitPoint {
			get { return _maxHitPoint; }
		}

		private float _currentHitPoint = 100;
        public float CurrentHitPoint {
            get { return this._currentHitPoint; }
        }

		private int _armor = 0;
		public int Armor {
			get { return _armor; }
		}

        public void Init(Actor_Player basePlayer) {

			this._maxHitPoint = basePlayer.MaxHealthPoint();
			this._armor = basePlayer.ArmorAmount();

			ResetDamage();
        }

        public void ResetDamage() {
            _currentHitPoint = _maxHitPoint;
            OnResetDamage?.Invoke();
        }

		public void Heal(float amount) {
			_currentHitPoint = Math.Min(_maxHitPoint, _currentHitPoint + amount);
			this.OnReceiveHeal?.Invoke();
		}

        public void Hit(DamageInfo info) {

			if (null == info) {
				Global_Common.LogError("I CANT HIT BY NULL");
				return;
			}

			if (true == _showFloatingDamage) {
				Global_UIEvent.CallUIEvent<DamageInfo>(eEventType.ShowFloatingDamage, info);
			}

			// 데미지 계산 전에 이미 죽어있다면 이후 처리를 안한다.
			if (_currentHitPoint <= 0) {
				return;
			}

			_currentHitPoint = _currentHitPoint - info.Damage;

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