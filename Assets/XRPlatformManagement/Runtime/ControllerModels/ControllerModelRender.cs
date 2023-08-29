using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace CENTIS.XRPlatform.ControllerModels
{
    public class ControllerModelRender : MonoBehaviour
    {
        private enum Handedness
        {
            Left,
            Right
        }

        
        #region Fields
        
        [SerializeField] private Handedness _handedness = Handedness.Left;
        [SerializeField] private ControllerProfile _defaultProfile;
        [SerializeField, Tooltip("The default Controller will be active, event if a new device was enabled")] private bool _alwaysShowDefault;

        [Header("Events")] 
        [SerializeField] private UnityEvent<ControllerModelRender, Vector3, Vector3> _modelInitialized;

        private readonly Dictionary<Enum, ControllerElement> inputDeviceModelsLookup = new();
        private ControllerProfile inputDeviceBufferProfile;
        private readonly Dictionary<Enum, ControllerElement> defaultModelsLookup = new();
        private ControllerProfile defaultBufferProfile;
        private readonly Dictionary<string, ControllerProfile> profiles = new();
        
        public ControllerElement CompleteModel => completeModel;
        private ControllerElement completeModel;

        public ControllerElement Body => body;
        private ControllerElement body;

        public ControllerElement PrimaryButton => primaryButton;
        private ControllerElement primaryButton;

        public ControllerElement SecondaryButton => secondaryButton;
        private ControllerElement secondaryButton;

        public ControllerElement Trigger => trigger;
        private ControllerElement trigger;

        public ControllerElement SystemButton => systemButton;
        private ControllerElement systemButton;

        public ControllerElement ThumbStick => thumbStick;
        private ControllerElement thumbStick;

        public ControllerElement Trackpad => trackpad;
        private ControllerElement trackpad;

        public ControllerElement StatusLed => statusLed;
        private ControllerElement statusLed;

        public ControllerElement GripButtonPrimary => gripButtonPrimary;
        private ControllerElement gripButtonPrimary;

        public ControllerElement GripButtonSecondary => gripButtonSecondary;
        private ControllerElement gripButtonSecondary;
        
        #endregion
        
        #region Unity Lifecycle
        
        private void Awake()
        {
            InitializeControllerProfiles();

            LoadController(ref defaultBufferProfile, defaultModelsLookup, _defaultProfile);
        }

        private void OnDestroy()
        {
            DestroyInputDevice(ref defaultBufferProfile, defaultModelsLookup);
        }

        private void OnEnable()
        {
            InputTracking.nodeAdded += OnTrackingAcquired;
            InputTracking.nodeRemoved += OnTrackingLost;
        }

        private void OnDisable()
        {
            InputTracking.nodeAdded -= OnTrackingAcquired;
            InputTracking.nodeRemoved -= OnTrackingLost;
        }

        #endregion
        
        #region Private Methods
        
        private void InitializeControllerProfiles()
        {
            // Lookup for existing supported Controllers and load them into a lookup-dict.
            foreach (ControllerProfile profile in Resources.LoadAll<ControllerProfile>("SupportedController"))
            {
                profiles.TryAdd(profile.ManufacturerName, profile);
            }
        }
        
        private void OnTrackingAcquired(XRNodeState node)
        {
            if (_alwaysShowDefault)
            {
                return;
            }
        
            InputDevice controller = InputDevices.GetDeviceAtXRNode(node.nodeType);
            if (!IsValidController(controller))
                return;
        
            if (profiles.TryGetValue(controller.manufacturer, out var profile))
            {
                DisableDefaultInputDevice();
                LoadController(ref inputDeviceBufferProfile, inputDeviceModelsLookup, profile);
            }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            else
            {
                Debug.LogWarning($"Couldn't load profile with the name {controller.manufacturer} because it was not found at loading!");
            }
#endif
        }

        private void OnTrackingLost(XRNodeState node)
        {
            InputDevice controller = InputDevices.GetDeviceAtXRNode(node.nodeType);
            if (!IsValidController(controller) || inputDeviceBufferProfile == null)
                return;

            DestroyInputDevice(ref inputDeviceBufferProfile, inputDeviceModelsLookup);
            EnableDefaultInputDevice();
        }
        
        private bool IsValidController(InputDevice controller)
        {
            // Check if its a controller and the correct side
            return (controller.characteristics & InputDeviceCharacteristics.Controller) == 0 ||
                   ((controller.characteristics & InputDeviceCharacteristics.Left) != 0) !=
                   (_handedness == Handedness.Left);
        }

        private void LoadController(ref ControllerProfile writtenProfile, Dictionary<Enum, ControllerElement> modelsLookup, ControllerProfile newProfile)
        {
            ControllerModel settings = _handedness == Handedness.Left ? newProfile.LeftHand : newProfile.RightHand;
            if (settings != null)
            {
                writtenProfile = newProfile;
                InitRenderModel(writtenProfile, modelsLookup, settings);
                _modelInitialized?.Invoke(this, writtenProfile.PenOffsetPosition, writtenProfile.PenOffsetRotation);
            }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            else
            {
                Debug.LogError($"Controller couldn't be initialized!");
            }
#endif
        }

        private void DestroyInputDevice(ref ControllerProfile profile, Dictionary<Enum, ControllerElement> modelsLookup)
        {
            foreach (ControllerElement element in modelsLookup.Values)
            {
                Destroy(element);
            }
            modelsLookup.Clear();
            profile = null;
        }

        private void EnableDefaultInputDevice()
        {
            foreach (ControllerElement element in defaultModelsLookup.Values)
            {
                element.gameObject.SetActive(true);
            }
        }

        private void DisableDefaultInputDevice()
        {
            foreach (ControllerElement element in defaultModelsLookup.Values)
            {
                element.gameObject.SetActive(false);
            }
        }
        #endregion
        
        #region HELPER
        
        private void InitRenderModel(ControllerProfile bufferedProfile, Dictionary<Enum, ControllerElement> modelsLookup, ControllerModel controllerModel)
        {
            if (!bufferedProfile.UseParts)
            {
                completeModel = InstantiateControllerElement(modelsLookup, ControllerButtonMask.CompleteModel, controllerModel.CompleteModel);
                return;
            }

            body = InstantiateControllerElement(modelsLookup, ControllerButtonMask.Body,controllerModel.Body);
            primaryButton = InstantiateControllerElement(modelsLookup, ControllerButtonMask.PrimaryButton, controllerModel.PrimaryButton);
            secondaryButton = InstantiateControllerElement(modelsLookup, ControllerButtonMask.SecondaryButton, controllerModel.SecondaryButton);
            trigger = InstantiateControllerElement(modelsLookup, ControllerButtonMask.Trigger, controllerModel.Trigger);
            systemButton = InstantiateControllerElement(modelsLookup, ControllerButtonMask.SystemButton, controllerModel.SystemButton);
            thumbStick = InstantiateControllerElement(modelsLookup, ControllerButtonMask.ThumbStick, controllerModel.ThumbStick);
            trackpad = InstantiateControllerElement(modelsLookup, ControllerButtonMask.Trackpad, controllerModel.Trackpad);
            statusLed = InstantiateControllerElement(modelsLookup, ControllerButtonMask.StatusLED, controllerModel.StatusLed);
            gripButtonPrimary = InstantiateControllerElement(modelsLookup, ControllerButtonMask.GripButtonPrimary, controllerModel.GripButtonPrimary);
            gripButtonSecondary = InstantiateControllerElement(modelsLookup, ControllerButtonMask.GripButtonSecondary, controllerModel.GripButtonSecondary);
        }

        private ControllerElement InstantiateControllerElement(Dictionary<Enum, ControllerElement> modelsLookup, Enum key, GameObject element)
        {
            if (element == null) return null;

            if (modelsLookup.ContainsKey(key))
            {
                Destroy(modelsLookup[key].gameObject);
                modelsLookup.Remove(key);
            }

            GameObject clone = Instantiate(element, transform, true);
            ControllerElement newElement = clone.AddComponent<ControllerElement>();
            modelsLookup.Add(key, newElement);
            newElement.ControllerElementName = key;
            return newElement;
        }
        
        #endregion
        
        #region public methods
        
        public Dictionary<Enum, ControllerElement> GetCurrentModelsLookup()
        {
            if (inputDeviceBufferProfile != null && !_alwaysShowDefault)
            {
                return inputDeviceModelsLookup;
            }

            return defaultModelsLookup;
        }
        
        #endregion
    }
}
