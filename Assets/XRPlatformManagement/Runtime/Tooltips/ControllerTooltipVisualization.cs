using UnityEngine;

namespace CENTIS.XRPlatformManagement.Tooltips
{
    /// <summary>
    /// Class holds components to visualize a tooltip
    /// </summary>
    public class ControllerTooltipVisualization : MonoBehaviour
    {
        #region ################################# Unity inspector fields #################################
        
        [SerializeField] protected LineRenderer line;
        [SerializeField] protected Transform label;
        
        #endregion
        
        #region ################################# Properties #################################
        
        public LineRenderer Line => line;
        public Transform Label => label;
        
        #endregion
        
        #region ################################# Methods #################################
        
        /// <summary>
        /// Sets the position where tool tip should visualize
        /// </summary>
        /// <param name="multiplier">Distance of object in %. 1 is same position</param>
        public virtual void SetVisualizationPosition(float multiplier)
        {
            if (line) line.SetPosition(1, line.GetPosition(1) * multiplier);
            if (label) label.localPosition *= multiplier;
        }
        
        /// <summary>
        /// Set visibility of connection line
        /// </summary>
        /// <param name="visibility"></param>
        public virtual void SetLineVisibility(bool visibility)
        {
            if (line) line.gameObject.SetActive(visibility);
        }
        
        /// <summary>
        /// Sets visibility of text label
        /// </summary>
        /// <param name="visibility"></param>
        public virtual void SetLabelVisibility(bool visibility)
        {
            if (label) label.gameObject.SetActive(visibility);
        }
        
        #endregion
    }
}