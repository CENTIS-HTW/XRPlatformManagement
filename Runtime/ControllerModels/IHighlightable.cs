using UnityEngine;

namespace CENTIS.XRPlatform.ControllerModels
{
    public interface IHighlightable
    {
        void OnHighlightElement(Material highlightMat);
        void OnUnhighlightElement();
    }
}
