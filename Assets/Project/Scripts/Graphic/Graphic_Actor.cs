using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
    public class Graphic_Actor : MonoBehaviour {
        public Vector3 Pos { get { return this.transform.position; } }

		/// <summary>
		/// 모든 액터의 기본 상태는 왼쪽을 보고 있음에 주의!!
		/// </summary>
		public void Flip(bool isRight) {
			this.transform.localScale = new Vector2(isRight ? -1 : 1, 1);
		}
    }
}