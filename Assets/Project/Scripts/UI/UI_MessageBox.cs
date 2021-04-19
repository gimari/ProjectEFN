using EFN.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN {

	public class MessageData {
		public string Context = "";
		public Action OnClickOkFunc = null;
		public Action OnClickCancelFunc = null;
	}

	public class UI_MessageBox : MonoBehaviour, IFocusable {

		[SerializeField] private Graphic_FadePop _popup = default;
		[SerializeField] private Text _txtContext = default;

		private MessageData _cachedData = null;

		private void Awake() {
			Global_UIEvent.RegisterUIEvent<MessageData>(ePermanetEventType.ShowMessage, OpenMessage);
		}

		public void OpenMessage(MessageData data) {
			this._cachedData = data;
			this._txtContext.text = data.Context;
			OnFocus();
			_popup.Show();
		}

		public void Close() {
			EndFocus();
			_popup.Hide();
		}

		public void OnClickOK() {
			Close();

			if (null != this._cachedData) {
				this._cachedData.OnClickOkFunc?.Invoke();
			}
		}

		public void OnClickCancel () {
			Close();

			if (null != this._cachedData) {
				this._cachedData.OnClickCancelFunc?.Invoke();
			}
		}

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