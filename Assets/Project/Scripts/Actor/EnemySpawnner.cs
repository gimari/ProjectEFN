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
        [SerializeField] private GameObject[] _actorObjectList = default;

        private void Start() {
            StartCoroutine(EnemySpawnRoutine());
        }

        private IEnumerator EnemySpawnRoutine() {

            // 일단 처음에는 전부다 생성해놓고 시작
            yield return EnemySpawnProcess();

            // 그 뒤에는 5~10초마다 자리있으면 한개씩 충전해주자.
            while (this.gameObject != null) {
                EnemySpawnSingle();
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }

        private IEnumerator EnemySpawnProcess() {
            int targetCount = _maxEnemySize - this.transform.childCount;

            for (; 0 < targetCount; targetCount--) {
                Vector3 target = _spawnPoint[Random.Range(0, _spawnPoint.Length)].position;
                GameObject go = _actorObjectList[Random.Range(0, _actorObjectList.Length)];
                Instantiate(go, target, Quaternion.identity, this.transform);

                yield return new WaitForSeconds(0.2f);
            }
        }

        private void EnemySpawnSingle() {
            int targetCount = _maxEnemySize - this.transform.childCount;

            if (0 < targetCount) {
                Vector3 target = _spawnPoint[Random.Range(0, _spawnPoint.Length)].position;
                GameObject go = _actorObjectList[Random.Range(0, _actorObjectList.Length)];
                Instantiate(go, target, Quaternion.identity, this.transform);
            }
        }
    }
}