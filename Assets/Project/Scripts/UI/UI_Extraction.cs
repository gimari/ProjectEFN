using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class UI_Extraction : MonoBehaviour {

		[SerializeField] private Graphic_FadePop _extractPopup = default;
		[SerializeField] private Graphic_TimeText _extractTimer = default;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent<float>(eEventType.ShowExtract, ShowExtract);
			Global_UIEvent.RegisterUIEvent(eEventType.HideExtract, HideExtract);
		}

		public void ShowExtract(float timer) {
			_extractPopup.Show();

			DateTime dt = Global_Time.CurrentTime.AddSeconds(timer);

			_extractTimer.Init(dt, OnEndExtract);
		}

		private void OnEndExtract() {
			// 탈출 성공
			Global_SelfPlayerData.SetExtract();
		}

		public void HideExtract() {
			_extractPopup.Hide();
			_extractTimer.Clear();
		}
	}
}