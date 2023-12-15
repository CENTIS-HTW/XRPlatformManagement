using System;
using System.Collections.Generic;
using CENTIS.XRPlatformManagement.Controller.Elements;

namespace CENTIS.XRPlatformManagement.Controller.Manager
{
    /// <summary>
    /// Class for accessing highlighting for the 'ControllerModelRender'. Use this class as an entry point for highlighting in your required way.
    /// </summary>
    public abstract class ControllerHighlighting<T> : BaseActivatableControllerElementRegistration<T> where T : BaseHighlightableComponent
    {
        public void SetHighlighting(bool shouldHighlight)
        {
            if(shouldHighlight) Activate();
            else Deactivate();
        }

        public void UpdateHighlighting(IEnumerable<Enum> buttonMask)
        {
            bool previouslyHighlighted = IsCurrentlyActive;
            if(previouslyHighlighted) Deactivate();

            SetButtonMask(buttonMask);
            UpdateContainedElements();
                
            if(previouslyHighlighted) Activate();
        }

        protected abstract override void InitializeElement(Enum buttonType, T element);

        protected override void ActivateElement(T element)
        {
            element.HighlightElement();
        }

        protected override void DeactivateElement(T element)
        {
            element.UnhighlightElement();
        }
    }
}