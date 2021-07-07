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

    public bool Enabled = false; //More to show than to switch

    [SerializeField]
    private Text toggleButtonText;

    private void Awake()
    {

        planeManager = GetComponent<ARPlaneManager>();

        SetAllPlannesActive(Enabled);

        toggleButtonText.text = "Disable";
    }

    public void PlaneDetectionToggle()
    {

        Enabled = !Enabled;


       SetAllPlannesActive(Enabled);
    
    }

    private void SetAllPlannesActive(bool value)
    {
        foreach(var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }


}
