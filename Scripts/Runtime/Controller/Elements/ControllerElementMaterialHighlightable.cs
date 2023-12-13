using System.Collections.Generic;
using UnityEngine;

namespace CENTIS.XRPlatformManagement.Controller.Elements
{
    /// <summary>
    /// Class for highlight a single controller part with a material. It is part of the 'ControllerElementServiceLocator' component.
    /// </summary>
    public class ControllerElementMaterialHighlightable : BaseHighlightableComponent
    {
        private bool _isHighlighted;
        private Material _highlightMaterialReference;

        private readonly List<MaterialHighlightable> _materialHighlightable = new();

        public void Initialize(Material highlightMaterial, bool exchangeFirst)
        {
            _highlightMaterialReference = highlightMaterial;
            
            foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
            {
                _materialHighlightable.Add(new MaterialHighlightable(meshRenderer, _highlightMaterialReference, exchangeFirst));
            }
            
            _isHighlighted = false;
        }

        public override void HighlightElement()
        {
            if (_isHighlighted)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"Tried to highlight: {gameObject.name} which is already highlighted!!");
#endif
                return;
            }

            _isHighlighted = true;
            
            foreach (var highlightData in _materialHighlightable)
            {
                highlightData.Highlight();
            }
        }
        
        public override void UnhighlightElement()
        {
            if (!_isHighlighted)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"Tried to unhighlight: {gameObject.name} which is already unhighlighted!");
#endif
                return;
            }

            _isHighlighted = false;

            foreach (var highlightData in _materialHighlightable)
            {
                highlightData.Unhighlight(_highlightMaterialReference);
            }
        }
    }
}
