using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Main {
	public class UI_MainIntro : MonoBehaviour {

		[SerializeField] Graphic_FadePop _introPopup = default;

		private void Awake() {
			_introPopup.Show();
		}

		public void OnClickOK() {
			_introPopup.Hide();
			Global_UIEvent.CallUIEvent(eEventType.OpenStartMenu);
		}
	}
}