using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOrientationHandler : MonoBehaviour
{
    public bool ShouldForceOrientation;

    [Tooltip("Have this selected for forced landscape, unselected for forced portrait")]
    [SerializeField]
    public bool ForceLandscape;

    void Update()
    {
        if (ShouldForceOrientation)
        {
            if (ForceLandscape)
            {
                Screen.orientation = ScreenOrientation.Landscape;
            }
            else
            {
                Screen.orientation = ScreenOrientation.Portrait;
            }
        }
        else
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
        }

       
    }

}
