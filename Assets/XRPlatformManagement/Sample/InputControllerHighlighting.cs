using System;
using CENTIS.XRPlatformManagement.Controller.Elements;
using CENTIS.XRPlatformManagement.Controller.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VENTUS.PlaformPackageExtension
{
    public class InputControllerHighlighting : ControllerButtonHighlighting<ControllerElementMaterialHighlightable>
    {
        #region Fields
    
        [Header("Highlight")]
        [SerializeField] private Material _highlightMaterial;
        [SerializeField] private bool _exchangeFirstMaterial;
        [SerializeField] private InputActionProperty _interactionInput;
    
        #endregion
    
        #region Unity Lifecycle
    
        protected override void Awake()
        {
            base.Awake();
            _interactionInput.action.started += OnEnableInteraction;
            _interactionInput.action.canceled += OnDisableInteraction;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _interactionInput.action.started -= OnEnableInteraction;
            _interactionInput.action.canceled -= OnDisableInteraction;
        }
    
        #endregion

        #region Private Methods
    
        private void OnEnableInteraction(InputAction.CallbackContext context)
        {
            Debug.Log("Activate");
            Activate();
        }

        private void OnDisableInteraction(InputAction.CallbackContext context)
        {
            Debug.Log("Deactivate");
            Deactivate();
        }
    
        #endregion
    
        #region Inheritance
    
        protected override void InitializeElement(Enum buttonType, ControllerElementMaterialHighlightable element)
        {
            element.Initialize(_highlightMaterial, _exchangeFirstMaterial);
        }
    
        #endregion
    }
}
