using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class UI_Ingame : MonoBehaviour, IFocusable {

		public virtual void EndFocus() {
			Global_UIEvent.Focus.EndFocus();
			Global_UIEvent.CallUIEvent(eEventType.OnEndFocus);
		}

		public virtual void OnFocus() {
			if (null != Global_Actor.SelfPlayer) {
				Global_Actor.SelfPlayer.Stop();
			}

			Global_UIEvent.Focus.SetFocus(this.gameObject);
		}
	}
}