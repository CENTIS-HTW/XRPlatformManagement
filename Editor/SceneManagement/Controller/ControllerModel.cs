using UnityEngine;

namespace CENTIS.XRPlatform.Editor.SceneManagement.Controller
{
    /// <summary>
    /// Defines a Controller Render Model.
    /// </summary>
    [CreateAssetMenu(fileName = "ControllerModel", menuName = "INSPIRER/VR/Controller/RenderModel", order = 2)]
    public class ControllerModel : ScriptableObject
    {
        [SerializeField] private GameObject completeModel;
        [SerializeField] private GameObject body;
        [SerializeField] private GameObject primaryButton;
        [SerializeField] private GameObject secondaryButton;
        [SerializeField] private GameObject trigger;
        [SerializeField] private GameObject systemButton;
        [SerializeField] private GameObject thumbStick;
        [SerializeField] private GameObject trackpad;
        [SerializeField] private GameObject statusLed;
        [SerializeField] private GameObject gripButtonPrimary;
        [SerializeField] private GameObject gripButtonSecondary;
        
        public GameObject CompleteModel => completeModel;

        public GameObject Body => body;

        public GameObject PrimaryButton => primaryButton;

        public GameObject SecondaryButton => secondaryButton;
        
        public GameObject Trigger => trigger;

        public GameObject SystemButton => systemButton;

        public GameObject ThumbStick => thumbStick;

        public GameObject Trackpad => trackpad;

        public GameObject StatusLed => statusLed;

        public GameObject GripButtonPrimary => gripButtonPrimary;
        
        public GameObject GripButtonSecondary => gripButtonSecondary;
    }
}
