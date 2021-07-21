// GENERATED AUTOMATICALLY FROM 'Assets/InputController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputController : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputController"",
    ""maps"": [
        {
            ""name"": ""Match Controller"",
            ""id"": ""f8c2e74f-1ec9-4ea3-a1a4-4dc04b054f36"",
            ""actions"": [
                {
                    ""name"": ""Start/End Time"",
                    ""type"": ""Button"",
                    ""id"": ""8ea0f4c7-30f5-4d19-b009-a3ce3b1116de"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Home Goal Up"",
                    ""type"": ""Button"",
                    ""id"": ""9bab24ff-dd0b-4848-b682-4dd58bbc6278"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Home Goal Down"",
                    ""type"": ""Button"",
                    ""id"": ""c23e8d2f-2913-4b3a-81af-266bec66923b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Home Fault"",
                    ""type"": ""Button"",
                    ""id"": ""f4755938-4f67-4606-b06b-33cd2919b156"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Away Goal Up"",
                    ""type"": ""Button"",
                    ""id"": ""2235cb38-66c3-4fcd-a17f-9cec4d2ce9a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Away Goal Down"",
                    ""type"": ""Button"",
                    ""id"": ""27f9d23f-4e3e-4076-8a5b-1b7442bbbeb5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Away Fault"",
                    ""type"": ""Button"",
                    ""id"": ""eac9b861-b99c-435e-a33d-1ab158d0bdfa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Timeout"",
                    ""type"": ""Button"",
                    ""id"": ""41006ae5-b3ba-4f7c-9dbd-f43788d448f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9f139eaf-18a8-4e39-974e-835e63887e92"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Start/End Time"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0843abc9-4ade-444a-85a0-e9ee4bab308b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Start/End Time"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a103560-a20b-4175-9ecc-6db7f0226f11"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Home Goal Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a629c15f-f6fb-4614-8ffc-4c124ed289f0"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Home Goal Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd5ce545-a441-4e70-a759-ae61d10e149a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Home Goal Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6fcc07c-c674-4c2f-a4c2-f393c8a4445b"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Home Goal Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e7379e0-2315-45aa-9fc1-c6b39164ab6e"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Home Fault"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93434c26-6526-4ec3-bceb-7c63437818de"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Home Fault"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb9335d0-cd6e-47aa-974c-d4b1e03feddc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Away Goal Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""046cfa03-0b25-4880-8c3f-2f6c10c307b8"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Away Goal Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""660651a3-7eb6-4683-866f-f6c87f9ad320"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Away Goal Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""319204b8-5b50-4743-b7f0-66da0894c1be"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Away Goal Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c05fad6a-959c-49e9-a6fb-75e437d9d0ef"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Away Fault"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20133148-920b-4278-8d25-2eba3d515692"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Away Fault"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d09984e7-aebb-42c7-9bb3-8b174f8fbbed"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Timeout"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30000e9e-2308-422a-a6df-3ed9c4315c56"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Timeout"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
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
        // Match Controller
        m_MatchController = asset.FindActionMap("Match Controller", throwIfNotFound: true);
        m_MatchController_StartEndTime = m_MatchController.FindAction("Start/End Time", throwIfNotFound: true);
        m_MatchController_HomeGoalUp = m_MatchController.FindAction("Home Goal Up", throwIfNotFound: true);
        m_MatchController_HomeGoalDown = m_MatchController.FindAction("Home Goal Down", throwIfNotFound: true);
        m_MatchController_HomeFault = m_MatchController.FindAction("Home Fault", throwIfNotFound: true);
        m_MatchController_AwayGoalUp = m_MatchController.FindAction("Away Goal Up", throwIfNotFound: true);
        m_MatchController_AwayGoalDown = m_MatchController.FindAction("Away Goal Down", throwIfNotFound: true);
        m_MatchController_AwayFault = m_MatchController.FindAction("Away Fault", throwIfNotFound: true);
        m_MatchController_Timeout = m_MatchController.FindAction("Timeout", throwIfNotFound: true);
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

    // Match Controller
    private readonly InputActionMap m_MatchController;
    private IMatchControllerActions m_MatchControllerActionsCallbackInterface;
    private readonly InputAction m_MatchController_StartEndTime;
    private readonly InputAction m_MatchController_HomeGoalUp;
    private readonly InputAction m_MatchController_HomeGoalDown;
    private readonly InputAction m_MatchController_HomeFault;
    private readonly InputAction m_MatchController_AwayGoalUp;
    private readonly InputAction m_MatchController_AwayGoalDown;
    private readonly InputAction m_MatchController_AwayFault;
    private readonly InputAction m_MatchController_Timeout;
    public struct MatchControllerActions
    {
        private @InputController m_Wrapper;
        public MatchControllerActions(@InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @StartEndTime => m_Wrapper.m_MatchController_StartEndTime;
        public InputAction @HomeGoalUp => m_Wrapper.m_MatchController_HomeGoalUp;
        public InputAction @HomeGoalDown => m_Wrapper.m_MatchController_HomeGoalDown;
        public InputAction @HomeFault => m_Wrapper.m_MatchController_HomeFault;
        public InputAction @AwayGoalUp => m_Wrapper.m_MatchController_AwayGoalUp;
        public InputAction @AwayGoalDown => m_Wrapper.m_MatchController_AwayGoalDown;
        public InputAction @AwayFault => m_Wrapper.m_MatchController_AwayFault;
        public InputAction @Timeout => m_Wrapper.m_MatchController_Timeout;
        public InputActionMap Get() { return m_Wrapper.m_MatchController; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MatchControllerActions set) { return set.Get(); }
        public void SetCallbacks(IMatchControllerActions instance)
        {
            if (m_Wrapper.m_MatchControllerActionsCallbackInterface != null)
            {
                @StartEndTime.started -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnStartEndTime;
                @StartEndTime.performed -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnStartEndTime;
                @StartEndTime.canceled -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnStartEndTime;
                @HomeGoalUp.started -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnHomeGoalUp;
                @HomeGoalUp.performed -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnHomeGoalUp;
                @HomeGoalUp.canceled -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnHomeGoalUp;
                @HomeGoalDown.started -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnHomeGoalDown;
                @HomeGoalDown.performed -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnHomeGoalDown;
                @HomeGoalDown.canceled -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnHomeGoalDown;
                @HomeFault.started -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnHomeFault;
                @HomeFault.performed -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnHomeFault;
                @HomeFault.canceled -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnHomeFault;
                @AwayGoalUp.started -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnAwayGoalUp;
                @AwayGoalUp.performed -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnAwayGoalUp;
                @AwayGoalUp.canceled -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnAwayGoalUp;
                @AwayGoalDown.started -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnAwayGoalDown;
                @AwayGoalDown.performed -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnAwayGoalDown;
                @AwayGoalDown.canceled -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnAwayGoalDown;
                @AwayFault.started -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnAwayFault;
                @AwayFault.performed -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnAwayFault;
                @AwayFault.canceled -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnAwayFault;
                @Timeout.started -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnTimeout;
                @Timeout.performed -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnTimeout;
                @Timeout.canceled -= m_Wrapper.m_MatchControllerActionsCallbackInterface.OnTimeout;
            }
            m_Wrapper.m_MatchControllerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @StartEndTime.started += instance.OnStartEndTime;
                @StartEndTime.performed += instance.OnStartEndTime;
                @StartEndTime.canceled += instance.OnStartEndTime;
                @HomeGoalUp.started += instance.OnHomeGoalUp;
                @HomeGoalUp.performed += instance.OnHomeGoalUp;
                @HomeGoalUp.canceled += instance.OnHomeGoalUp;
                @HomeGoalDown.started += instance.OnHomeGoalDown;
                @HomeGoalDown.performed += instance.OnHomeGoalDown;
                @HomeGoalDown.canceled += instance.OnHomeGoalDown;
                @HomeFault.started += instance.OnHomeFault;
                @HomeFault.performed += instance.OnHomeFault;
                @HomeFault.canceled += instance.OnHomeFault;
                @AwayGoalUp.started += instance.OnAwayGoalUp;
                @AwayGoalUp.performed += instance.OnAwayGoalUp;
                @AwayGoalUp.canceled += instance.OnAwayGoalUp;
                @AwayGoalDown.started += instance.OnAwayGoalDown;
                @AwayGoalDown.performed += instance.OnAwayGoalDown;
                @AwayGoalDown.canceled += instance.OnAwayGoalDown;
                @AwayFault.started += instance.OnAwayFault;
                @AwayFault.performed += instance.OnAwayFault;
                @AwayFault.canceled += instance.OnAwayFault;
                @Timeout.started += instance.OnTimeout;
                @Timeout.performed += instance.OnTimeout;
                @Timeout.canceled += instance.OnTimeout;
            }
        }
    }
    public MatchControllerActions @MatchController => new MatchControllerActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
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
    public interface IMatchControllerActions
    {
        void OnStartEndTime(InputAction.CallbackContext context);
        void OnHomeGoalUp(InputAction.CallbackContext context);
        void OnHomeGoalDown(InputAction.CallbackContext context);
        void OnHomeFault(InputAction.CallbackContext context);
        void OnAwayGoalUp(InputAction.CallbackContext context);
        void OnAwayGoalDown(InputAction.CallbackContext context);
        void OnAwayFault(InputAction.CallbackContext context);
        void OnTimeout(InputAction.CallbackContext context);
    }
}
