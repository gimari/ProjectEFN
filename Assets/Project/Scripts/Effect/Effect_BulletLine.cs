using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
    public class Effect_BulletLine : Effect_Base {

		[SerializeField] private LineRenderer _line = default;

        public override void SetInfo(EffectInstanceInfo info) {

			_line.SetPosition(0, info.Pos);
			_line.SetPosition(1, info.EndPos);

			StartCoroutine(this.AutoDestroyRoutine(info.Duration));
		}
    }
}