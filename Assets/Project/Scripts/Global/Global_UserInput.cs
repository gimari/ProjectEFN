using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EFN.Game {
	public class Global_UserInput : MonoBehaviour {

		public void Move(InputAction.CallbackContext context) {
			if (null == Global_Actor.SelfPlayer) { return; }

			Global_Actor.SelfPlayer.SetMoveDirection(context.ReadValue<Vector2>());
		}

		public void View(InputAction.CallbackContext context) {
			if (null == Global_Actor.SelfPlayer) { return; }

			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());

			Global_Actor.SelfPlayer.SetSightDirection(worldPoint);
		}

		public void Fire(InputAction.CallbackContext context) {
			if (context.phase != InputActionPhase.Started) {
				return;
			}

			if (null == Global_Actor.SelfPlayer) {
				return;
			}

			Global_Actor.SelfPlayer.Fire();
			Graphic_GameCamera.Shake(5);
		}

		public void Zoom(InputAction.CallbackContext context) {
			if (context.phase != InputActionPhase.Started) {
				return;
			}

		}

		public void TryInteract(InputAction.CallbackContext context) {
			if (false == Global_Actor.Interactable.IsExist()) {
				return;
			}

			if (context.phase != InputActionPhase.Started) {
				return;
			}

			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			RaycastHit2D rays = Physics2D.Raycast(worldPoint, transform.forward, 100, 1 << (int)eLayerMask.Interactable);

			if (rays) {
				Debug.LogFormat("You hit [{0}]", rays.collider.gameObject.name);
			}
		}
	}
}