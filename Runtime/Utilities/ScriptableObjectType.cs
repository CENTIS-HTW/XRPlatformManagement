using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CENTIS.XRPlatformManagement
{
    [CreateAssetMenu(fileName = "new ScriptableObjectType", menuName = "CENTIS/XRPlatformManagement/ScriptableObjectType")]
    public class ScriptableObjectType : ScriptableObject
    {
        public string type;
    }
}
