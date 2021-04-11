using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace EFN.Game {

	public enum eActorName {
		None = 0,
		Actor_Player,
		
	}

	public class Global_Actor : MonoBehaviour {

		[SerializeField] private GameObject[] _actorObjectList = default;

		private static Actor_SelfPlayer _selfPlayer = null;
		public static Actor_SelfPlayer SelfPlayer {
			get { return Global_Actor._selfPlayer; }
			set { _selfPlayer = value; }
		}

		private void Start() {
			this.InitActor(eActorName.Actor_Player.ToString());
		}

		public void InitActor(string actorName) {
			GameObject go = Array.Find(_actorObjectList, x => x.name == actorName);

			Vector3 target = Global_Environment.GetRandomEntranceIndex();

			Instantiate(go, Global_Environment.GetRandomEntranceIndex(), Quaternion.identity, this.transform);
		}

		public static class Interactable {

			private static List<GameObject> _interactableActors = new List<GameObject>();

			public static bool IsExist() {
				return 0 < _interactableActors.Count;
			}

			public static bool IsExist(GameObject Actor) {
				return _interactableActors.Contains(Actor);
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