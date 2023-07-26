using System;
using System.Collections.Generic;
using System.Linq;
using CENTIS.XRPlatform.ControllerModels;
using CENTIS.XRPlatform.Utilities;
using UnityEngine;

namespace CENTIS.XRPlatform.Tooltips
{
    /// <summary>
    /// Class manages and stores all single tooltips for Controller Buttons
    /// </summary>
    [RequireComponent(typeof(ControllerButtonHighlighting))]
    public class ControllerTooltips : MonoBehaviour
    {
        #region ################################# Unity Inspector Values #################################
        
        [Tooltip("If unset, will be set on start.")]
        [SerializeField] protected ControllerButtonHighlighting buttonHighlighting;
        
        [Tooltip("Enable/Disable Tooltip within application")]
        [SerializeField] protected bool isEnabled = true;
        
        #endregion
        
        #region ################################# Properties #################################
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                
                // Hide tooltips on deactivation
                if (!isEnabled) Hide();
            }
        }
        
        public ControllerButtonHighlighting ButtonHighlighting => buttonHighlighting;

        #endregion

        #region ################################# Events #################################
        
        public event Action TooltipsShown;
        public event Action TooltipsHidden;
        
        #endregion
        
        #region ################################# Private Fields #################################
        
        private Dictionary<Enum, ControllerTooltip> _tooltipLookup = new();
        
        #endregion
        
        #region ################################# Unity Lifecycle #################################
        
        protected void Awake()
        {
            if (!buttonHighlighting) buttonHighlighting = GetComponent<ControllerButtonHighlighting>();
        }

        protected void OnEnable()
        {
            buttonHighlighting.HighlightStarted.AddListener(Show);
            buttonHighlighting.HighlightStopped.AddListener(Hide);
        }
        
        protected void OnDisable()
        {
            buttonHighlighting.HighlightStarted.RemoveListener(Show);
            buttonHighlighting.HighlightStopped.RemoveListener(Hide);
        }
        
        #endregion

        #region ################################# Methods #################################
        
        /// <summary>
        /// Shows all tooltips managed by this class
        /// </summary>
        public void Show()
        {
            if (!IsEnabled) return;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[XR-PM] GameObject: {gameObject.name} Tooltip shown!");
#endif
            
            Enum[] buttonValues = buttonHighlighting.ButtonMask.GetUniqueFlags().ToArray();

            foreach (Enum button in buttonValues)
            {
                if (!_tooltipLookup.ContainsKey(button)) continue;
                _tooltipLookup[button].Show();
            }
            
            TooltipsShown?.Invoke();
        }

        /// <summary>
        /// Hide all tooltips managed by this class
        /// </summary>
        public void Hide()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[XR-PM] GameObject: {gameObject.name} Tooltip Hide!");
#endif
            Enum[] buttonValues = buttonHighlighting.ButtonMask.GetUniqueFlags().ToArray();

            foreach (Enum button in buttonValues)
            {
                if (!_tooltipLookup.ContainsKey(button)) continue;
                _tooltipLookup[button].Hide();
            }
            
            TooltipsHidden?.Invoke();
        }

        /// <summary>
        /// Update all tooltips text
        /// </summary>
        /// <param name="newToolTips"></param>
        public void UpdateTooltips(IEnumerable<KeyValuePair<Enum, string>> newToolTips)
        {
            foreach ((Enum key, string value) in newToolTips)
            {
                _tooltipLookup[key].SetInfoText(value);
            }
        }

        /// <summary>
        /// Get List of tooltips
        /// ToDo: Maybe not give back the whole list and instead a copy?
        /// </summary>
        /// <returns>List of tooltips</returns>
        public IEnumerable<ControllerTooltip> GetAllControllerTooltips()
        {
            return _tooltipLookup.Values.ToList();
        }

        /// <summary>
        /// Adding a tooltip to this class
        /// </summary>
        /// <param name="element"></param>
        /// <param name="tooltip"></param>
        public void AddTooltipElement(Enum element, ControllerTooltip tooltip)
        {
            if (!_tooltipLookup.TryAdd(element, tooltip))
            {
                Debug.LogWarning($"tooltip for {element} already present!");
            }
        }
        
        #endregion
    }
}