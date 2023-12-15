using CENTIS.XRPlatformManagement.Controller.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CENTIS.XRPlatformManagement.UsageSample
{
    /// <summary>
    /// Just Activate/Deactivate the inherited activatable part.
    /// </summary>
    public class InputControllerHighlighting : ControllerMaterialHighlighting
    {
        #region Fields
    
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
            Activate();
        }

        private void OnDisableInteraction(InputAction.CallbackContext context)
        {
            Deactivate();
        }
    
        #endregion
    }
}
