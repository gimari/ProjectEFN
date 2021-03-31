// GENERATED AUTOMATICALLY FROM 'Assets/InputAction/InputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @EFNInputAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @EFNInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputAction"",
    ""maps"": [
        {
            ""name"": ""Actor"",
            ""id"": ""8ce30ecc-a1ac-4186-ac0d-f9bb58784c05"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""b21318f0-f49c-46a7-8e5d-d301a19a459f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""View"",
                    ""type"": ""Value"",
                    ""id"": ""fd086a2d-f65f-4eaf-8b2c-8c0a81a67544"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""d1994b02-2ee4-4f65-97dc-66ff529ad225"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""654df9fe-5ed4-48c6-8ee4-d0f3223cc799"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Button"",
                    ""id"": ""56c85fbf-a6c2-44b9-b1ea-bcce2fd5b9e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""1323942c-3000-4a84-a870-0e1f0858c284"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QuickSlot"",
                    ""type"": ""Button"",
                    ""id"": ""8e300899-3f15-4464-8c89-3cd0c590b5ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Exit"",
                    ""type"": ""Button"",
                    ""id"": ""e375d42a-f210-40d8-927f-c91dde52c27b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""7e8444ba-29b5-4481-9fbd-1db2f22a81e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""VectorMove"",
                    ""id"": ""e84b09cd-415c-48e2-a661-11302eba31ea"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9afa69d5-8dea-418a-aa6d-80b2a0c8310b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""582bb117-fb10-40e6-8760-0944abb5d160"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""78234227-42c5-43ed-b3b7-f6c8700049af"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8d8a150c-e39a-43e0-b3bf-8cf42e5a507a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""16102444-2145-4ddf-9faa-c4540d5adfeb"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""View"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f0a3960-a882-4d11-92d5-17a52fd90874"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f26859b5-d89a-4703-8128-80214de92251"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad33312e-0d20-4a85-8161-4a9c8c46b3de"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5ac0067-248e-4de7-a953-49323089b06c"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7cee36e0-f070-46dd-bbca-8fde8a5b3eac"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""39ca00e9-5457-41ba-9bd1-4cf6e7a86437"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db4d3759-a932-4790-ba85-ca62d0b600fa"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""967e3f00-8384-46f8-9392-726949685b29"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4aa89732-0c91-47b7-9230-d7e9a00454c9"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a9e4c4a7-2c21-4d2f-bb40-c102f4de9934"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""67865d1e-6693-47d4-b118-eb211102885e"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4971de59-e8fb-4660-b7bd-9266453bbfbc"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""241587ec-2ae0-44f8-aedb-4027c59644c3"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f366c2bd-ec50-4855-9395-d8518f53402e"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7de87e4-da9e-4872-aa65-d00e9d8ce437"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""859048d1-5c5a-4c48-9734-b32f6582fe95"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Actor
        m_Actor = asset.FindActionMap("Actor", throwIfNotFound: true);
        m_Actor_Move = m_Actor.FindAction("Move", throwIfNotFound: true);
        m_Actor_View = m_Actor.FindAction("View", throwIfNotFound: true);
        m_Actor_Interact = m_Actor.FindAction("Interact", throwIfNotFound: true);
        m_Actor_Fire = m_Actor.FindAction("Fire", throwIfNotFound: true);
        m_Actor_Zoom = m_Actor.FindAction("Zoom", throwIfNotFound: true);
        m_Actor_Inventory = m_Actor.FindAction("Inventory", throwIfNotFound: true);
        m_Actor_QuickSlot = m_Actor.FindAction("QuickSlot", throwIfNotFound: true);
        m_Actor_Exit = m_Actor.FindAction("Exit", throwIfNotFound: true);
        m_Actor_Run = m_Actor.FindAction("Run", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Actor
    private readonly InputActionMap m_Actor;
    private IActorActions m_ActorActionsCallbackInterface;
    private readonly InputAction m_Actor_Move;
    private readonly InputAction m_Actor_View;
    private readonly InputAction m_Actor_Interact;
    private readonly InputAction m_Actor_Fire;
    private readonly InputAction m_Actor_Zoom;
    private readonly InputAction m_Actor_Inventory;
    private readonly InputAction m_Actor_QuickSlot;
    private readonly InputAction m_Actor_Exit;
    private readonly InputAction m_Actor_Run;
    public struct ActorActions
    {
        private @EFNInputAction m_Wrapper;
        public ActorActions(@EFNInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Actor_Move;
        public InputAction @View => m_Wrapper.m_Actor_View;
        public InputAction @Interact => m_Wrapper.m_Actor_Interact;
        public InputAction @Fire => m_Wrapper.m_Actor_Fire;
        public InputAction @Zoom => m_Wrapper.m_Actor_Zoom;
        public InputAction @Inventory => m_Wrapper.m_Actor_Inventory;
        public InputAction @QuickSlot => m_Wrapper.m_Actor_QuickSlot;
        public InputAction @Exit => m_Wrapper.m_Actor_Exit;
        public InputAction @Run => m_Wrapper.m_Actor_Run;
        public InputActionMap Get() { return m_Wrapper.m_Actor; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActorActions set) { return set.Get(); }
        public void SetCallbacks(IActorActions instance)
        {
            if (m_Wrapper.m_ActorActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_ActorActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_ActorActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_ActorActionsCallbackInterface.OnMove;
                @View.started -= m_Wrapper.m_ActorActionsCallbackInterface.OnView;
                @View.performed -= m_Wrapper.m_ActorActionsCallbackInterface.OnView;
                @View.canceled -= m_Wrapper.m_ActorActionsCallbackInterface.OnView;
                @Interact.started -= m_Wrapper.m_ActorActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_ActorActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_ActorActionsCallbackInterface.OnInteract;
                @Fire.started -= m_Wrapper.m_ActorActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_ActorActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_ActorActionsCallbackInterface.OnFire;
                @Zoom.started -= m_Wrapper.m_ActorActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_ActorActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_ActorActionsCallbackInterface.OnZoom;
                @Inventory.started -= m_Wrapper.m_ActorActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_ActorActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_ActorActionsCallbackInterface.OnInventory;
                @QuickSlot.started -= m_Wrapper.m_ActorActionsCallbackInterface.OnQuickSlot;
                @QuickSlot.performed -= m_Wrapper.m_ActorActionsCallbackInterface.OnQuickSlot;
                @QuickSlot.canceled -= m_Wrapper.m_ActorActionsCallbackInterface.OnQuickSlot;
                @Exit.started -= m_Wrapper.m_ActorActionsCallbackInterface.OnExit;
                @Exit.performed -= m_Wrapper.m_ActorActionsCallbackInterface.OnExit;
                @Exit.canceled -= m_Wrapper.m_ActorActionsCallbackInterface.OnExit;
                @Run.started -= m_Wrapper.m_ActorActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_ActorActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_ActorActionsCallbackInterface.OnRun;
            }
            m_Wrapper.m_ActorActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @View.started += instance.OnView;
                @View.performed += instance.OnView;
                @View.canceled += instance.OnView;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @QuickSlot.started += instance.OnQuickSlot;
                @QuickSlot.performed += instance.OnQuickSlot;
                @QuickSlot.canceled += instance.OnQuickSlot;
                @Exit.started += instance.OnExit;
                @Exit.performed += instance.OnExit;
                @Exit.canceled += instance.OnExit;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
            }
        }
    }
    public ActorActions @Actor => new ActorActions(this);
    public interface IActorActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnView(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnQuickSlot(InputAction.CallbackContext context);
        void OnExit(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
    }
}
