using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class EntranceReceiver : MonoBehaviour {

		private bool _isCanExit = false;

		/// <summary>
		/// 이 탈출구로 탈출할 수 있음 체크
		/// </summary>
		public bool IsCanExit {
			get { return _isCanExit; }
			set { _isCanExit = value; }
		}

		private void OnEnterPlayer() {
			if (false == _isCanExit) {
				return;
			}

			Global_UIEvent.CallUIEvent(eEventType.ShowExtract, 9f);
		}

		private void OnExitPlayer() {
			if (false == _isCanExit) {
				return;
			}

			Global_UIEvent.CallUIEvent(eEventType.HideExtract);
		}

		private void OnTriggerEnter2D(Collider2D collision) {
			if (collision.tag == Global_Constant.TAG_Closechecker) {
				OnEnterPlayer();
			}
		}

		private void OnTriggerExit2D(Collider2D collision) {
			if (collision.tag == Global_Constant.TAG_Closechecker) {
				OnExitPlayer();
			}
		}
	}
}