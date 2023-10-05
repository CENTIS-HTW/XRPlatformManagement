using System.Collections;
using System.Collections.Generic;
using CENTIS.XRPlatform.ControllerModels;
using UnityEngine;

namespace CENTIS.XRPlatformManagement
{
    [CreateAssetMenu(fileName = "new ProfileSet", menuName = "INSPIRER/VR/Controller/ProfileSet")]
    public class ControllerProfileSet : ScriptableObject
    {
        [SerializeField] private List<ControllerProfile> _controllerProfiles;

        public ControllerProfile GetAt(int i)
        {
            return _controllerProfiles[i];
        }

        public int Count()
        {
            return _controllerProfiles.Count;
        }
    }
}
