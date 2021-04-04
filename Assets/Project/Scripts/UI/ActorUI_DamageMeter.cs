using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN.Game {
	public class ActorUI_DamageMeter : MonoBehaviour {
		[SerializeField] private GameObject _dmgMeterContent = default;

		private ActorUI_Base _uibase = null;

		private void Awake() {
			_uibase = transform.parent.GetComponentInParent<ActorUI_Base>();
			if (null == _uibase) {
				Global_Common.LogError("CANNOT FIND UI BASE");
				return;
			}

			_uibase.RegisterUIEvent<float>(eActorUIType.DisplayDamage, DisplayDamage);
		}

		public void DisplayDamage(float damage) {
			Instantiate(_dmgMeterContent, this.transform);
		}
	}
}