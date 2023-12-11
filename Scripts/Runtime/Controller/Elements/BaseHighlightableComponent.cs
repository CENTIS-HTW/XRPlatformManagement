using UnityEngine;

namespace CENTIS.XRPlatformManagement.Controller.Elements
{
    public abstract class BaseHighlightableComponent : MonoBehaviour, IHighlightable
    {
        public abstract void HighlightElement();
        public abstract void UnhighlightElement();
    }
}
