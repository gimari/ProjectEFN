using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
	public class UI_Deploy : MonoBehaviour {

		[SerializeField] private Graphic_FadePop _popup = default;
		[SerializeField] private Graphic_TimeText _timer = default;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent(eEventType.OnShowDeploy, Open);
		}

		public void Open() {
			_popup.Show();

			DateTime dt = Global_Time.CurrentTime.AddSeconds(5);
			_timer.Init(dt, OnEndTimer);
		}

		public void Close() {
			_timer.Clear();
			_popup.Hide();
		}

		public void OnEndTimer() {
			Global_SelfPlayerData.SaveAtGameIn();
			Global_UIEvent.CallUIEvent<string>(ePermanetEventType.TryChangeScene, eSceneName.SceneGame.ToString());
		}
	}
}