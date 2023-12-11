using TMPro;
using UnityEngine;

namespace CENTIS.XRPlatformManagement.Controller.Elements
{
    /// <summary>
    /// Class holds components to visualize a tooltip and change it at runtime
    /// </summary>
    public class ControllerElementTooltipView : MonoBehaviour
    {
        #region Unity Inspector Serialization
        
        [SerializeField] protected TMP_Text tmpText;
        [SerializeField] protected LineRenderer _line;
        [SerializeField] protected int _lineSubVerticesCount;
        
        #endregion
        
        #region Private Methods

        protected Transform ControllerElementOrigin;
        protected Vector3 MeshCenterLocalPosition;
        private bool _updateLinePosition;
        
        #endregion

        private void Update()
        {
            if (!_updateLinePosition) return;
            
            Vector3 originPosition = ControllerElementOrigin.TransformPoint(MeshCenterLocalPosition);
            Vector3[] positions = new Vector3[_lineSubVerticesCount + 1];
            for (int i = 0; i <= _lineSubVerticesCount; i++)
            {
                positions[i] = Vector3.Lerp(originPosition, transform.position, (float)i / _lineSubVerticesCount);
            }

            _line.positionCount = _lineSubVerticesCount;
            _line.SetPositions(positions);
        }

        #region Public Methods
        
        /// <summary>
        /// Will change the origin position for the 'LineRenderer'
        /// </summary>
        public virtual void SetPosition(Transform controllerElementOrigin, Vector3 meshCenterLocalPosition, Vector3 localPositionOverride)
        {
            transform.localPosition = meshCenterLocalPosition + localPositionOverride;
            
            ControllerElementOrigin = controllerElementOrigin;
            MeshCenterLocalPosition = meshCenterLocalPosition;

            if (!_line) return;
            
            _line.useWorldSpace = true;
            _updateLinePosition = true;
        }

        public virtual void SetText(string text)
        {
            tmpText.text = text;
        }
        
        public virtual void SetConnectionLineVisibility(bool visibility)
        {
            if (_line) _line.gameObject.SetActive(visibility);
        }
        
        public virtual void SetTextVisibility(bool visibility)
        {
            if (tmpText) tmpText.gameObject.SetActive(visibility);
        }
        
        #endregion
    }
}