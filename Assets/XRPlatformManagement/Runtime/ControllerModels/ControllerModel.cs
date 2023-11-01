using System;
using System.Collections.Generic;
using UnityEngine;

namespace CENTIS.XRPlatformManagement.ControllerModels
{
    /// <summary>
    /// Defines a Controller Render Model.
    /// </summary>
    [CreateAssetMenu(fileName = "ControllerModel", menuName = "CENTIS/XRPlatformManagement/RenderModel", order = 2)]
    public class ControllerModel : ScriptableObject
    {
        [SerializeField] private GameObject completeModel;
        [SerializeField] private GameObject body;
        [SerializeField] private GameObject primaryButton;
        [SerializeField] private GameObject secondaryButton;
        [SerializeField] private GameObject trigger;
        [SerializeField] private GameObject systemButton;
        [SerializeField] private GameObject thumbStick;
        [SerializeField] private GameObject trackpad;
        [SerializeField] private GameObject statusLed;
        [SerializeField] private GameObject gripButtonPrimary;
        [SerializeField] private GameObject gripButtonSecondary;

        public GameObject GetModelByMask(ControllerButtonMask controllerButtonMask)
        {
            switch (controllerButtonMask)
            {
                case ControllerButtonMask.None:
                    Debug.LogWarning($"Can't return a model with {ControllerButtonMask.None}!");
                    return null;
                case ControllerButtonMask.CompleteModel:
                    return completeModel;
                case ControllerButtonMask.Body:
                    return body;
                case ControllerButtonMask.PrimaryButton:
                    return primaryButton;
                case ControllerButtonMask.SecondaryButton:
                    return secondaryButton;
                case ControllerButtonMask.Trigger:
                    return trigger;
                case ControllerButtonMask.SystemButton:
                    return systemButton;
                case ControllerButtonMask.ThumbStick:
                    return thumbStick;
                case ControllerButtonMask.Trackpad:
                    return trackpad;
                case ControllerButtonMask.StatusLED:
                    return statusLed;
                case ControllerButtonMask.GripButtonPrimary:
                    return gripButtonPrimary;
                case ControllerButtonMask.GripButtonSecondary:
                    return gripButtonSecondary;
                default:
                    throw new ArgumentOutOfRangeException(nameof(controllerButtonMask), controllerButtonMask, null);
            }
        }
    }
}
