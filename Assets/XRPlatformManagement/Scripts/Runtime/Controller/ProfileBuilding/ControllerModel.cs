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

        public GameObject GetModelByMask(ControllerModelMask controllerModelMask)
        {
            switch (controllerModelMask)
            {
                case ControllerModelMask.None:
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                    Debug.LogWarning($"Can't return a model with {ControllerModelMask.None}!");
#endif
                    return null;
                case ControllerModelMask.CompleteModel:
                    return completeModel;
                case ControllerModelMask.Body:
                    return body;
                case ControllerModelMask.PrimaryButton:
                    return primaryButton;
                case ControllerModelMask.SecondaryButton:
                    return secondaryButton;
                case ControllerModelMask.Trigger:
                    return trigger;
                case ControllerModelMask.SystemButton:
                    return systemButton;
                case ControllerModelMask.ThumbStick:
                    return thumbStick;
                case ControllerModelMask.Trackpad:
                    return trackpad;
                case ControllerModelMask.StatusLED:
                    return statusLed;
                case ControllerModelMask.GripButtonPrimary:
                    return gripButtonPrimary;
                case ControllerModelMask.GripButtonSecondary:
                    return gripButtonSecondary;
                default:
                    throw new ArgumentOutOfRangeException(nameof(controllerModelMask), controllerModelMask, null);
            }
        }
    }
}
