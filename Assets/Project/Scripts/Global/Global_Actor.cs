using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace EFN.Game {

	public enum eActorName {
		None = 0,
		Actor_Player,
		Actor_EnemyPurple_0,
	}

	public class Global_Actor : MonoBehaviour {

		[SerializeField] private GameObject[] _actorObjectList = default;

		private static Actor_SelfPlayer _selfPlayer = null;
		public static Actor_SelfPlayer SelfPlayer {
			get { return Global_Actor._selfPlayer; }
			set { _selfPlayer = value; }
		}

		private static Global_Actor _instance;

		private void Awake() {
			_instance = this;
		}

		private void Start() {
			InitActor(eActorName.Actor_Player.ToString(), this.transform, Global_Environment.GetRandomEntranceIndex());
		}

		public static GameObject InitActor(string actorName, Transform parent, Vector2 position) {
			if (null == _instance) {
				return null;
			}

			GameObject go = Array.Find(_instance._actorObjectList, x => x.name == actorName);
			return Instantiate(go, position, Quaternion.identity, parent);
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