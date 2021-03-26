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
    public struct ActorActions
    {
        private @EFNInputAction m_Wrapper;
        public ActorActions(@EFNInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Actor_Move;
        public InputAction @View => m_Wrapper.m_Actor_View;
        public InputAction @Interact => m_Wrapper.m_Actor_Interact;
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
            }
        }
    }
    public ActorActions @Actor => new ActorActions(this);
    public interface IActorActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnView(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}
