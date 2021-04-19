using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN {
	public class Content_KeySetting : MonoBehaviour {

		[SerializeField] private Text _txtContext = default;
		[SerializeField] private Text _txtKey = default;

		public void OnUpdate(string context, string key) {
			this._txtContext.text = context;
			this._txtKey.text = key;
		}

	}
}