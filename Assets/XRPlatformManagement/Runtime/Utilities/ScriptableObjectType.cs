using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CENTIS.XRPlatformManagement
{
    [CreateAssetMenu(fileName = "new Type", menuName = "CENTIS/XRPlatformManagement/Type")]
    public class ScriptableObjectType : ScriptableObject
    {
        public string type;
    }
}
