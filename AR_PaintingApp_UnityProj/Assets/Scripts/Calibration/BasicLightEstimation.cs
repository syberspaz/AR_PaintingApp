using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class BasicLightEstimation : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager arCameraManager;

    [SerializeField]
    private Text colorValue;

    private Light currentLight;

    // Start is called before the first frame update
    private void Start()
    {
        currentLight = GetComponent<Light>();
    }

    private void OnEnable()
    {
        arCameraManager.frameReceived += FrameChanged;
    }

    private void OnDisable()
    {
        arCameraManager.frameReceived -= FrameChanged;
    }

    private void FrameChanged(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.mainLightColor != null)
        {
            colorValue.text = "Main Color: R: " + args.lightEstimation.mainLightColor.Value.r + " G: " + args.lightEstimation.mainLightColor.Value.g + " B: " + args.lightEstimation.mainLightColor.Value.b;
            currentLight.color = args.lightEstimation.mainLightColor.Value;
        }
    }

    public float Map(float value, float min1, float max1, float min2, float max2)
    {
        return min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
    }
}
