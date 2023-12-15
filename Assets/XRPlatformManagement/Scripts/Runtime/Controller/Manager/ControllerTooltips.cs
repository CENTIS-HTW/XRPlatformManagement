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
    /// Class manages and stores all single tooltips for Controller Buttons. Use this class as an entry point for showing tooltips in your required way.
    /// </summary>
    public class ControllerTooltips : BaseActivatableControllerElementRegistration<ControllerElementTooltip>
    {
        #region Unity Inspector Serialization

        [Header("Tooltip")]
        [SerializeField] private ControllerElementTooltipView _controllerElementTooltipViewPrefab;
        [SerializeField] private bool _isTextVisible = true;
        [SerializeField] private bool _isConnectionLineVisible = true;
        [Space]
        [SerializeField] private List<ButtonTextMap> _buttonTextMap;
        
        #endregion
        
        #region Inheritance Methods
        
        protected override void InitializeElement(Enum buttonType, ControllerElementTooltip element)
        {
            foreach (var buttonTextElement in _buttonTextMap)
            {
                Enum[] buttonTypes = buttonTextElement.ModelMask.GetUniqueFlags().ToArray();
                if (buttonTypes.Contains(buttonType))
                {
                    element.Initialize(_controllerElementTooltipViewPrefab, buttonTextElement.InstantiateAtObjectCenter, buttonTextElement.LocalPositionOverride, buttonTextElement.Text,
                        _isTextVisible, _isConnectionLineVisible, true);
                    return;
                }
            }
            
            element.Initialize(_controllerElementTooltipViewPrefab, true, Vector3.zero, $"Missing Text: {buttonType.ToString()}", 
                _isConnectionLineVisible, _isTextVisible, true);
        }

        protected override void ActivateElement(ControllerElementTooltip element)
        {
            element.Show();
        }

        protected override void DeactivateElement(ControllerElementTooltip element)
        {
            element.Hide();
        }
        
        #endregion

        [Serializable]
        private class ButtonTextMap
        {
            [FormerlySerializedAs("_modelButtonMask")] [SerializeField] private ControllerModelMask _modelMask;
            public ControllerModelMask ModelMask => _modelMask;
            
            [SerializeField] private bool _instantiateAtObjectCenter = true;
            public bool InstantiateAtObjectCenter => _instantiateAtObjectCenter;

            [SerializeField] private Vector3 _localPositionOverride;
            public Vector3 LocalPositionOverride => _localPositionOverride;
            
            [SerializeField] private string _text;
            public String Text => _text;
        }
    }
}