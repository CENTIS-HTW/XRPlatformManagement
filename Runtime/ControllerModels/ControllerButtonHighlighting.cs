using System;
using System.Collections.Generic;
using System.Linq;
using CENTIS.XRPlatform.Utilities;
using UnityEngine;
using UnityEngine.Events;
using VRScripts.UI.Device;

namespace CENTIS.XRPlatform.ControllerModels
{
    [RequireComponent(typeof(ControllerModelRender))]
    public class ControllerButtonHighlighting : MonoBehaviour
    {
        [SerializeField] private ControllerModelRender modelRenderer;

        [SerializeField] private Material highlightMaterial;
        [SerializeField] private ControllerButtonMask buttonMask;

        [SerializeField] private UnityEvent highlightStarted;
        [SerializeField] private UnityEvent highlightStopped;

        public event Action highlightButtonMaskUpdated;
        public ControllerModelRender ModelRenderer => modelRenderer;
        public ControllerButtonMask ButtonMask => buttonMask;

        public UnityEvent HighlightStarted => highlightStarted;

        public UnityEvent HighlightStopped => highlightStopped;

        private bool _isHighlighted = false;
        private void Awake()
        {
            if (modelRenderer == null)
            {
                modelRenderer = gameObject.GetComponent<ControllerModelRender>();
            }
        }

        public void SetButtonsToHighlight(IEnumerable<Enum> buttonMask)
        {
            this.buttonMask = buttonMask.Select(x => (ControllerButtonMask) Enum.Parse(typeof(ControllerButtonMask), x.ToString()))
                .Aggregate(ControllerButtonMask.None, (current, next) => current | next);
        }

        public void Highlighting(bool shouldHighlight)
        {
            if(shouldHighlight) StartHighlight();
            else StopHighlight();
        }

        public void UpdateHighlighting(IEnumerable<Enum> buttonMask)
        {
            bool previouslyHighlighted = _isHighlighted;
            if(previouslyHighlighted) StopHighlight();
            
            SetButtonsToHighlight(buttonMask);
            
            if(previouslyHighlighted) StartHighlight();
            
            highlightButtonMaskUpdated?.Invoke();
        }

        public void StartHighlight()
        {
            Enum[] buttonValues = buttonMask.GetUniqueFlags().ToArray();

            foreach (Enum button in buttonValues)
            {
                if (!modelRenderer.Objects.ContainsKey(button)) continue;
                modelRenderer.Objects[button].OnHighlightElement(highlightMaterial);
            }

            _isHighlighted = true;
            
            HighlightStarted?.Invoke();
        }

        public void StopHighlight()
        {
            Enum[] buttonValues = buttonMask.GetUniqueFlags().ToArray();
            foreach (Enum button in buttonValues)
            {
                if (!modelRenderer.Objects.ContainsKey(button)) continue;
                modelRenderer.Objects[button].OnUnhighlightElement();
            }

            _isHighlighted = false;
            
            HighlightStopped?.Invoke();
        }
    }
}