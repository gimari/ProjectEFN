using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
	public class Graphic_DepthFixer : MonoBehaviour {

		[SerializeField] private bool _usingStartPos = false;
		[SerializeField] private float _startPos = 0;

		[SerializeField] private Transform _yTarget = default;
		[SerializeField] private bool _setDynamic = false;

		private WaitForFixedUpdate _wff = new WaitForFixedUpdate();

		private void Awake() {
			DepthFix();

			if (true == this._setDynamic) {
				StartCoroutine(DynamicFix());
			}
		}

		private IEnumerator DynamicFix() {
			while (this.gameObject != null) {
				DepthFix();
				yield return _wff;
			}
		}

		protected virtual void DepthFix() {
			if (null == _yTarget) { return; }

			this.transform.localPosition = new Vector3(transform.localPosition.x, _usingStartPos ? _startPos : transform.localPosition.y, _yTarget.position.y * 0.1f);
		}
	}
}