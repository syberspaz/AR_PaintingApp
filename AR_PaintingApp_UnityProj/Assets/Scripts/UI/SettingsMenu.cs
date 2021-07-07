using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public float dragMovementSpeed;
    public float gyroMovementSpeed;
    public float pinchScaleSpeed;

    [SerializeField]
    private Slider dragSpeedSlider;

    [SerializeField]
    private Slider gyroSpeedSlider;

    [SerializeField]
    private Slider pinchScaleSlider;

    [SerializeField]
    private GameObject parentGO; //used to disabled and enable this

    public void Start()
    {
        dragMovementSpeed = PlayerPrefs.GetFloat("DragMovementSpeed");
        gyroMovementSpeed = PlayerPrefs.GetFloat("GyroMovementSpeed");
        pinchScaleSpeed = PlayerPrefs.GetFloat("PinchScaleSpeed");


        dragSpeedSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("DragMovementSpeed"));
        gyroSpeedSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("GyroMovementSpeed"));
        pinchScaleSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("PinchScaleSpeed"));


//        parentGO.SetActive(false);

        

    }

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
