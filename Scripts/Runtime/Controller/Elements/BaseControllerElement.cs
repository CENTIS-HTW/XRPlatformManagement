using CENTIS.XRPlatformManagement.Controller.Manager;
using UnityEngine;

namespace CENTIS.XRPlatformManagement.Controller.Elements
{
    /// <summary>
    /// A basic implementation, every 'Controller Element' can derive from.
    /// Used to setup all data, that is available for a 'Controller Element'.
    /// It is also responsible for preparing a method, that is available as soon as the data was setup.
    /// </summary>
    [RequireComponent(typeof(ControllerElementServiceLocator))]
    public class BaseControllerElement : MonoBehaviour, IControllerElement
    {
        protected ControllerModelSpawner ControllerModelSpawner;
        protected ControllerElementServiceLocator ControllerElementServiceLocator;
        protected bool IsInitialized;
        
        public virtual void InitializeControllerElement(ControllerModelSpawner controllerModelSpawner, ControllerElementServiceLocator controllerElementServiceLocator)
        {
            ControllerModelSpawner = controllerModelSpawner;
            ControllerElementServiceLocator = controllerElementServiceLocator;
            IsInitialized = true;
            OnControllerInitialized();
        }
        
        /// <summary>
        /// Called as soon as all data is prepared.
        /// </summary>
        protected virtual void OnControllerInitialized() { }

        protected virtual void OnDestroy()
        {
            if (IsInitialized)
            {
                OnControllerDestroying();
                IsInitialized = false;
            }
        }

        /// <summary>
        /// Used to respond to the destruction of an 'Controller Element', if it was initialized.
        /// </summary>
        protected virtual void OnControllerDestroying() { }
    }
}
