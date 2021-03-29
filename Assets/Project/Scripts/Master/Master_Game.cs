using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class Master_Game : MonoBehaviour {

		private void Awake() {
#if !UNITY_EDITOR
			Cursor.lockState = CursorLockMode.Confined;
#endif
		}

	}
}