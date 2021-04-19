using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EFN.Game {
	public enum eMouseCursorType {
		None = 0,
		Interact = 1,
		Interact_Disable,
	}

	public class Global_UserInput : MonoBehaviour {

		[SerializeField] private SpriteRenderer _cursor = default;

		[EnumNamedArray(typeof(eMouseCursorType))]
		[SerializeField] private Sprite[] _cursorSprite = default;

		private void Update() {
			SelfActorViewProcess();
			MousePointerDrawProcess();
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

		/// <summary>
		/// 현재 마우스 위치에 따라 상호작용 가능한지 커서를 표시해주는 로직
		/// </summary>
		private void MousePointerDrawProcess() {

			eMouseCursorType cursor = eMouseCursorType.None;
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			RaycastHit2D rays = Physics2D.Raycast(worldPoint, transform.forward, 100, 1 << (int)eLayerMask.Interactable);

			if (rays) {
				Actor_Base actor = rays.collider.gameObject.GetComponent<Actor_Base>();
				if (null != actor) {
					cursor = eMouseCursorType.Interact_Disable;

					if (true == Global_Actor.Interactable.IsExist(actor.gameObject)) {
						cursor = eMouseCursorType.Interact;
					}
				}
			}

			_cursor.transform.position = worldPoint;
			_cursor.sprite = _cursorSprite[(int)cursor];
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

				if (false == Global_Actor.Interactable.IsExist(actor.gameObject)) {
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

			switch (int.Parse(context.control.name)) {
				case 1:
				case 2:
				case 3:
					Global_Actor.SelfPlayer.SetCurrentEquipSlot(int.Parse(context.control.name));
					break;

				default:
					Global_Actor.SelfPlayer.UseQuickSlot(int.Parse(context.control.name));
					break;
			}
		}

		public void PressExit(InputAction.CallbackContext context) {
			if (context.phase != InputActionPhase.Started) { return; }

			MessageData data = new MessageData();
			data.Context = "임의로 전투 이탈시에는 죽는 것과 같은 패널티가 부과됩니다. 정말 이탈하시겠습니까?";
			data.OnClickOkFunc = OnExitGame;

			Global_UIEvent.CallUIEvent(ePermanetEventType.ShowMessage, data);
		}

		public void OnExitGame() {
			Global_SelfPlayerData.SetKilledInAction();
		}

		public void Run(InputAction.CallbackContext context) {
			if (null == Global_Actor.SelfPlayer) { return; }

			if (context.phase == InputActionPhase.Started) {
				Global_Actor.SelfPlayer.RunStart();

			} else if (context.phase == InputActionPhase.Disabled || context.phase == InputActionPhase.Canceled) {
				Global_Actor.SelfPlayer.RunEnd();
			}
		}

		public void EquipKnife(InputAction.CallbackContext context) {
			if (context.phase != InputActionPhase.Started) { return; }

			if (null == Global_Actor.SelfPlayer) { return; }

			Global_Actor.SelfPlayer.SetCurrentEquipSlot((int)ePlayerSlotType.Knife);
		}

		public void Reload(InputAction.CallbackContext context) {
			if (context.phase != InputActionPhase.Started) { return; }

			if (null == Global_Actor.SelfPlayer) { return; }

			Global_Actor.SelfPlayer.ReloadEquipWeapon();
		}
	}
}