using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class SoundReceiver : MonoBehaviour {

		private Action _onHearSound = null;
		public Action OnHearSound {
			get { return this._onHearSound; }
			set { this._onHearSound = value; }
		}

		private void OnTriggerEnter2D(Collider2D collision) {
			if (collision.tag != Global_Constant.TAG_SOUNDCOLLIDER) {
				return;
			}

			_onHearSound?.Invoke();
		}
	}
}