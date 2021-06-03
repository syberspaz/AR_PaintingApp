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

    private bool CalipersEnabled;

    private bool ColorToolsEnabled;

    private bool PerspectiveLinesEnabled;

    [SerializeField]
    private GameObject caliperUIGameObject;

    [SerializeField]
    private GameObject colorToolUIGameObject;

    [SerializeField]
    private GameObject LineToolUIGameObject;

    //Just a lot of small functions to control the UI in the explore mode scene
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

    public void ToggleCalipers()
    {
        CalipersEnabled = !CalipersEnabled;
        caliperUIGameObject.SetActive(CalipersEnabled);
        if (CalipersEnabled)
        {
           
            EnableCalipers();
        }
        else
        {
            DisableCalipers();
        }
    }


    public void ToggleColorTools()
    {
        ColorToolsEnabled = !ColorToolsEnabled;
        colorToolUIGameObject.SetActive(ColorToolsEnabled);
    }

    public void TogglePerspectiveLines()
    {
        PerspectiveLinesEnabled = !PerspectiveLinesEnabled;
        LineToolUIGameObject.SetActive(PerspectiveLinesEnabled);
    }

}
