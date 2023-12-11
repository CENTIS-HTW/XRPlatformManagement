using System;
using CENTIS.XRPlatformManagement.Controller.Manager;
using UnityEngine;

namespace CENTIS.XRPlatformManagement.Controller.Elements
{
    /// <summary>
    /// Component, that every controller must have attached. It acts like a service locator. For this service locator a service is every class, that implements interface 'IController Element'.
    /// It is also responsible for initializing every 'Controller Element'.
    /// </summary>
    public sealed class ControllerElementServiceLocator : MonoBehaviour
    {
        public Enum ControllerElementType { get; private set; }
        public ControllerModelSpawner ControllerModelSpawner { get; private set; }
        
        
        private IControllerElement[] _controllerElements;

        /// <summary>
        /// Used by the 'ControllerModelRender' to initialize 'Controller Elements'
        /// </summary>
        public void Initialize(ControllerModelSpawner controllerModelSpawner, Enum controllerElementType)
        {
            ControllerModelSpawner = controllerModelSpawner;
            ControllerElementType = controllerElementType;
            
            UpdateControllerElements();

            foreach (var controllerElement in _controllerElements)
            {
                controllerElement.InitializeControllerElement(ControllerModelSpawner, this);
            }
        }

        /// <summary>
        /// Needed to be called, in order to update the registered 'Controller Elements', which are attached to this transform or its children.
        /// Call this after adding/removing a 'Controller Element'!
        /// </summary>
        public void UpdateControllerElements()
        {
            _controllerElements = GetComponentsInChildren<IControllerElement>();
        }

        /// <summary>
        /// Entry point for the Service Provider: Call this method for accessing 'Controller Elements'.
        /// </summary>
        /// <param name="element">Returns the first found element</param>
        /// <typeparam name="T">Thy type to search for. This provides the option to search for every type, as long as that type is part of the 'Controller Element'. </typeparam>
        /// <returns>If the requested Type was found. If your components was not found, you may need to call 'UpdateControllerElements'.</returns>
        public bool TryGetFirstControllerElementOfType<T>(out T element) where T : IControllerElement
        {
            element = default;
            
            foreach (var controllerElement in _controllerElements)
            {
                if (controllerElement is not T result) continue;
                
                element = result;
                return true;
            }
            
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning($"Couldn't find the requested element of type {typeof(T)}!");
#endif
            
            return false;
        }

        public T AddComponentOfType<T>() where T : Component
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (gameObject.GetComponent<T>())
            {
                Debug.LogWarning($"There is already an object with type {typeof(T)} registered!");
            }
#endif
            
            return gameObject.AddComponent<T>();
        }
    }
}
