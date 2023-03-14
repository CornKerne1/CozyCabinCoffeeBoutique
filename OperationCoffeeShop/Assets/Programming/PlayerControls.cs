//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Programming/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""FPPlayer"",
            ""id"": ""0b34927f-cc2b-4de9-8b25-203c84a93f63"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""06ce4dd4-db77-4083-827e-e31c3a15530d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""Value"",
                    ""id"": ""4ae8a51c-bd65-49c6-83a2-25b8f90db72d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""910954c9-7cae-4429-8fa1-4e43b66bced5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""723a6272-67ec-4d51-aa8f-4e36ad45b53a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3ea3144a-5a2a-4a72-a50f-d5969353c8ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim Down Sights"",
                    ""type"": ""Button"",
                    ""id"": ""bde0b333-1f4b-4192-b1f6-d746caad385d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""Button"",
                    ""id"": ""ba688af3-eee2-476a-ae2b-ffcf1fe4864c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InteractWtih "",
                    ""type"": ""Button"",
                    ""id"": ""75575b93-b018-45d2-8613-d6e904b34fdb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""fd9f8615-f07f-42df-8111-1d7adef34605"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Alt_Interact"",
                    ""type"": ""Button"",
                    ""id"": ""7d93fb72-49cb-4230-bedc-0008aacd0cb4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MiniGames"",
                    ""type"": ""Button"",
                    ""id"": ""ead2043c-2da1-4e4b-8a13-bac652936298"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""3e138dd2-bbf4-45d6-a99b-b9b08fa76b2b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveObj"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bf22d0ea-6856-41a1-bf87-fc05afae3585"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""FreeCam"",
                    ""type"": ""Button"",
                    ""id"": ""623f3f07-bf02-44b9-9491-3cd64de8c467"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9f295a2f-2b46-4450-8aa8-091c0d1cc783"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Aim Down Sights"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""edc8045c-d307-43d6-b1ba-53da60a44b8f"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim Down Sights"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5a5a96f-0352-48ed-9759-d697b39d9a18"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e7f79ce-35a7-40cc-bf67-442d435f35da"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6a64f63-39dc-4fe7-a1ae-f8ac93a8ea88"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a89b5d4-edce-438b-8744-43f2499a1092"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f4e9fb4-c3f5-4350-8854-032148332365"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0dfa4dc9-c85a-4138-a545-64564aaf1ee1"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f5b4fc8-2a3b-47cc-a0f5-4ed70f7f8d97"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0c35607e-f3aa-4407-a3b4-7a9aafecacff"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""InteractWtih "",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4e365cf-1a60-4e30-a2ed-a33c1f256ab5"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""InteractWtih "",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc756f09-3acd-4768-a651-8485be74f4e3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7be0fce5-5c10-4b98-978f-cd38411dc4bb"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0449fc5b-eaaa-4874-85c6-715c2fa4d7e5"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MiniGames"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""3940d5f0-289b-4619-8d6f-9b6128bf13e3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c135abaf-f094-4532-a573-672dd7faeeeb"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""666cf2b3-4f6e-4169-a020-05f74bded36a"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cce4f883-6d7a-472f-93db-439b56bedca4"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d28b64f1-8e1a-4f9a-ad33-e69d5767bb5f"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c80920d0-ec70-48a6-b0b9-a719d312e6b4"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveObj"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1db265ba-97ea-4276-85ee-9590ac133653"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MoveObj"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b37bee69-aeae-4da2-a964-1c338601a8ce"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Alt_Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37a455be-7a85-41d2-921e-b9d4c868b517"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Alt_Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""efebcd80-fa03-4f3f-9957-2ed063eac19f"",
                    ""path"": ""<Keyboard>/f1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""FreeCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82a7b3f9-f166-4dfc-b4b7-e7b76fd6c6ca"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""86fd400d-9491-463f-913a-010b50d022e5"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""c1bc7dc1-535d-46dc-84bc-ef289e667b6a"",
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
                    ""id"": ""39b7d0e9-7b88-4518-b457-6a5183d6a443"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""caf312ee-afed-4c89-8c5d-2692f042a24d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""db8b6219-df00-4eca-a733-5c5ea425eaf4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""aa5fe3ba-6286-4868-a130-1cbbbb5c2c6d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a3556010-8fc9-4334-a37d-70fb7df70b19"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c6a94c85-0faa-4b07-bbb1-3af36a095a73"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""137a873b-2f7d-4d32-9f2f-7ff140635740"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // FPPlayer
        m_FPPlayer = asset.FindActionMap("FPPlayer", throwIfNotFound: true);
        m_FPPlayer_Move = m_FPPlayer.FindAction("Move", throwIfNotFound: true);
        m_FPPlayer_Mouse = m_FPPlayer.FindAction("Mouse", throwIfNotFound: true);
        m_FPPlayer_Jump = m_FPPlayer.FindAction("Jump", throwIfNotFound: true);
        m_FPPlayer_Sprint = m_FPPlayer.FindAction("Sprint", throwIfNotFound: true);
        m_FPPlayer_Crouch = m_FPPlayer.FindAction("Crouch", throwIfNotFound: true);
        m_FPPlayer_AimDownSights = m_FPPlayer.FindAction("Aim Down Sights", throwIfNotFound: true);
        m_FPPlayer_PauseGame = m_FPPlayer.FindAction("PauseGame", throwIfNotFound: true);
        m_FPPlayer_InteractWtih = m_FPPlayer.FindAction("InteractWtih ", throwIfNotFound: true);
        m_FPPlayer_Interact = m_FPPlayer.FindAction("Interact", throwIfNotFound: true);
        m_FPPlayer_Alt_Interact = m_FPPlayer.FindAction("Alt_Interact", throwIfNotFound: true);
        m_FPPlayer_MiniGames = m_FPPlayer.FindAction("MiniGames", throwIfNotFound: true);
        m_FPPlayer_Rotate = m_FPPlayer.FindAction("Rotate", throwIfNotFound: true);
        m_FPPlayer_MoveObj = m_FPPlayer.FindAction("MoveObj", throwIfNotFound: true);
        m_FPPlayer_FreeCam = m_FPPlayer.FindAction("FreeCam", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // FPPlayer
    private readonly InputActionMap m_FPPlayer;
    private IFPPlayerActions m_FPPlayerActionsCallbackInterface;
    private readonly InputAction m_FPPlayer_Move;
    private readonly InputAction m_FPPlayer_Mouse;
    private readonly InputAction m_FPPlayer_Jump;
    private readonly InputAction m_FPPlayer_Sprint;
    private readonly InputAction m_FPPlayer_Crouch;
    private readonly InputAction m_FPPlayer_AimDownSights;
    private readonly InputAction m_FPPlayer_PauseGame;
    private readonly InputAction m_FPPlayer_InteractWtih;
    private readonly InputAction m_FPPlayer_Interact;
    private readonly InputAction m_FPPlayer_Alt_Interact;
    private readonly InputAction m_FPPlayer_MiniGames;
    private readonly InputAction m_FPPlayer_Rotate;
    private readonly InputAction m_FPPlayer_MoveObj;
    private readonly InputAction m_FPPlayer_FreeCam;
    public struct FPPlayerActions
    {
        private @PlayerControls m_Wrapper;
        public FPPlayerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_FPPlayer_Move;
        public InputAction @Mouse => m_Wrapper.m_FPPlayer_Mouse;
        public InputAction @Jump => m_Wrapper.m_FPPlayer_Jump;
        public InputAction @Sprint => m_Wrapper.m_FPPlayer_Sprint;
        public InputAction @Crouch => m_Wrapper.m_FPPlayer_Crouch;
        public InputAction @AimDownSights => m_Wrapper.m_FPPlayer_AimDownSights;
        public InputAction @PauseGame => m_Wrapper.m_FPPlayer_PauseGame;
        public InputAction @InteractWtih => m_Wrapper.m_FPPlayer_InteractWtih;
        public InputAction @Interact => m_Wrapper.m_FPPlayer_Interact;
        public InputAction @Alt_Interact => m_Wrapper.m_FPPlayer_Alt_Interact;
        public InputAction @MiniGames => m_Wrapper.m_FPPlayer_MiniGames;
        public InputAction @Rotate => m_Wrapper.m_FPPlayer_Rotate;
        public InputAction @MoveObj => m_Wrapper.m_FPPlayer_MoveObj;
        public InputAction @FreeCam => m_Wrapper.m_FPPlayer_FreeCam;
        public InputActionMap Get() { return m_Wrapper.m_FPPlayer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FPPlayerActions set) { return set.Get(); }
        public void SetCallbacks(IFPPlayerActions instance)
        {
            if (m_Wrapper.m_FPPlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMove;
                @Mouse.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMouse;
                @Mouse.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMouse;
                @Mouse.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMouse;
                @Jump.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnJump;
                @Sprint.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnSprint;
                @Crouch.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnCrouch;
                @AimDownSights.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnAimDownSights;
                @AimDownSights.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnAimDownSights;
                @AimDownSights.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnAimDownSights;
                @PauseGame.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnPauseGame;
                @InteractWtih.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnInteractWtih;
                @InteractWtih.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnInteractWtih;
                @InteractWtih.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnInteractWtih;
                @Interact.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnInteract;
                @Alt_Interact.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnAlt_Interact;
                @Alt_Interact.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnAlt_Interact;
                @Alt_Interact.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnAlt_Interact;
                @MiniGames.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMiniGames;
                @MiniGames.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMiniGames;
                @MiniGames.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMiniGames;
                @Rotate.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnRotate;
                @MoveObj.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMoveObj;
                @MoveObj.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMoveObj;
                @MoveObj.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnMoveObj;
                @FreeCam.started -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnFreeCam;
                @FreeCam.performed -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnFreeCam;
                @FreeCam.canceled -= m_Wrapper.m_FPPlayerActionsCallbackInterface.OnFreeCam;
            }
            m_Wrapper.m_FPPlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Mouse.started += instance.OnMouse;
                @Mouse.performed += instance.OnMouse;
                @Mouse.canceled += instance.OnMouse;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @AimDownSights.started += instance.OnAimDownSights;
                @AimDownSights.performed += instance.OnAimDownSights;
                @AimDownSights.canceled += instance.OnAimDownSights;
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
                @InteractWtih.started += instance.OnInteractWtih;
                @InteractWtih.performed += instance.OnInteractWtih;
                @InteractWtih.canceled += instance.OnInteractWtih;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Alt_Interact.started += instance.OnAlt_Interact;
                @Alt_Interact.performed += instance.OnAlt_Interact;
                @Alt_Interact.canceled += instance.OnAlt_Interact;
                @MiniGames.started += instance.OnMiniGames;
                @MiniGames.performed += instance.OnMiniGames;
                @MiniGames.canceled += instance.OnMiniGames;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @MoveObj.started += instance.OnMoveObj;
                @MoveObj.performed += instance.OnMoveObj;
                @MoveObj.canceled += instance.OnMoveObj;
                @FreeCam.started += instance.OnFreeCam;
                @FreeCam.performed += instance.OnFreeCam;
                @FreeCam.canceled += instance.OnFreeCam;
            }
        }
    }
    public FPPlayerActions @FPPlayer => new FPPlayerActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IFPPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMouse(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnAimDownSights(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
        void OnInteractWtih(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnAlt_Interact(InputAction.CallbackContext context);
        void OnMiniGames(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnMoveObj(InputAction.CallbackContext context);
        void OnFreeCam(InputAction.CallbackContext context);
    }
}
