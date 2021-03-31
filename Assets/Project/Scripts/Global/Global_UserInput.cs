using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EFN.Game {
	public class Global_UserInput : MonoBehaviour {

		private void Update() {
			SelfActorViewProcess();
		}

		/// <summary>
		/// self actor 가 있다면 마우스 포지션 받아와서 sight direction 갱신해주는 로직
		/// </summary>
		private void SelfActorViewProcess() {
			if (null == Global_Actor.SelfPlayer) {
				return;
			}

			if (true == Global_UIEvent.Focus.IsFocusing) { 
				return; 
			}

			Vector3 screenPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - Global_Actor.SelfPlayer.Graphic.Pos;

			// Z포지션을 10 으로 줘가지고 좀더 시점을 자연스럽게 유도했음.
			screenPos.z = 10;

			Global_Actor.SelfPlayer.SetSightDirection(screenPos.normalized);
		}

		public void Move(InputAction.CallbackContext context) {

			if (true == Global_UIEvent.Focus.IsFocusing) { return; }

			if (null == Global_Actor.SelfPlayer) { return; }

			Global_Actor.SelfPlayer.SetMoveDirection(context.ReadValue<Vector2>());
		}

		public void View(InputAction.CallbackContext context) { }

		public void Fire(InputAction.CallbackContext context) {

			if (true == Global_UIEvent.Focus.IsFocusing) { return; }

			if (null == Global_Actor.SelfPlayer) { return; }

			if (context.phase == InputActionPhase.Started) {
				Global_Actor.SelfPlayer.FireStart();

			} else if (context.phase == InputActionPhase.Disabled || context.phase == InputActionPhase.Canceled) {
				Global_Actor.SelfPlayer.FireEnd();
			}
		}

		public void Zoom(InputAction.CallbackContext context) {
			if (true == Global_UIEvent.Focus.IsFocusing) { return; }

			if (context.phase != InputActionPhase.Started) { return; }

			Graphic_GameCamera.Zoom();
		}

		public void TryInteract(InputAction.CallbackContext context) {
			if (true == Global_UIEvent.Focus.IsFocusing) { return; }

			if (false == Global_Actor.Interactable.IsExist()) { return; }

			if (context.phase != InputActionPhase.Started) {
				return;
			}

			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			RaycastHit2D rays = Physics2D.Raycast(worldPoint, transform.forward, 100, 1 << (int)eLayerMask.Interactable);

			if (rays) {
				Actor_Base actor = rays.collider.gameObject.GetComponent<Actor_Base>();
				if (null == actor) {
					return;
				}

				Global_UIEvent.CallUIEvent<Actor_Base>(eEventType.TryInteractWith, actor);
			}
		}

		public void ToggleInventory(InputAction.CallbackContext context) {
			if (context.phase != InputActionPhase.Started) { return; }

			if (null == Global_Actor.SelfPlayer) { return; }

			Global_UIEvent.CallUIEvent(eEventType.ToggleIngameInven);
		}

		public void QuickSlot(InputAction.CallbackContext context) {
			if (context.phase != InputActionPhase.Started) { return; }

			if (null == Global_Actor.SelfPlayer) { return; }

			(Global_Actor.SelfPlayer.ActorInventory as Inventory_SelfPlayer).UseQuickSlot(int.Parse(context.control.name));
		}

		public void PressExit(InputAction.CallbackContext context) {
			if (context.phase != InputActionPhase.Started) { return; }

			Global_Common.LoadScene(eSceneName.SceneMain.ToString());
		}

		public void Run(InputAction.CallbackContext context) {
			if (null == Global_Actor.SelfPlayer) { return; }

			if (context.phase == InputActionPhase.Started) {
				Global_Actor.SelfPlayer.RunStart();

			} else if (context.phase == InputActionPhase.Disabled || context.phase == InputActionPhase.Canceled) {
				Global_Actor.SelfPlayer.RunEnd();
			}
		}
	}
}