using System;
using System.Collections.Generic;
using CENTIS.XRPlatformManagement;
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

        [Tooltip("The Profiles of the Controllers, that can be loaded. If null it will call Resources.LoadAll<ControllerProfile>(\"SupportedController\")")]
        [SerializeField] private ControllerProfileSet _controllerProfileSet;
        [SerializeField] private Handedness _handedness = Handedness.Left;
        [Tooltip("The default Controller that will be shown, if no Controller was found. Can be Null, in which case no Controller will be instantiated!")]
        [SerializeField] private ControllerProfile _defaultProfile;
        [Tooltip("The default Controller will be active, event if a new device was enabled")]
        [SerializeField] private bool _alwaysShowDefault;
        [SerializeField] private Transform _modelParent;

        [Header("Events")] 
        [SerializeField] private UnityEvent _onTrackingAcquired;
        [SerializeField] private UnityEvent _onTrackingLost;
        [SerializeField] private UnityEvent<ControllerModelRender, Vector3, Vector3> _modelInitialized;

        private readonly Dictionary<Enum, ControllerElement> inputDeviceModelsLookup = new();
        private ControllerProfile inputDeviceBufferProfile;
        private readonly Dictionary<Enum, ControllerElement> defaultModelsLookup = new();
        private ControllerProfile defaultBufferProfile;
        private readonly Dictionary<string, ControllerProfile> profiles = new();
        
        #endregion
        
        #region Unity Lifecycle
        
        private void Awake()
        {
            if (_modelParent == null)
            {
                _modelParent = new GameObject($"[{gameObject.name}] Model Parent").transform;
                _modelParent.SetParent(transform, false);
                _modelParent.localPosition = Vector3.zero;
                _modelParent.localRotation = Quaternion.identity;
            }
            
            InitializeControllerProfiles();
            
            if (_defaultProfile != null)
            {
                LoadController(ref defaultBufferProfile, defaultModelsLookup, _defaultProfile);
            }

            InputTracking.trackingAcquired += OnTrackingAcquired;
            InputTracking.trackingLost += OnTrackingLost;
        }

        private void OnDestroy()
        {
            InputTracking.trackingAcquired -= OnTrackingAcquired;
            InputTracking.trackingLost -= OnTrackingLost;
            
            DestroyInputDevice(ref defaultBufferProfile, defaultModelsLookup);
        }

        #endregion
        
        #region Private Methods
        
        private void InitializeControllerProfiles()
        {
            if (_controllerProfileSet != null)
            {
                for (int i = 0; i < _controllerProfileSet.Count(); i++)
                {
                    ControllerProfile profile = _controllerProfileSet.GetAt(i);
                    profiles.TryAdd(profile.ManufacturerType.type, profile);
                }
            }
            else
            {
                // Lookup for existing supported Controllers and load them into a lookup-dict.
                foreach (ControllerProfile profile in Resources.LoadAll<ControllerProfile>("SupportedController"))
                {
                    profiles.TryAdd(profile.ManufacturerType.type, profile);
                }
            }
        }
        
        private void OnTrackingAcquired(XRNodeState node)
        {
            Debug.Log("Tracking Aquired");
            
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
                _onTrackingAcquired?.Invoke();
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
            Debug.Log("Tracking Lost");
            InputDevice controller = InputDevices.GetDeviceAtXRNode(node.nodeType);
            if (!IsValidController(controller) || inputDeviceBufferProfile == null)
                return;

            DestroyInputDevice(ref inputDeviceBufferProfile, inputDeviceModelsLookup);
            EnableDefaultInputDevice();
            _onTrackingLost?.Invoke();
        }
        
        /// <summary>
        /// Check if its a controller and the correct side
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        private bool IsValidController(InputDevice controller)
        {
            return (controller.characteristics & InputDeviceCharacteristics.HeldInHand) == 0 ||
                   (controller.characteristics & InputDeviceCharacteristics.TrackedDevice) == 0 ||
                   (controller.characteristics & InputDeviceCharacteristics.Controller) == 0 ||
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
                Destroy(element.gameObject);
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
                InstantiateControllerElement(modelsLookup, ControllerButtonMask.CompleteModel, controllerModel.GetModelByMask(ControllerButtonMask.CompleteModel));
                return;
            }
            
            foreach (ControllerButtonMask controllerButtonMask in (ControllerButtonMask[]) Enum.GetValues(typeof(ControllerButtonMask)))
            {
                if (controllerButtonMask != ControllerButtonMask.CompleteModel && controllerButtonMask != ControllerButtonMask.None)
                {
                    InstantiateControllerElement(modelsLookup, controllerButtonMask, controllerModel.GetModelByMask(controllerButtonMask));
                }
            }
        }

        private void InstantiateControllerElement(Dictionary<Enum, ControllerElement> modelsLookup, Enum key, GameObject element)
        {
            if (element == null) return;

            if (modelsLookup.ContainsKey(key))
            {
                Destroy(modelsLookup[key].gameObject);
                modelsLookup.Remove(key);
            }

            GameObject clone = Instantiate(element, _modelParent, false);
            ControllerElement newElement = clone.AddComponent<ControllerElement>();
            modelsLookup.Add(key, newElement);
            newElement.ControllerElementName = key;
        }
        
        #endregion
        
        #region public

        public ControllerProfile GetCurrentControllerProfile()
        {
            if (inputDeviceBufferProfile != null && !_alwaysShowDefault)
            {
                return inputDeviceBufferProfile;
            }

            if (defaultBufferProfile == null)
            {
                Debug.LogWarning("Couldn't get a controller profile due to none registered right now.");
                return null;
            }

            return defaultBufferProfile;
        }
        
        public ControllerModel GetCurrentControllerModel()
        {
            ControllerProfile currentControllerProfile = GetCurrentControllerProfile();
            return _handedness == Handedness.Left
                ? currentControllerProfile.LeftHand
                : currentControllerProfile.RightHand;
        }
        
        public Dictionary<Enum, ControllerElement> GetCurrentModelsLookup()
        {
            if (inputDeviceBufferProfile != null && !_alwaysShowDefault)
            {
                return inputDeviceModelsLookup;
            }
            
            if (defaultModelsLookup == null)
            {
                Debug.LogWarning("Couldn't get a models lookup due to none registered right now.");
                return null;
            }

            return defaultModelsLookup;
        }
        
        public bool TryGetCurrentModelsLookupByType(ControllerButtonMask controllerButtonMask, out ControllerElement controllerElement)
        {
            return GetCurrentModelsLookup().TryGetValue(controllerButtonMask, out controllerElement);
        }
        
        #endregion
    }
}
