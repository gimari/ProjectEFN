using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class Graphic_Gun : MonoBehaviour {

		[SerializeField] private Animator _gunAnimator = default;
		public Animator GunAnimator { get { return this._gunAnimator; } }

		[SerializeField] private Transform _gunMuzzle = default;
		public Transform GunMuzzle { get { return this._gunMuzzle; } }

	}
}