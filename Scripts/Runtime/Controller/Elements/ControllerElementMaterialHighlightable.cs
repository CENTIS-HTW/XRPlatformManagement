using System.Collections.Generic;
using UnityEngine;

namespace CENTIS.XRPlatformManagement.Controller.Elements
{
    /// <summary>
    /// Class for highlight a single controller part with a material. It is part of the 'ControllerElementServiceLocator' component.
    /// </summary>
    public class ControllerElementMaterialHighlightable : BaseHighlightableComponent
    {
        protected MeshRenderer Renderer;
        protected bool IsHighlighted;
        protected bool ExchangeFirst;

        private Material _highlightMaterialReference;
        private Material _exchangedMaterial;
        private readonly List<Material> _currentMaterials = new();

        public void Initialize(Material highlightMaterial, bool exchangeFirst)
        {
            _highlightMaterialReference = highlightMaterial;
            Renderer = GetComponentInChildren<MeshRenderer>();
            IsHighlighted = false;
            ExchangeFirst = exchangeFirst;
        }

        public override void HighlightElement()
        {
            if (IsHighlighted)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"Tried to highlight: {gameObject.name} which is already highlighted!!");
#endif
                return;
            }

            IsHighlighted = true;


            Renderer.GetMaterials(_currentMaterials);

            if (ExchangeFirst)
            {
                _exchangedMaterial = _currentMaterials[0];
                _currentMaterials[0] = _highlightMaterialReference;
            }
            else
            {
                _currentMaterials.Add(_highlightMaterialReference);
            }

            Renderer.materials = _currentMaterials.ToArray();
        }
        
        public override void UnhighlightElement()
        {
            if (!IsHighlighted)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"Tried to unhighlight: {gameObject.name} which is already unhighlighted!");
#endif
                return;
            }

            IsHighlighted = false;

            if (_exchangedMaterial != null)
            {
                _currentMaterials[0] = _exchangedMaterial;
                _exchangedMaterial = null;
            }
            else
            {
                _currentMaterials.Remove(_highlightMaterialReference);
            }
            
            Renderer.materials = _currentMaterials.ToArray();
        }
    }
}
