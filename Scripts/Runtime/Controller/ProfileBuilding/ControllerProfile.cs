using CENTIS.XRPlatformManagement.Utilities;
using UnityEngine;

namespace CENTIS.XRPlatformManagement.Controller.ProfileBuilding
{
    /// <summary>
    /// Defines a Controller Profile with references to specific controller models for left and right hand.
    /// </summary>
    [CreateAssetMenu(fileName = "new ControllerProfile", menuName = "CENTIS/XRPlatformManagement/Profile", order = 1)]
    public class ControllerProfile : ScriptableObject
    {
        [SerializeField] private string manufacturerType;
        [SerializeField] private bool useParts;

        [SerializeField] private ControllerModel leftHand;
        [SerializeField] private ControllerModel rightHand;

        public string ManufacturerType => manufacturerType;
        public bool UseParts => useParts;
        public ControllerModel LeftHand => leftHand;
        public ControllerModel RightHand => rightHand;
    }
}
