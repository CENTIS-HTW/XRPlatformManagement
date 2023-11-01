using UnityEngine;

namespace CENTIS.XRPlatformManagement.ControllerModels
{
    public interface IHighlightable
    {
        void OnHighlightElement(Material highlightMat);
        void OnUnhighlightElement();
    }
}
