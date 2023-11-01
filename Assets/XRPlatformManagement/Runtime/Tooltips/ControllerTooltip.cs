using UnityEngine;
using CENTIS.XRPlatformManagement.ControllerModels;

namespace CENTIS.XRPlatformManagement.Tooltips
{
    /// <summary>
    /// Holds information for a tooltip
    /// </summary>
    public class ControllerTooltip : MonoBehaviour
    {
        #region ################################# Unity Inspector Fields #################################
        
        [SerializeField] protected ControllerElement controllerElement;
        [SerializeField] protected ControllerTooltipVisualization visualization;
        [SerializeField] protected bool hideOnStart = true;
        
        #endregion

        #region ################################# Properties #################################
        
        public ControllerElement ControllerElementObject
        {
            get => controllerElement;
            set => controllerElement = value;
        }

        public bool IsVisible => _isVisible;

        #endregion

        #region ################################# Private and protected fields #################################
        
        protected bool _isVisible = true;
        private string _infoText = "";
        
        #endregion
        
        #region ################################# Unity lifecycle #################################
        
        protected void Start()
        {
            visualization.gameObject.SetActive(!hideOnStart);
        }
        
        #endregion

        #region ################################# Methods #################################
        
        /// <summary>
        /// Registers Highlight-Events to Tooltips
        /// </summary>
        public virtual void Initialize()
        {
            if (!controllerElement)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("No ControllerElement set!");
#endif
                return;
            }
            
            controllerElement.highlightStarted += Show;
            controllerElement.highlightStopped += Hide;
        }

        /// <summary>
        /// Unregistier Highlight-Events to Tooltips
        /// </summary>
        public virtual void Deinitialize()
        {
            if (!controllerElement)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("No ControllerElement set!");
#endif
                return;
            }
            
            controllerElement.highlightStarted -= Show;
            controllerElement.highlightStopped -= Hide;
        }


        /// <summary>
        /// Sets the tooltip text
        /// </summary>
        /// <param name="text">Tooltip text</param>
        public virtual void SetInfoText(string text)
        {
            _infoText = text;
        }
        
        /// <summary>
        /// Showing this element
        /// </summary>
        public void Show()
        {
            visualization.gameObject.SetActive(true);
            _isVisible = true;
        }

        /// <summary>
        /// Hide this element
        /// </summary>
        public void Hide()
        {
            visualization.gameObject.SetActive(false);
            _isVisible = false;
        }
        
        #endregion
    }
}