using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class UI_FloatingDamage : MonoBehaviour {

		[SerializeField] private GameObject _contentFloatingDamage = default;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent<DamageInfo>(eEventType.ShowFloatingDamage, ShowFloatingDamage);
		}

		public void ShowFloatingDamage(DamageInfo info) {
			if (null == info) {
				return;
			}

			Content_ActorDamage damage = Instantiate(_contentFloatingDamage, this.transform).GetComponent<Content_ActorDamage>();
			damage.transform.position = info.Pos;
			damage.PlayDamage(info.Damage, info.DamagedByCrit);
		}
	}
}