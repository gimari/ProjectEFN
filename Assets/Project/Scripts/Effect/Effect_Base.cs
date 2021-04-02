using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
	public class Effect_Base : MonoBehaviour {

		[SerializeField] private eEffectType _type = default;

		public virtual void SetInfo(EffectInstanceInfo info) {
			this.transform.position = info.Pos;

			switch (info.RotateType) {
				case eEffectRotateType.Normal:
					this.transform.rotation = Quaternion.FromToRotation(Vector2.up, info.TargetNormal);
					break;
			}

			StartCoroutine(this.AutoDestroyRoutine(info.Duration));
		}

		protected IEnumerator AutoDestroyRoutine(float timer) {
			yield return new WaitForSeconds(timer);

			Destroy(this.gameObject);
		}
	}
}