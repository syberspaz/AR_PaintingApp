//This script was taken from:
//https://youtu.be/1Edsroa16fo

//just to toggle planes on & off

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class TogglePlaneDetection : MonoBehaviour
{
    private ARPlaneManager planeManager;

    [SerializeField]
    private Text toggleButtonText;

    private void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();

        toggleButtonText.text = "Disable";
    }

    public void PlaneDetectionToggle()
    {
        planeManager.enabled = !planeManager.enabled;
        string toggleButtonMessage = "";

        if (planeManager.enabled)
        {
            toggleButtonMessage = "Disable";
            SetAllPlannesActive(true);
        }
        else
        {
            toggleButtonMessage = "Enable";
            SetAllPlannesActive(false);
        }

        toggleButtonText.text = toggleButtonMessage;
    }

    private void SetAllPlannesActive(bool value)
    {
        foreach(var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }


}
