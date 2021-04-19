using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN {
	public class UI_Setting : MonoBehaviour {

		[SerializeField] private Graphic_FadePop _popup = default;
		[SerializeField] private Slider _volumeSlider = default;
		[SerializeField] private Graphic_List _list = default;

		public void Awake() {
			_list.Init();
			Global_UIEvent.RegisterUIEvent(eEventType.OpenSetting, Open);
		}

		public void Open() {
			this._volumeSlider.SetValueWithoutNotify(AudioListener.volume);

			_list.Clear();

			AddKey("Move forward", "W");
			AddKey("Move left", "A");
			AddKey("Move backward", "S");
			AddKey("Move right", "D");
			AddKey("Fire", "Mouse 1");
			AddKey("Zoom", "Mouse 2");
			AddKey("Interact", "E");
			AddKey("Melee weapon", "V");
			AddKey("Reload", "R");
			AddKey("Open Bag", "Tab");
			AddKey("Primary weapon", "1");
			AddKey("Secondary weapon", "2");
			AddKey("Holster weapon", "3");
			AddKey("Quick 1", "4");
			AddKey("Quick 2", "5");
			AddKey("Quick 3", "6");
			AddKey("Quick 4", "7");
			AddKey("Quick 5", "8");
			AddKey("Quick 6", "9");
			AddKey("Quick 7", "0");

			_popup.Show();
		}

		private void AddKey(string context, string key) {
			Content_KeySetting content = _list.AddWith<Content_KeySetting>();
			content.OnUpdate(context, key);
		}

		public void Close() {
			Global_UIEvent.CallUIEvent(eEventType.OpenStartMenu);
			_popup.Hide();
		}

		public void OnSliderModify() {
			AudioListener.volume = _volumeSlider.value;
			PlayerPrefs.SetFloat("_volume", _volumeSlider.value);
		}
	}
}