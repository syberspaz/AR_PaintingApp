using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
public class ExploremodeUI : MonoBehaviour
{
    [SerializeField]
    private ARSessionOrigin arSession;

    private MeasurementController calipers;

    public void EnableCalipers()
    {
        calipers = arSession.GetComponent<MeasurementController>();
        calipers.enabled = true;
    }
    public void DisableCalipers()
    {
        calipers = arSession.GetComponent<MeasurementController>();
        calipers.enabled = false;
    }

    public void SwitchToMainMenu()
    {
        SceneManager.LoadScene(0);//main menu is scene 0
    }

}
