using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_DD : MonoBehaviour {

	private void Awake() {
		DontDestroyOnLoad(this.gameObject);

		IDontDestroy[] ddList = GetComponentsInChildren<IDontDestroy>();
		foreach (IDontDestroy dd in ddList) {
			dd.Init();
		}
	}
}

public interface IDontDestroy {
	void Init();
}