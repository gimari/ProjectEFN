using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EFN.Game {
    public class Damageable : MonoBehaviour {

        [SerializeField] private float _maxHitPoint = 0;

        public Action OnDieInAction = null;
        public Action OnReceiveDamage = null;
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

        public void Hit(eItemType firedItem) {

            Status_Base firedStatus = Status_Base.GetStatus(firedItem);
            if (null == firedStatus) {
                Global_Common.LogError("I DONT KNOW WHAT ITEM IS : " + firedItem);
                return;
            }

            _currentHitPoint = _currentHitPoint - firedStatus.DmgAmount;
            if (_currentHitPoint <= 0) {
                DieInAction();
            }
        }

        protected void DieInAction() {
            this.OnDieInAction?.Invoke();
        }
    }
}