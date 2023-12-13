using System.Collections.Generic;
using UnityEngine;

namespace CENTIS.XRPlatformManagement
{
    public class MaterialHighlightable
    {
        private readonly MeshRenderer _renderer;
        private readonly Material _highlightMaterialReference;
        private readonly bool _exchangeFirst;
        private readonly List<Material> _currentMaterials;
        private Material _exchangedMaterial;

        public MaterialHighlightable(MeshRenderer renderer, Material highlightMaterialReference, bool exchangeFirst)
        {
            _renderer = renderer;
            _highlightMaterialReference = highlightMaterialReference;
            _exchangeFirst = exchangeFirst;
            _currentMaterials = new List<Material>();
        }
        
        public void Highlight()
        {
            _renderer.GetMaterials(_currentMaterials);

            if (_exchangeFirst)
            {
                _exchangedMaterial = _currentMaterials[0];
                _currentMaterials[0] = _highlightMaterialReference;
            }
            else
            {
                _currentMaterials.Add(_highlightMaterialReference);
            }

            _renderer.materials = _currentMaterials.ToArray();
        }

        public void Unhighlight(Material highlightMaterialReference)
        {
            if (_exchangedMaterial != null)
            {
                _currentMaterials[0] = _exchangedMaterial;
                _exchangedMaterial = null;
            }
            else
            {
                _currentMaterials.Remove(highlightMaterialReference);
            }
        
            _renderer.materials = _currentMaterials.ToArray();
        }
    }
}
