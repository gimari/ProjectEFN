using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DEBUG : MonoBehaviour {

	[SerializeField] private Text _text = default;

	private void Awake() {
		Application.logMessageReceived += Application_logMessageReceived;
	}


	private void Application_logMessageReceived(string condition, string stackTrace, LogType type) {
		_text.text = stackTrace;
	}
}