using System;
using UnityEngine;

namespace CENTIS.XRPlatformManagement.ControllerModels
{
    public class ControllerElement : MonoBehaviour, IHighlightable
    {
        public event Action highlightStarted;
        public event Action highlightStopped;
        
        public MeshRenderer Renderer { get; private set; }
        public Enum ControllerElementName { get; set; }
        
        public bool IsHighlighted { get; private set; } = false;

        // private ElementTooltip _tooltip = null;
        // private ElementTooltip Tooltip
        // {
        //     get
        //     {
        //         if (_tooltip is null)
        //             _tooltip = GetComponentInChildren<ElementTooltip>(true);
        //         if (_tooltip is null)
        //             Debug.LogError("Can not find tooltip object!", gameObject);
        //         return _tooltip;
        //     }
        // }

        // private bool showsInfo = false;

        private void Awake()
        {
            if(!Renderer) Renderer = GetComponentInChildren<MeshRenderer>();
        }

        public virtual void OnHighlightElement(Material highlightMat)
        {
            if (IsHighlighted) return;

            Material[] oldMaterials = Renderer.materials;

            Material[] materials = new Material[oldMaterials.Length + 1];

            for (int index = 0; index < oldMaterials.Length; index++)
            {
                materials[index] = oldMaterials[index];
            }
            materials[^1] = highlightMat;
            Renderer.materials = materials;
            
            highlightStarted?.Invoke();
            IsHighlighted = true;
        }



        public virtual void OnUnhighlightElement()
        {
            if (!IsHighlighted) return;

            Material[] oldMaterials = Renderer.materials;

            // This is a bit hacky, when there is only 1 material assigned it would be removed and let it left without any material
            if (oldMaterials.Length < 2) return;

            Material[] materials = new Material[oldMaterials.Length - 1];

            for (int index = 0; index < materials.Length; index++)
            {
                materials[index] = oldMaterials[index];
            }
            Renderer.materials = materials;

            highlightStopped?.Invoke();
            IsHighlighted = false;
        }
        
        
        // public void ShowInfo(string info)
        // {
        //     if (showsInfo || info.Equals("none"))
        //     {
        //         HideInfo();
        //         return;
        //     }
        //
        //     // Tooltip?.Show(info);
        //     showsInfo = true;
        // }
        //
        // public void HideInfo()
        // {
        //     if (!showsInfo) return;
        //
        //     // Tooltip?.Hide();
        //     showsInfo = false;
        //     
        // }
    }
}
