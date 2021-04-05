using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace EFN.Game {
	public class UI_IngameOverlay : MonoBehaviour {

		[SerializeField] private Text _txtRemainAmmo = default;
		[SerializeField] private Image _playerHealthImage = default;
		[SerializeField] private Image _heartbeatImage = default;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent(eEventType.UpdateUserInventory, UpdateAmmo);
			Global_UIEvent.RegisterUIEvent(eEventType.OnPlayerShoot, UpdateAmmo);
			Global_UIEvent.RegisterUIEvent(eEventType.OnPlayerSwapWeapon, UpdateAmmo);
			Global_UIEvent.RegisterUIEvent(eEventType.OnPlayerDamageTaken, OnPlayerDamageTaken);

			StartCoroutine(HeartBeatRoutine());
		}

		public void UpdateAmmo() {
			if (null == Global_Actor.SelfPlayer) {
				return;
			}

			ePlayerEquipSlot equipslot = Global_Actor.SelfPlayer.CurrentEquipSlot;

			Data_Item item = Global_Actor.SelfPlayer.ActorInventory.Get((int)equipslot);
			if (null == item || null == item.FireModule || true == item.StatusData.IsKnifeWeapon) {
				this._txtRemainAmmo.gameObject.SetActive(false);
				return;
			}

			this._txtRemainAmmo.gameObject.SetActive(true);
			_txtRemainAmmo.text = item.FireModule.AmmoCount.ToString() + " / " + item.StatusData.MaxRoundAmount;

			_txtRemainAmmo.DOKill();
			_txtRemainAmmo.DOFade(1f, 0.1f).OnComplete(() => {
				_txtRemainAmmo.DOFade(0.5f, 0.5f).SetEase(Ease.OutCirc);
			});
		}

		public void OnPlayerDamageTaken() {
			if (null == Global_Actor.SelfPlayer) {
				return;
			}

			float hitpoint = Global_Actor.SelfPlayer.Dmgable.CurrentHitPoint;

			if (50 <= hitpoint) {
				_playerHealthImage.color = new Color(1, (hitpoint - 50) / 50f, (hitpoint - 50) / 50f);
			} else {
				_playerHealthImage.color = new Color((hitpoint) / 50f, 0, 0);
			}

			_heartbeatImage.DOKill();
			_heartbeatImage.DOFade(0.6f, 0.1f).OnComplete(() => {
				_heartbeatImage.DOFade(0f, 0.5f).SetEase(Ease.OutCirc);
			});
		}

		private IEnumerator HeartBeatRoutine() {

			WaitForFixedUpdate wff = new WaitForFixedUpdate();

			while (true) {
				if (null != Global_Actor.SelfPlayer && null != Global_Actor.SelfPlayer.Dmgable) {

					float hitpoint = Global_Actor.SelfPlayer.Dmgable.CurrentHitPoint;

					if (hitpoint < 50) {
						_heartbeatImage.DOKill();
						_heartbeatImage.DOFade(0.6f * ( -hitpoint / 50 + 1 ), 0.1f).OnComplete(() => {
							_heartbeatImage.DOFade(0f, 0.5f).SetEase(Ease.OutCirc);
						});

						yield return new WaitForSeconds((hitpoint / 50) + 0.2f);
					}
				}

				yield return wff;
			}
		}
	}
}