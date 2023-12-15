using System;
using CENTIS.XRPlatformManagement.Controller.Elements;
using UnityEngine;

namespace CENTIS.XRPlatformManagement.Controller.Manager
{
    /// <summary>
    /// Handles the highlighting of the base class as a material
    /// </summary>
    public class ControllerMaterialHighlighting : ControllerHighlighting<ControllerElementMaterialHighlightable>
    {
        [Header("Highlight")]
        [SerializeField] private Material _highlightMaterial;
        [SerializeField] private bool _exchangeFirstMaterial;
        
        protected override void InitializeElement(Enum buttonType, ControllerElementMaterialHighlightable element)
        {
            element.Initialize(_highlightMaterial, _exchangeFirstMaterial);
        }
    }
}
