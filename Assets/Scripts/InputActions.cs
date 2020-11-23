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
            ""name"": ""Swimming"",
            ""id"": ""b2ef0e01-46f9-4f2c-a3fb-1275cd22270f"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""aa76ede9-503b-4214-8054-ee5c598d8d69"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""dc2077f4-76d7-4b2b-af80-fe480d983995"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""c24640dd-657e-46b9-8d0d-e89060a51783"",
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
                    ""id"": ""6b3f915b-7c8b-45d8-b9df-d91fc62cf6ff"",
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
                    ""id"": ""da844862-c5ac-4535-8697-54241d6d4242"",
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
                    ""id"": ""f36a6435-119c-4268-88b9-bb64fd6bb458"",
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
                    ""id"": ""ed2b1412-dd30-4599-922a-532ffcad7797"",
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
                    ""id"": ""e736f5ca-96b4-4e45-a42a-5bbca3a40387"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Biking"",
            ""id"": ""bc100f3f-18ac-4a4a-ae83-bafdd85413cd"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1b71e55b-6f7c-411b-9ff6-dfee60f8c157"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f821413a-e5c4-442a-9a25-631d38f72d25"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""1c5a5bed-0ac2-4a28-838e-f346db48871a"",
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
                    ""id"": ""61a15c95-1ad7-462a-9fce-458d2cdcd273"",
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
                    ""id"": ""9c79acbd-4602-47ad-a921-7f66020329e8"",
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
                    ""id"": ""9846b6e4-ab8a-4c5b-ad96-c1a94c902f8e"",
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
                    ""id"": ""18b63607-89ba-4aed-9ccd-dee9a5c50fc2"",
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
                    ""id"": ""51f2de77-7cdd-4dba-9a15-b15a84704189"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Jetpacking"",
            ""id"": ""fa7ebb7c-390a-44e2-aa31-25f84c515f18"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""007b87fc-44d8-42b7-aa9e-9ed20e045433"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""39edf10f-15e6-4ee5-9cdb-c4d1a7ef07e0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Value"",
                    ""id"": ""e28d79d8-fa6f-432a-a85d-0a83b7757a64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""e7190954-4e1e-44fa-b885-7eae5f7bbe62"",
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
                    ""id"": ""d8b79142-eae9-4907-b9fa-aa32d8ac0baf"",
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
                    ""id"": ""5d7a940a-8966-4f53-9103-ffd135fe9d45"",
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
                    ""id"": ""4fa88fce-3f46-4cd0-9e70-069bc0454ea5"",
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
                    ""id"": ""2bf002ae-f0a9-4d10-b36e-0d937de44e07"",
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
                    ""id"": ""a7656801-239d-4883-8ca6-ed5d62e7d6cf"",
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
                    ""id"": ""521a519b-7fe8-4cc3-b5bd-84dc832987b4"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Look"",
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
                },
                {
                    ""name"": ""Die"",
                    ""type"": ""Button"",
                    ""id"": ""b2043a1e-4252-4bec-afc1-6c92eb7096df"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""1ce8b14e-a2e6-4d08-a813-b027a3ac4c6f"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Die"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Driving"",
            ""id"": ""f14021a1-234f-40e7-a36d-7fd0c6e36ca1"",
            ""actions"": [
                {
                    ""name"": ""Steer"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5a1044ce-5bf2-4ffc-93c1-6bd0907ffc6a"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Accelerate"",
                    ""type"": ""Button"",
                    ""id"": ""499eaa70-296c-4cf6-a4b7-f81b58326225"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bf0efc01-2c93-41c6-b6c0-89ada0763955"",
                    ""path"": ""<Mouse>/position/x"",
                    ""interactions"": """",
                    ""processors"": ""Clamp(min=-200,max=200),Normalize(min=-200,max=200)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96425631-a7d1-4f72-b019-f6a0ebabe8eb"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Accelerate"",
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
        // Swimming
        m_Swimming = asset.FindActionMap("Swimming", throwIfNotFound: true);
        m_Swimming_Movement = m_Swimming.FindAction("Movement", throwIfNotFound: true);
        m_Swimming_Look = m_Swimming.FindAction("Look", throwIfNotFound: true);
        // Biking
        m_Biking = asset.FindActionMap("Biking", throwIfNotFound: true);
        m_Biking_Movement = m_Biking.FindAction("Movement", throwIfNotFound: true);
        m_Biking_Look = m_Biking.FindAction("Look", throwIfNotFound: true);
        // Jetpacking
        m_Jetpacking = asset.FindActionMap("Jetpacking", throwIfNotFound: true);
        m_Jetpacking_Movement = m_Jetpacking.FindAction("Movement", throwIfNotFound: true);
        m_Jetpacking_Look = m_Jetpacking.FindAction("Look", throwIfNotFound: true);
        m_Jetpacking_Jump = m_Jetpacking.FindAction("Jump", throwIfNotFound: true);
        // Debug
        m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
        m_Debug_SlowTime = m_Debug.FindAction("SlowTime", throwIfNotFound: true);
        m_Debug_Die = m_Debug.FindAction("Die", throwIfNotFound: true);
        // Driving
        m_Driving = asset.FindActionMap("Driving", throwIfNotFound: true);
        m_Driving_Steer = m_Driving.FindAction("Steer", throwIfNotFound: true);
        m_Driving_Accelerate = m_Driving.FindAction("Accelerate", throwIfNotFound: true);
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

    // Swimming
    private readonly InputActionMap m_Swimming;
    private ISwimmingActions m_SwimmingActionsCallbackInterface;
    private readonly InputAction m_Swimming_Movement;
    private readonly InputAction m_Swimming_Look;
    public struct SwimmingActions
    {
        private @InputActions m_Wrapper;
        public SwimmingActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Swimming_Movement;
        public InputAction @Look => m_Wrapper.m_Swimming_Look;
        public InputActionMap Get() { return m_Wrapper.m_Swimming; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SwimmingActions set) { return set.Get(); }
        public void SetCallbacks(ISwimmingActions instance)
        {
            if (m_Wrapper.m_SwimmingActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_SwimmingActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_SwimmingActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_SwimmingActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_SwimmingActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_SwimmingActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_SwimmingActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_SwimmingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
            }
        }
    }
    public SwimmingActions @Swimming => new SwimmingActions(this);

    // Biking
    private readonly InputActionMap m_Biking;
    private IBikingActions m_BikingActionsCallbackInterface;
    private readonly InputAction m_Biking_Movement;
    private readonly InputAction m_Biking_Look;
    public struct BikingActions
    {
        private @InputActions m_Wrapper;
        public BikingActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Biking_Movement;
        public InputAction @Look => m_Wrapper.m_Biking_Look;
        public InputActionMap Get() { return m_Wrapper.m_Biking; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BikingActions set) { return set.Get(); }
        public void SetCallbacks(IBikingActions instance)
        {
            if (m_Wrapper.m_BikingActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_BikingActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_BikingActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_BikingActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_BikingActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_BikingActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_BikingActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_BikingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
            }
        }
    }
    public BikingActions @Biking => new BikingActions(this);

    // Jetpacking
    private readonly InputActionMap m_Jetpacking;
    private IJetpackingActions m_JetpackingActionsCallbackInterface;
    private readonly InputAction m_Jetpacking_Movement;
    private readonly InputAction m_Jetpacking_Look;
    private readonly InputAction m_Jetpacking_Jump;
    public struct JetpackingActions
    {
        private @InputActions m_Wrapper;
        public JetpackingActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Jetpacking_Movement;
        public InputAction @Look => m_Wrapper.m_Jetpacking_Look;
        public InputAction @Jump => m_Wrapper.m_Jetpacking_Jump;
        public InputActionMap Get() { return m_Wrapper.m_Jetpacking; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(JetpackingActions set) { return set.Get(); }
        public void SetCallbacks(IJetpackingActions instance)
        {
            if (m_Wrapper.m_JetpackingActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_JetpackingActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_JetpackingActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_JetpackingActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_JetpackingActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_JetpackingActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_JetpackingActionsCallbackInterface.OnLook;
                @Jump.started -= m_Wrapper.m_JetpackingActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_JetpackingActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_JetpackingActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_JetpackingActionsCallbackInterface = instance;
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
    public JetpackingActions @Jetpacking => new JetpackingActions(this);

    // Debug
    private readonly InputActionMap m_Debug;
    private IDebugActions m_DebugActionsCallbackInterface;
    private readonly InputAction m_Debug_SlowTime;
    private readonly InputAction m_Debug_Die;
    public struct DebugActions
    {
        private @InputActions m_Wrapper;
        public DebugActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @SlowTime => m_Wrapper.m_Debug_SlowTime;
        public InputAction @Die => m_Wrapper.m_Debug_Die;
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
                @Die.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnDie;
                @Die.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnDie;
                @Die.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnDie;
            }
            m_Wrapper.m_DebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SlowTime.started += instance.OnSlowTime;
                @SlowTime.performed += instance.OnSlowTime;
                @SlowTime.canceled += instance.OnSlowTime;
                @Die.started += instance.OnDie;
                @Die.performed += instance.OnDie;
                @Die.canceled += instance.OnDie;
            }
        }
    }
    public DebugActions @Debug => new DebugActions(this);

    // Driving
    private readonly InputActionMap m_Driving;
    private IDrivingActions m_DrivingActionsCallbackInterface;
    private readonly InputAction m_Driving_Steer;
    private readonly InputAction m_Driving_Accelerate;
    public struct DrivingActions
    {
        private @InputActions m_Wrapper;
        public DrivingActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Steer => m_Wrapper.m_Driving_Steer;
        public InputAction @Accelerate => m_Wrapper.m_Driving_Accelerate;
        public InputActionMap Get() { return m_Wrapper.m_Driving; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DrivingActions set) { return set.Get(); }
        public void SetCallbacks(IDrivingActions instance)
        {
            if (m_Wrapper.m_DrivingActionsCallbackInterface != null)
            {
                @Steer.started -= m_Wrapper.m_DrivingActionsCallbackInterface.OnSteer;
                @Steer.performed -= m_Wrapper.m_DrivingActionsCallbackInterface.OnSteer;
                @Steer.canceled -= m_Wrapper.m_DrivingActionsCallbackInterface.OnSteer;
                @Accelerate.started -= m_Wrapper.m_DrivingActionsCallbackInterface.OnAccelerate;
                @Accelerate.performed -= m_Wrapper.m_DrivingActionsCallbackInterface.OnAccelerate;
                @Accelerate.canceled -= m_Wrapper.m_DrivingActionsCallbackInterface.OnAccelerate;
            }
            m_Wrapper.m_DrivingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Steer.started += instance.OnSteer;
                @Steer.performed += instance.OnSteer;
                @Steer.canceled += instance.OnSteer;
                @Accelerate.started += instance.OnAccelerate;
                @Accelerate.performed += instance.OnAccelerate;
                @Accelerate.canceled += instance.OnAccelerate;
            }
        }
    }
    public DrivingActions @Driving => new DrivingActions(this);
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
    public interface ISwimmingActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
    public interface IBikingActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
    public interface IJetpackingActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
    public interface IDebugActions
    {
        void OnSlowTime(InputAction.CallbackContext context);
        void OnDie(InputAction.CallbackContext context);
    }
    public interface IDrivingActions
    {
        void OnSteer(InputAction.CallbackContext context);
        void OnAccelerate(InputAction.CallbackContext context);
    }
}
