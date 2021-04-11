using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
    public class Global_Environment : MonoBehaviour {

		[SerializeField] private Transform[] _patrolPosition = default;

		[Tooltip("진입 포지션은 0번 인덱스부터 무조건 시계방향으로 들어가 있어야 한다.. 그래야 입-출구 계산이 편하다.")]
		[SerializeField] private EntranceReceiver[] _entrancePosition = default;

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

		public static Vector3 GetRandomEntranceIndex() {

			int targetIdx = Random.Range(0, _instance._entrancePosition.Length);

			// 생성으로 당첨된애 말고는 탈출 불가로 찍어놓는다.
			for (int idx = 0; idx < _instance._entrancePosition.Length; idx++) {
				_instance._entrancePosition[idx].IsCanExit = idx != targetIdx;
			}

			return _instance._entrancePosition[targetIdx].transform.position;
		}
	}
}