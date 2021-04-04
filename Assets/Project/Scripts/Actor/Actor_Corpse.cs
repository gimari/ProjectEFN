
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {

	public class Actor_Corpse : Actor_Box {

		[SerializeField] private Rigidbody2D _rigidBody = default;
		public Rigidbody2D RigidBody { get { return this._rigidBody; } }
		
	}
}