using CENTIS.XRPlatformManagement.Utilities;
using UnityEngine;

namespace CENTIS.XRPlatformManagement.ControllerModels
{
    /// <summary>
    /// Defines a Controller Profile with references to specific controller models for left and right hand.
    /// </summary>
    [CreateAssetMenu(fileName = "new ControllerProfile", menuName = "CENTIS/XRPlatformManagement/Profile", order = 1)]
    public class ControllerProfile : ScriptableObject
    {
        [SerializeField] private ManufacturerType manufacturerType;
        [SerializeField] private bool useParts;

        [SerializeField] private Vector3 penOffsetPosition;
        [SerializeField] private Vector3 penOffsetRotation;

        [SerializeField] private Vector3 center = Vector3.zero;

        [SerializeField] private ControllerModel leftHand;
        [SerializeField] private ControllerModel rightHand;

        public ManufacturerType ManufacturerType => manufacturerType;

        public bool UseParts => useParts;

        public Vector3 PenOffsetPosition => penOffsetPosition;

        public Vector3 PenOffsetRotation => penOffsetRotation;

        public Vector3 Center => center;

        public ControllerModel LeftHand => leftHand;

        public ControllerModel RightHand => rightHand;
    }
}
