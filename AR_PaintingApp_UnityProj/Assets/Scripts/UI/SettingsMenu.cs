using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public float dragMovementSpeed;
    public float gyroMovementSpeed;
    public float pinchScaleSpeed;

    public void Update()
    {
        PlayerPrefs.SetFloat("DragMovementSpeed", dragMovementSpeed);
        PlayerPrefs.SetFloat("GyroMovementSpeed", gyroMovementSpeed);
        PlayerPrefs.SetFloat("PinchScaleSpeed", pinchScaleSpeed);
    }

    public void UpdateDragMovementSpeed(float sliderIn)
    {
        dragMovementSpeed = sliderIn;
    }

    public void UpdateGyroMovementSpeed(float sliderIn)
    {
        gyroMovementSpeed = sliderIn;
    }

    public void UpdatePinchScaleSpeed(float sliderIn)
    {
        pinchScaleSpeed = sliderIn;
    }
}
