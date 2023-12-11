using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

namespace CENTIS.XRPlatformManagement.SceneManagement
{
    /// <summary>
    /// This class checks the current running platform and if there are some Unity XR Features for VR/AR are activated.
    /// Depending on the determined platform its loading its predefined scene.
    /// </summary>
    public class SceneManagement : MonoBehaviour
    {
        [SerializeField] private SceneReference vrSceneToLoad;
        [SerializeField] private SceneReference desktopSceneToLoad;
        [SerializeField] private SceneReference arSceneToLoad;

        private void Awake()
        {
#if UNITY_EDITOR
                Debug.Log("Running Platform: Unity Editor");
#endif
            
#if UNITY_EDITOR_WIN
                Debug.Log("Running Platform: Unity Editor Windows");
                StartCoroutine(CheckVRSupport());
#endif
            
#if UNITY_STANDALONE_WINDOWS
                Debug.Log("Running Platform: Standalone Windows");
                if (checkVRAvailbility())
                {
                    SceneManager.LoadScene(vRScene);
                }
                else
                {
                    SceneManager.LoadScene(desktopScene);
                }
#endif
            
#if UNITY_ANDROID
                Debug.Log("Running Platform: ANDROID");
                StartCoroutine(CheckARSupport());
#endif

#if UNITY_IOS
                Debug.Log("Running Platform: IOS");
#endif
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CheckVRSupport()
        {
            // Devices like Valve Index need to be initialized first which might take some frames, so wait a short amount of time until its ready
            yield return new WaitForSeconds(0.1f);
            
            List<InputDevice> inputDevices = new List<InputDevice>();
            InputDevices.GetDevices(inputDevices);
            
            if ( InputDevices.GetDeviceAtXRNode(XRNode.Head).isValid)
            {
                foreach (InputDevice device in inputDevices)
                {
                    Debug.Log(string.Format($"Device found with name '{device.manufacturer}' and role '{device.characteristics}'"));
                }
                Debug.Log("Load VR-Scene...");
                SceneManager.LoadScene(vrSceneToLoad);
            }
            else
            {
                Debug.LogWarning("No VR-Headset available or XR-Plugin disabled, loading Desktop Scene...");
                SceneManager.LoadScene(desktopSceneToLoad);
            }
        }

        private IEnumerator CheckARSupport() {
            if (ARSession.state is ARSessionState.None or ARSessionState.CheckingAvailability)
            {
                yield return ARSession.CheckAvailability();
            }

            if (ARSession.state == ARSessionState.Unsupported)
            {
                // Start some fallback experience for unsupported devices
            }
            else
            {
                // Start the AR session
                SceneManager.LoadScene(arSceneToLoad);
            }
        }
    }
}
