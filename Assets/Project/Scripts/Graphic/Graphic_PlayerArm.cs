using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public enum eArmGraphicStatus {
		BareHand = 0,
		Gun = 1,
	}

	public class Graphic_PlayerArm : MonoBehaviour {

		[SerializeField] private Graphic_Gun _targetGun = null;
		private eArmGraphicStatus _currentStatus = eArmGraphicStatus.BareHand;

		public Vector2 GetMuzzlePos {
			get {
				if (null == _targetGun) {
					return this.transform.position;
				}

				return _targetGun.GunMuzzle.position;
			}
		}
		
		public void SetItem(Data_Item item) {
			if (null != _targetGun) {
				Destroy(_targetGun.gameObject);
			}

			if (null == item) {
				SetBareHand();
				return;
			}

			GameObject targetPrefab = Global_ResourceContainer.GetGunPrefab(item.ItemType.ToString());
			if (null == targetPrefab) {
				SetBareHand();
				return;
			}

			Graphic_Gun gunGraphic = Instantiate(targetPrefab, this.transform).GetComponent<Graphic_Gun>();
			if (null == gunGraphic) {
				SetBareHand();
				return;
			}

			_currentStatus = eArmGraphicStatus.Gun;
			_targetGun = gunGraphic;

			switch(item.StatusData.ItemCategory) {
				case eItemCategory.Weapon:
					// 총 종류는 use cooltime 이 스왑속도랑 같아서 이렇게 연출해준다.
					GunSwapProcess(1 / (item.StatusData.UseCoolTime));
					break;

				default:
					// 총 종류가 아니면 2배속 재생이 공통이다.
					GunSwapProcess(2);
					break;
			}
		}

		protected void GunSwapProcess(float targetSpeed) {
			_targetGun.GunAnimator.Rebind();

			// 스왑 애니메이션이 1초이므로
			_targetGun.GunAnimator.speed = targetSpeed;
			_targetGun.GunAnimator.Play("Swap");
		}

		public void Fire() {
			if (null == _targetGun) {
				return;
			}

			_targetGun.GunAnimator.speed = 1;
			_targetGun.GunAnimator.Play("Fire");
		}

		public void SetBareHand() {
			if (_currentStatus == eArmGraphicStatus.BareHand) {
				return;
			}

			if (null != _targetGun) {
				Destroy(_targetGun.gameObject);
			}

			_currentStatus = eArmGraphicStatus.BareHand;
		}
	}
}