using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
	public class Global_DebugConfig : MonoBehaviour {

#if EFN_DEBUG
		[SerializeField] private bool _infiniteBullet = false;
		public static bool InfiniteBullet = false;

		[SerializeField] private bool _invinsible = false;
		public static bool Invinsible = false;

		[SerializeField] private bool _infiniteMoney = false;
		public static bool InfiniteMoney = false;

		private void Awake() {
			InfiniteBullet = _infiniteBullet;
			Invinsible = _invinsible;
			InfiniteMoney = _infiniteMoney;
		}
#endif

	}
}