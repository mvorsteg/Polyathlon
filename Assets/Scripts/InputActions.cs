// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Running"",
            ""id"": ""5336a2dc-329d-4b49-b7d4-0fc4962b3579"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""af720142-01e4-43bb-a8a8-3725b582fe55"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""28dfa16e-9758-4310-8265-3ef6cc506c06"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""f70547e9-a07e-4ba0-b39b-eac63638aff3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""ed0af60f-25a1-4901-95aa-549bad3e2cfd"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""aaf67ea6-ed93-4a92-9bdf-00935a651106"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1995a841-af20-4e2c-a5ee-c98e7980e875"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""420713d2-f507-4110-93a8-5e9cc9465f45"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""17a5eeaf-a634-40a9-ad7c-8b1231a03e22"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""277703a5-cc04-49b2-906c-4052058bc7b0"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""92bd6738-1ba2-4582-bcbe-19674595ee3a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debug"",
            ""id"": ""6311b186-9385-4739-8d72-970c1f919cff"",
            ""actions"": [
                {
                    ""name"": ""SlowTime"",
                    ""type"": ""Button"",
                    ""id"": ""d2437af0-ae33-453f-b64c-9f16c35f1fd9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1c82d449-71a6-4200-a92b-d7bb94f299bc"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SlowTime"",
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
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
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
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Running
        m_Running = asset.FindActionMap("Running", throwIfNotFound: true);
        m_Running_Movement = m_Running.FindAction("Movement", throwIfNotFound: true);
        m_Running_Look = m_Running.FindAction("Look", throwIfNotFound: true);
        m_Running_Jump = m_Running.FindAction("Jump", throwIfNotFound: true);
        // Debug
        m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
        m_Debug_SlowTime = m_Debug.FindAction("SlowTime", throwIfNotFound: true);
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

    // Running
    private readonly InputActionMap m_Running;
    private IRunningActions m_RunningActionsCallbackInterface;
    private readonly InputAction m_Running_Movement;
    private readonly InputAction m_Running_Look;
    private readonly InputAction m_Running_Jump;
    public struct RunningActions
    {
        private @InputActions m_Wrapper;
        public RunningActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Running_Movement;
        public InputAction @Look => m_Wrapper.m_Running_Look;
        public InputAction @Jump => m_Wrapper.m_Running_Jump;
        public InputActionMap Get() { return m_Wrapper.m_Running; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RunningActions set) { return set.Get(); }
        public void SetCallbacks(IRunningActions instance)
        {
            if (m_Wrapper.m_RunningActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_RunningActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_RunningActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_RunningActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_RunningActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_RunningActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_RunningActionsCallbackInterface.OnLook;
                @Jump.started -= m_Wrapper.m_RunningActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_RunningActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_RunningActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_RunningActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }
        }
    }
    public RunningActions @Running => new RunningActions(this);

    // Debug
    private readonly InputActionMap m_Debug;
    private IDebugActions m_DebugActionsCallbackInterface;
    private readonly InputAction m_Debug_SlowTime;
    public struct DebugActions
    {
        private @InputActions m_Wrapper;
        public DebugActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @SlowTime => m_Wrapper.m_Debug_SlowTime;
        public InputActionMap Get() { return m_Wrapper.m_Debug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
        public void SetCallbacks(IDebugActions instance)
        {
            if (m_Wrapper.m_DebugActionsCallbackInterface != null)
            {
                @SlowTime.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnSlowTime;
                @SlowTime.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnSlowTime;
                @SlowTime.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnSlowTime;
            }
            m_Wrapper.m_DebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SlowTime.started += instance.OnSlowTime;
                @SlowTime.performed += instance.OnSlowTime;
                @SlowTime.canceled += instance.OnSlowTime;
            }
        }
    }
    public DebugActions @Debug => new DebugActions(this);
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
    public interface IRunningActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
    public interface IDebugActions
    {
        void OnSlowTime(InputAction.CallbackContext context);
    }
}
