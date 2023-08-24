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

        private enum ControllerModel
        {
            HTC,
            Valve,
            Oculus,
            NotSpecified
        }

        [SerializeField] private ControllerModel currentControllerModel = ControllerModel.NotSpecified;

        [SerializeField] private Handedness handedness = Handedness.Left;

        // [SerializeField] private ElementTooltip tooltipInfoPrefab = null;

        [Tooltip("In debug mode the specified controller is used instead on the automatically found controller.")]
        [SerializeField]
        private bool debugMode = false;

        [SerializeField] 
        private ControllerProfile defaultProfile;

        [Header("Instantiated Controller (set during runtime)")]
        
        [SerializeField] private ControllerElement completeModel;
        [SerializeField] private ControllerElement body;
        [SerializeField] private ControllerElement primaryButton;
        [SerializeField] private ControllerElement secondaryButton;
        [SerializeField] private ControllerElement trigger;
        [SerializeField] private ControllerElement systemButton;
        [SerializeField] private ControllerElement thumbStick;
        [SerializeField] private ControllerElement trackpad;
        [SerializeField] private ControllerElement statusLed;
        [SerializeField] private ControllerElement gripButtonPrimary;
        [SerializeField] private ControllerElement gripButtonSecondary;

        [Header("Events")] [SerializeField] 
        private UnityEvent<ControllerModelRender, Vector3, Vector3> modelInitialized;

        private Dictionary<Enum, ControllerElement> _objects = new();
        private Dictionary<string, ControllerProfile> _profiles = new();

        private ControllerProfile _currentProfile;
        
        #region PROPERTIES

        public ControllerElement CompleteModel => completeModel;

        public ControllerElement Body => body;

        public ControllerElement PrimaryButton => primaryButton;

        public ControllerElement SecondaryButton => secondaryButton;

        public ControllerElement Trigger => trigger;

        public ControllerElement SystemButton => systemButton;

        public ControllerElement ThumbStick => thumbStick;

        public ControllerElement Trackpad => trackpad;

        public ControllerElement StatusLed => statusLed;

        public ControllerElement GripButtonPrimary => gripButtonPrimary;

        public ControllerElement GripButtonSecondary => gripButtonSecondary;

        public Dictionary<Enum, ControllerElement> Objects => _objects;

        public ControllerProfile CurrentProfile => _currentProfile;

        #endregion

        #region MONOBEHAVIOUR

        private void Awake()
        {
            // Lookup for existing supported Controllers and load them into a lookup-dict.
            foreach (ControllerProfile profile in Resources.LoadAll<ControllerProfile>("SupportedController"))
            { 
                if (!_profiles.ContainsKey(profile.ManufacturerName))
                {
                    _profiles.Add(profile.ManufacturerName, profile);
                }
            }

            InputDevice device =
                InputDevices.GetDeviceAtXRNode(handedness == Handedness.Left ? XRNode.LeftHand : XRNode.RightHand);

            if (device.isValid)
            {
                LoadController(device);
            }
            else if (debugMode)
            {
                _currentProfile = _profiles[currentControllerModel.ToString()];
                InitializeController();
            }
            else
            {
                _currentProfile = defaultProfile;
                InitializeController();
            }
        }

        private void OnEnable()
        {
            InputTracking.trackingAcquired += InputTrackingOnTrackingAcquired;
            InputTracking.trackingLost += InputTrackingOnTrackingLost;
        }

        private void OnDisable()
        {
            InputTracking.trackingAcquired -= InputTrackingOnTrackingAcquired;
            InputTracking.trackingLost -= InputTrackingOnTrackingLost;
        }

        #endregion

        #region EVENT_SUBSCRIPTIONS

        private void InputTrackingOnTrackingAcquired(XRNodeState node)
        {
            InputDevice controller = InputDevices.GetDeviceAtXRNode(node.nodeType);

            // Check if its a controller and the correct side
            if ((controller.characteristics & InputDeviceCharacteristics.Controller) == 0 ||
                ((controller.characteristics & InputDeviceCharacteristics.Left) != 0) !=
                (handedness == Handedness.Left))
                return;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"Tracking acquired new Controller from {controller.manufacturer} for {handedness}");
#endif
            if (CurrentProfile == null)
            {
                LoadController(controller);
                return;
            }

            if (CurrentProfile.ManufacturerName == controller.manufacturer)
            {
                // just activated previously disabled controller GameObject
                ActivateControllerModels();
            }
            else
            {
                // Cleanup old controller models first
                CleanupControllerModels();

                // Load Controller from Profile
                LoadController(controller);
            }
        }

        private void InputTrackingOnTrackingLost(XRNodeState node)
        {
            InputDevice controller = InputDevices.GetDeviceAtXRNode(node.nodeType);

            // Check if its a controller and the correct side
            if ((controller.characteristics & InputDeviceCharacteristics.Controller) == 0 ||
                ((controller.characteristics & InputDeviceCharacteristics.Left) != 0) !=
                (handedness == Handedness.Left))
                return;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"Controller from {controller.manufacturer} for {handedness} tracking lost");
#endif

            // Disable Controllers
            DeactivateControllerModels();
        }

        #endregion

        private void LoadController(InputDevice controller)
        {
            if (Enum.TryParse(controller.manufacturer, out ControllerModel modelEnum))
            {
                currentControllerModel = modelEnum;
            }
            else
            {
                Debug.LogError($"Could not parse modelName");
            }
            
            

            _currentProfile = _profiles[controller.manufacturer];
            InitializeController();
        }

        private void CleanupControllerModels()
        {
            foreach (ControllerElement element in Objects.Values)
            {
                Destroy(element);
            }

            Objects.Clear();

            _currentProfile = null;
        }

        private void ActivateControllerModels()
        {
            foreach (ControllerElement element in Objects.Values)
            {
                element.gameObject.SetActive(true);
            }
        }

        private void DeactivateControllerModels()
        {
            foreach (ControllerElement element in Objects.Values)
            {
                element.gameObject.SetActive(false);
            }
        }

        private void InitializeController()
        {
            CENTIS.XRPlatform.ControllerModels.ControllerModel settings = handedness == Handedness.Left ? CurrentProfile.LeftHand : CurrentProfile.RightHand;

            if (settings != null)
            {
                InitRenderModel(settings);
                modelInitialized?.Invoke(this, CurrentProfile.PenOffsetPosition, CurrentProfile.PenOffsetRotation);
            }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            else
            {
                Debug.LogError($"Controller couldn't be initialized!");
            }
#endif
        }

        public bool TryGetControllerElementByName(Enum elementName, out ControllerElement element)
        {
            try
            {
                return Objects.TryGetValue(elementName, out element);
            }
            catch (Exception e)
            {
                Debug.LogError(e.StackTrace);
                element = null;
                return false;
            }
        }
        // public string GetControllerElementInfoByName(string elementName)
        // {
        //     try
        //     {
        //         return ToolTipController.Instance.CurrentState.GetFunctionName(elementName);
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogError(e.StackTrace);
        //         return "Tooltip";
        //     }
        // }

        #region HELPER

        private void InitRenderModel(CENTIS.XRPlatform.ControllerModels.ControllerModel controllerModel)
        {
            if (!CurrentProfile.UseParts)
            {
                completeModel = SetupControllerElement(ControllerButtonMask.CompleteModel, controllerModel.CompleteModel);
                return;
            }

            body = SetupControllerElement(ControllerButtonMask.Body,controllerModel.Body);
            primaryButton = SetupControllerElement(ControllerButtonMask.PrimaryButton, controllerModel.PrimaryButton);
            secondaryButton = SetupControllerElement(ControllerButtonMask.SecondaryButton, controllerModel.SecondaryButton);
            trigger = SetupControllerElement(ControllerButtonMask.Trigger, controllerModel.Trigger);
            systemButton = SetupControllerElement(ControllerButtonMask.SystemButton, controllerModel.SystemButton);
            thumbStick = SetupControllerElement(ControllerButtonMask.ThumbStick, controllerModel.ThumbStick);
            trackpad = SetupControllerElement(ControllerButtonMask.Trackpad, controllerModel.Trackpad);
            statusLed = SetupControllerElement(ControllerButtonMask.StatusLED, controllerModel.StatusLed);
            gripButtonPrimary = SetupControllerElement(ControllerButtonMask.GripButtonPrimary, controllerModel.GripButtonPrimary);
            gripButtonSecondary = SetupControllerElement(ControllerButtonMask.GripButtonSecondary, controllerModel.GripButtonSecondary);
        }

        private ControllerElement SetupControllerElement(Enum key, GameObject element)
        {
            if (element == null) return null;

            if (Objects.ContainsKey(key))
            {
                Destroy(Objects[key].gameObject);
                Objects.Remove(key);
            }

            GameObject clone = Instantiate(element, transform, true);
            ControllerElement newElement = clone.AddComponent<ControllerElement>();
            clone.transform.localPosition = Vector3.zero;

            Objects.Add(key, newElement);

            newElement.ControllerElementName = key;
            return newElement;
        }

        private ControllerElement InstantiateAndInitGameObject(GameObject go)
        {
            if (go == null) return null;

            GameObject clone = Instantiate(go, transform, true);
            clone.transform.localPosition = Vector3.zero;
            ControllerElement element = clone.AddComponent<ControllerElement>();

            return element;
        }

        

        #endregion
    }
}