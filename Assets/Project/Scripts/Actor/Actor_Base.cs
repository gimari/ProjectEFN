using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor_Base : MonoBehaviour {

	private void Awake() {
		this.OnAwake();
	}

	private void Start() {
		this.OnStart();
	}

	protected virtual void OnStart() { }
	protected virtual void OnAwake() { }
}