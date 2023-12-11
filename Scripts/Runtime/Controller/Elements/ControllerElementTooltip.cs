using UnityEngine;

namespace CENTIS.XRPlatformManagement.Controller.Elements
{
    /// <summary>
    /// Used for accessing, showing and hiding the 'ControllerTooltipVisualization' Component
    /// </summary>
    public class ControllerElementTooltip : BaseControllerElement
    {
        #region Fields
        
        private ControllerElementTooltipView _viewPrefab;

        public bool IsVisible => _viewPrefab.isActiveAndEnabled;
        public ControllerElementTooltipView ViewPrefab => _viewPrefab;

        #endregion

        #region Public Methods

        public void Initialize(ControllerElementTooltipView viewPrefab, bool instantiateAtObjectCenter, Vector3 localPositionOverride, string text, 
            bool isConnectionLineVisible, bool isTextVisible, bool hideOnInitialize)
        {
            _viewPrefab = Instantiate(viewPrefab, transform, false);

            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            Vector3 meshCenterLocalPosition = Vector3.zero;
            if (instantiateAtObjectCenter && meshFilters is { Length: > 0 })
            {
                meshCenterLocalPosition = CalculateCenterPosition(meshFilters);
            }
            else
            {
                Debug.LogWarning($"There was no {typeof(MeshFilter)} found on {gameObject.name}");
            }
            
            _viewPrefab.SetPosition(transform, meshCenterLocalPosition, localPositionOverride);
            _viewPrefab.gameObject.SetActive(!hideOnInitialize);
            _viewPrefab.SetText(text);
            _viewPrefab.SetConnectionLineVisibility(isConnectionLineVisible);
            _viewPrefab.SetConnectionLineVisibility(isTextVisible);
        }
        
        public void Show()
        {
            _viewPrefab.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _viewPrefab.gameObject.SetActive(false);
        }
        
        #endregion
        
        #region Private Methods
        
        private Vector3 CalculateCenterPosition(MeshFilter[] meshFilters)
        {
            Vector3 center = Vector3.zero;
            float totalVertices = 0;
            
            foreach (var meshFilter in meshFilters)
            {
                totalVertices += meshFilter.mesh.vertexCount;
                foreach (Vector3 vertex in meshFilter.mesh.vertices)
                {
                    center += vertex;
                }
            }
            
            center /= totalVertices;
            return center;
        }
        
        #endregion
    }
}