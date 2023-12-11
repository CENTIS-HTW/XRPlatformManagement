using System;
using System.Collections.Generic;
using System.Linq;
using CENTIS.XRPlatformManagement.Controller.Elements;
using CENTIS.XRPlatformManagement.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace CENTIS.XRPlatformManagement.Controller.Manager
{
    [RequireComponent(typeof(ControllerModelSpawner))]
    public abstract class BaseControllerElementRegistrator<T> : MonoBehaviour where T : Component
    {
        #region Fields
        
        [Header("Base")]
        [Tooltip("If unset, will be set on awake.")]
        [SerializeField] private ControllerModelSpawner _controllerModelSpawner;
        [SerializeField] private ControllerModelButtonMask _modelButtonMask;

        protected ControllerModelSpawner ControllerModelSpawner => _controllerModelSpawner;
        protected ControllerModelButtonMask ModelButtonMask => _modelButtonMask;
        protected readonly Dictionary<Enum, T> ElementLookup = new();
        
        #endregion

        #region Unity Lifecycle
        
        protected virtual void Awake()
        {
            if (_controllerModelSpawner == null)
            {
                _controllerModelSpawner = gameObject.GetComponent<ControllerModelSpawner>();
            }
            
            _controllerModelSpawner.RegisterAction(ControllerModelSpawner.EventType.OnTrackingInitialized, InitializeComponent);
            _controllerModelSpawner.RegisterAction(ControllerModelSpawner.EventType.OnFinishTrackingAcquired, InitializeComponent);
            _controllerModelSpawner.RegisterAction(ControllerModelSpawner.EventType.OnFinishTrackingLost, ReleaseComponent);
        }

        protected virtual void OnDestroy()
        {
            _controllerModelSpawner.UnregisterAction(ControllerModelSpawner.EventType.OnTrackingInitialized, InitializeComponent);
            _controllerModelSpawner.UnregisterAction(ControllerModelSpawner.EventType.OnFinishTrackingAcquired, InitializeComponent);
            _controllerModelSpawner.UnregisterAction(ControllerModelSpawner.EventType.OnFinishTrackingLost, ReleaseComponent);
        }
        
        #endregion

        #region Private Methods
        
        private void InitializeComponent(ControllerModelSpawner controllerModelSpawner)
        {
            PopulateLookupByMask();
            InternalOnFinishTrackingAcquired(controllerModelSpawner);
        }
        
        private void ReleaseComponent(ControllerModelSpawner controllerModelSpawner)
        {
            ClearLookup();
            InternalOnFinishTrackingLost(controllerModelSpawner);
        }
        
        private void PopulateLookupByMask()
        {
            Enum[] buttonTypes = _modelButtonMask.GetUniqueFlags().ToArray();
            foreach (Enum buttonType in buttonTypes)
            {
                if (_controllerModelSpawner.TryGetCurrentModelsLookupByType(buttonType, out ControllerElementServiceLocator controllerElementManager))
                {
                    T controllerElement = controllerElementManager.AddComponentOfType<T>();
                    InitializeElement(buttonType, controllerElement);
                    ElementLookup.TryAdd(buttonType, controllerElement);
                }
            }
        }
        
        private void ClearLookup()
        {
            foreach (var element in ElementLookup)
            {
                Destroy(element.Value);
            }
            
            ElementLookup.Clear();
        }
        
        #endregion

        #region Inheritance Methods

        protected abstract void InitializeElement(Enum buttonType, T element);
        protected virtual void InternalOnFinishTrackingAcquired(ControllerModelSpawner controllerModelSpawner) { }
        protected virtual void InternalOnFinishTrackingLost(ControllerModelSpawner controllerModelSpawner) { }

        #endregion

        #region Public Methods

        public void SetButtonMask(IEnumerable<Enum> buttonMask)
        {
            _modelButtonMask = buttonMask.Select(x => (ControllerModelButtonMask) Enum.Parse(typeof(ControllerModelButtonMask), x.ToString()))
                .Aggregate(ControllerModelButtonMask.None, (current, next) => current | next);
        }

        public void UpdateContainedElements()
        {
            ClearLookup();
            PopulateLookupByMask();
        }

        #endregion
    }
}
