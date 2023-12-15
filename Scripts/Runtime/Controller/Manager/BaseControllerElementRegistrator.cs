using System;
using System.Collections.Generic;
using System.Linq;
using CENTIS.XRPlatformManagement.Controller.Elements;
using CENTIS.XRPlatformManagement.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace CENTIS.XRPlatformManagement.Controller.Manager
{
    /// <summary>
    /// Base class for adding a component to a desired controller element (based on the ControllerModelMask).
    /// </summary>
    /// <typeparam name="T">The element to add to the controller object.</typeparam>
    [RequireComponent(typeof(ControllerModelSpawner))]
    public abstract class BaseControllerElementRegistrator<T> : MonoBehaviour where T : Component
    {
        #region Fields
        
        [Header("Base")]
        [Tooltip("If unset, will be set on awake.")]
        [SerializeField] private ControllerModelSpawner _controllerModelSpawner;
        [SerializeField] private ControllerModelMask _modelMask;

        protected ControllerModelSpawner ControllerModelSpawner => _controllerModelSpawner;
        protected ControllerModelMask ModelMask => _modelMask;
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
            InternalOnAfterInitialize(controllerModelSpawner);
        }
        
        private void ReleaseComponent(ControllerModelSpawner controllerModelSpawner)
        {
            InternalOnBeforeRelease(controllerModelSpawner);
            ClearLookup();
        }
        
        private void PopulateLookupByMask()
        {
            Enum[] buttonTypes = _modelMask.GetUniqueFlags().ToArray();
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
        protected virtual void InternalOnAfterInitialize(ControllerModelSpawner controllerModelSpawner) { }
        protected virtual void InternalOnBeforeRelease(ControllerModelSpawner controllerModelSpawner) { }

        #endregion

        #region Public Methods

        public void SetButtonMask(IEnumerable<Enum> buttonMask)
        {
            _modelMask = buttonMask.Select(x => (ControllerModelMask) Enum.Parse(typeof(ControllerModelMask), x.ToString()))
                .Aggregate(ControllerModelMask.None, (current, next) => current | next);
        }

        public void UpdateContainedElements()
        {
            ClearLookup();
            PopulateLookupByMask();
        }

        #endregion
    }
}
