using System;
using System.Collections.Generic;
using System.Linq;
using CENTIS.XRPlatformManagement.Controller.ProfileBuilding;
using UnityEngine;
using UnityEngine.Events;

namespace CENTIS.XRPlatformManagement.Controller.Manager
{
    public class ControllerModelInteractionData : MonoBehaviour
    {
        #region Fields
        
        [Tooltip("If unset, will be set on awake.")]
        [SerializeField] private ControllerModelSpawner _controllerModelSpawner;
        [SerializeField] private List<ControllerProfileInteractionData> _controllerInteractionData;
        [Space]
        [SerializeField] private Events _events;

        private ControllerModelSpawner ControllerModelSpawner => _controllerModelSpawner;
        public Vector3 LocalInteractionPoint => transform.position + GetCurrentInteractionData().Point;
        public Vector3 LocalInteractionRotationEuler => (transform.rotation * Quaternion.Euler(GetCurrentInteractionData().RotationEuler)).eulerAngles;
        public Quaternion LocalInteractionRotation => transform.rotation * Quaternion.Euler(GetCurrentInteractionData().RotationEuler);
        public Vector3 LocalCenter => transform.position + GetCurrentInteractionData().Center;
        public Vector3 Point => GetCurrentInteractionData().Point;
        public Vector3 RotationEulerEuler => GetCurrentInteractionData().RotationEuler;
        public Quaternion InteractionRotation => Quaternion.Euler(GetCurrentInteractionData().RotationEuler);
        public Vector3 Center => GetCurrentInteractionData().Center;
        
        #endregion
        
        #region Unity Lifecycle

        protected virtual void Awake()
        {
            if (_controllerModelSpawner == null)
            {
                _controllerModelSpawner = gameObject.GetComponent<ControllerModelSpawner>();
            }

            ControllerModelSpawner.EventType trackingInitialized = ControllerModelSpawner.EventType.OnTrackingInitialized;
            ControllerModelSpawner.EventType finishTrackingAcquired = ControllerModelSpawner.EventType.OnFinishTrackingAcquired;
            ControllerModelSpawner.EventType finishTrackingLost = ControllerModelSpawner.EventType.OnFinishTrackingLost;
            
            _controllerModelSpawner.RegisterAction(trackingInitialized, _ =>
            {
                
                InvokeAction(trackingInitialized);
            });
            _controllerModelSpawner.RegisterAction(finishTrackingAcquired, _ => InvokeAction(finishTrackingAcquired));
            _controllerModelSpawner.RegisterAction(finishTrackingLost, _ => InvokeAction(finishTrackingLost));
        }

        protected virtual void OnDestroy()
        {
            ControllerModelSpawner.EventType trackingInitialized = ControllerModelSpawner.EventType.OnTrackingInitialized;
            ControllerModelSpawner.EventType finishTrackingAcquired = ControllerModelSpawner.EventType.OnFinishTrackingAcquired;
            ControllerModelSpawner.EventType finishTrackingLost = ControllerModelSpawner.EventType.OnFinishTrackingLost;
            
            _controllerModelSpawner.UnregisterAction(trackingInitialized, _ => InvokeAction(trackingInitialized));
            _controllerModelSpawner.UnregisterAction(finishTrackingAcquired, _ => InvokeAction(finishTrackingAcquired));
            _controllerModelSpawner.UnregisterAction(finishTrackingLost, _ => InvokeAction(finishTrackingLost));
        }
        
        #endregion
        
        #region Private Methods
        
        private ControllerProfileInteractionData GetCurrentInteractionData()
        {
            return _controllerInteractionData.FirstOrDefault(controllerProfileInteractionData => controllerProfileInteractionData.ControllerProfile == _controllerModelSpawner.GetCurrentControllerProfile());
        }

        private void InvokeAction(ControllerModelSpawner.EventType eventType)
        {
            _events.OnControllerInteractionDataApplied.Invoke(ControllerModelSpawner, this, eventType);
        }
        
        #endregion
        
        #region Public Methods
        
        public void RegisterAction(UnityAction<ControllerModelSpawner, ControllerModelInteractionData, ControllerModelSpawner.EventType> action)
        {
            _events.OnControllerInteractionDataApplied.AddListener(action);
        }
        
        public void UnregisterAction(UnityAction<ControllerModelSpawner, ControllerModelInteractionData, ControllerModelSpawner.EventType> action)
        {
            _events.OnControllerInteractionDataApplied.RemoveListener(action);
        }
        
        #endregion
        
        #region Data Types
        
        [Serializable]
        private class Events
        {
            [SerializeField] private UnityEvent<ControllerModelSpawner, ControllerModelInteractionData, ControllerModelSpawner.EventType> _onControllerInteractionDataApplied;
            public UnityEvent<ControllerModelSpawner, ControllerModelInteractionData, ControllerModelSpawner.EventType> OnControllerInteractionDataApplied => _onControllerInteractionDataApplied;
        }
        
        [Serializable]
        private class ControllerProfileInteractionData
        {
            [SerializeField] private ControllerProfile _controllerProfile;
            public ControllerProfile ControllerProfile => _controllerProfile;
            
            [SerializeField] private Vector3 _point;
            public Vector3 Point => _point;
            
            [SerializeField] private Vector3 _rotationEuler;
            public Vector3 RotationEuler => _rotationEuler;
            
            [SerializeField] private Vector3 _center;
            public Vector3 Center => _center;
        }
        
        #endregion
    }
}
