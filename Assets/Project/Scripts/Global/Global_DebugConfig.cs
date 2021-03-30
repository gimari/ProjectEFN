using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
	public class Global_DebugConfig : MonoBehaviour {

		[SerializeField] private bool _infiniteBullet = false;
		public static bool InfiniteBullet = false;

		private void Awake() {
			InfiniteBullet = _infiniteBullet;
		}

	}
}