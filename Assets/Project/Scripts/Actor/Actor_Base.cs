﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFN.Game {
	public class Actor_Base : MonoBehaviour, IPointerEnterHandler {

		[SerializeField] private Graphic_Actor _graphic = default;
		public Graphic_Actor Graphic {
			get { return this._graphic; }
		}

		protected Vector2 _movDirection = default;
		protected Vector2 _sightDirection = default;

		private void Awake() {
			this.OnAwake();
		}

		private void Start() {
			this.OnStart();
		}

		protected virtual void OnStart() { }
		protected virtual void OnAwake() { }

		protected virtual void OnTriggerEnter2D(Collider2D other) { }

		public void OnPointerEnter(PointerEventData eventData) {
			Debug.LogError("FUCK");
		}
	}
}