using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {

    /// <summary>
    /// enemy spawnner 에서는 랜덤한 주기마다 전체 enemy 개수를 체크하여 최대 enemy 를 유지하게 만들어준다.
    /// </summary>
    public class EnemySpawnner : MonoBehaviour {

        [SerializeField] private int _maxEnemySize = 10;
        [SerializeField] private Transform[] _spawnPoint = default;

        private void Start() {
            StartCoroutine(EnemySpawnRoutine());
        }

        private IEnumerator EnemySpawnRoutine() {
            while (this.gameObject != null) {
                yield return EnemySpawnProcess();
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }

        private IEnumerator EnemySpawnProcess() {
            int targetCount = _maxEnemySize - this.transform.childCount;

            for (; 0 < targetCount; targetCount--) {
                Vector3 target = _spawnPoint[Random.Range(0, _spawnPoint.Length)].position;
                GameObject go = Global_Actor.InitActor(eActorName.Actor_EnemyPurple_0.ToString(), this.transform, target);

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}