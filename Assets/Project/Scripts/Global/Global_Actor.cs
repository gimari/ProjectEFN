using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class Global_Actor : MonoBehaviour {

		private static Actor_SelfPlayer _selfPlayer = null;
		public static Actor_SelfPlayer SelfPlayer {
			get { return Global_Actor._selfPlayer; }
			set { _selfPlayer = value; }
		}

		public static class Interactable {

			private static List<GameObject> _interactableActors = new List<GameObject>();

			public static bool IsExist() {
				return 0 < _interactableActors.Count;
			}

			public static void Add(GameObject Actor) {
				_interactableActors.Add(Actor);
			}

			public static void Remove(GameObject Actor) {
				_interactableActors.Remove(Actor);
			}
		}
	}
}