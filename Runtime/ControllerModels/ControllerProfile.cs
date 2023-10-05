using CENTIS.XRPlatformManagement;
using UnityEngine;

namespace CENTIS.XRPlatform.ControllerModels
{
    /// <summary>
    /// Defines a Controller Profile with references to specific controller models for left and right hand.
    /// </summary>
    [CreateAssetMenu(fileName = "ControllerProfile", menuName = "INSPIRER/VR/Controller/Profile", order = 1)]
    public class ControllerProfile : ScriptableObject
    {
        [SerializeField] private ScriptableObjectType manufacturerType;
        [SerializeField] private bool useParts;

        [SerializeField] private Vector3 penOffsetPosition;
        [SerializeField] private Vector3 penOffsetRotation;

        [SerializeField] private Vector3 center = Vector3.zero;

        [SerializeField] private ControllerModel leftHand;
        [SerializeField] private ControllerModel rightHand;

        public ScriptableObjectType ManufacturerType => manufacturerType;

        public bool UseParts => useParts;

        public Vector3 PenOffsetPosition => penOffsetPosition;

        public Vector3 PenOffsetRotation => penOffsetRotation;

        public Vector3 Center => center;

        public ControllerModel LeftHand => leftHand;

        public ControllerModel RightHand => rightHand;
    }
}
