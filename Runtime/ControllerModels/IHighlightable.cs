using UnityEngine;

namespace VRScripts.UI
{
    public interface IHighlightable
    {
        void OnHighlightElement(Material highlightMat);
        void OnUnhighlightElement();
    }
}
