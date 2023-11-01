using System.Collections;
using System.Collections.Generic;
using CENTIS.XRPlatformManagement.ControllerModels;
using UnityEngine;

namespace CENTIS.XRPlatformManagementManagement
{
    [CreateAssetMenu(fileName = "new ProfileSet", menuName = "CENTIS/XRPlatformManagement/ProfileSet")]
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
