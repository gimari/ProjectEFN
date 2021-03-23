using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class Global_Actor : MonoBehaviour {

		private static Actor_Base _selfPlayer = null;
		public static Actor_Base SelfPlayer {
			get { return Global_Actor._selfPlayer; }
		}


	}
}