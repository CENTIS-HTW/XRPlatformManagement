using System;
using CENTIS.XRPlatformManagement.Controller.Manager;
using UnityEngine;

namespace CENTIS.XRPlatformManagement.Controller.ProfileBuilding
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

        public GameObject GetModelByMask(ControllerModelButtonMask controllerModelButtonMask)
        {
            switch (controllerModelButtonMask)
            {
                case ControllerModelButtonMask.None:
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                    Debug.LogWarning($"Can't return a model with {ControllerModelButtonMask.None}!");
#endif
                    return null;
                case ControllerModelButtonMask.CompleteModel:
                    return completeModel;
                case ControllerModelButtonMask.Body:
                    return body;
                case ControllerModelButtonMask.PrimaryButton:
                    return primaryButton;
                case ControllerModelButtonMask.SecondaryButton:
                    return secondaryButton;
                case ControllerModelButtonMask.Trigger:
                    return trigger;
                case ControllerModelButtonMask.SystemButton:
                    return systemButton;
                case ControllerModelButtonMask.ThumbStick:
                    return thumbStick;
                case ControllerModelButtonMask.Trackpad:
                    return trackpad;
                case ControllerModelButtonMask.StatusLED:
                    return statusLed;
                case ControllerModelButtonMask.GripButtonPrimary:
                    return gripButtonPrimary;
                case ControllerModelButtonMask.GripButtonSecondary:
                    return gripButtonSecondary;
                default:
                    throw new ArgumentOutOfRangeException(nameof(controllerModelButtonMask), controllerModelButtonMask, null);
            }
        }
    }
}
