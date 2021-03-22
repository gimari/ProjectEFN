using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_DD : MonoBehaviour {
	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
}