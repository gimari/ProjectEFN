using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class BossSpawnner : MonoBehaviour {

		[SerializeField] private GameObject[] _actorObjectList = default;
		[SerializeField] private Transform[] _spawnPoint = default;

		private void Start() {
			float random = Random.Range(0f, 100f);
			if (random < 50) {
				return;
			}

			Vector3 target = _spawnPoint[Random.Range(0, _spawnPoint.Length)].position;
			GameObject go = _actorObjectList[Random.Range(0, _actorObjectList.Length)];
			Instantiate(go, target, Quaternion.identity, this.transform);
		}
	}
}