using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
    public class Global_Environment : MonoBehaviour {

		[SerializeField] private Transform[] _patrolPosition = default;

		private static Global_Environment _instance = null;

		private void Awake() {
			_instance = this;
		}

		public static Transform GetRandomPatrolPosition() {
			if (null == _instance) {
				return null;
			}

			return _instance._patrolPosition[Random.Range(0, _instance._patrolPosition.Length)];
		}
	}
}