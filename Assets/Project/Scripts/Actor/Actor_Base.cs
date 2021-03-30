using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFN.Game {
	public class Actor_Base : MonoBehaviour {

		[SerializeField] private Graphic_Actor _graphic = default;
		public Graphic_Actor Graphic {
			get { return this._graphic; }
		}

		protected Vector2 _movDirection = default;
		public Vector2 MoveDirection {
			get { return this._movDirection; }
		}

		protected Inventory_Item _actorInventory = null;
		public virtual Inventory_Item ActorInventory {
			get { return this._actorInventory; }
		}

		protected Vector2 _sightDirection = Vector2.right;

		private void Awake() {
			this.OnAwake();
		}

		private void Start() {
			this.OnStart();
		}

		protected virtual void OnStart() { }
		protected virtual void OnAwake() { }

		protected virtual void OnTriggerEnter2D(Collider2D other) { }
		protected virtual void OnTriggerExit2D(Collider2D other) { }

		public virtual void SetMoveDirection(Vector2 vec) {
			this._movDirection = vec;
		}

		public virtual void SetSightDirection(Vector2 vec) {
			this._sightDirection = vec;
		}

		public virtual void Stop() {
			this._movDirection = Vector2.zero;
		}
	}
}