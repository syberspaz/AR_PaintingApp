using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceUserPlane : MonoBehaviour
{

    /*
    private bool startedPlacing = false;
    private bool progressFlagPlace = false;
    private bool progressFlagRotate = false;
    private bool progressFlagScale = false;

    private bool RotationSnapFlag = false;

    private bool canSnapRotate = true;
    */

    public bool LocationActive;
    public bool RotationActive;
    public bool ScaleActive;


    // Update is called once per frame
    void Update()
    {

        if (LocationActive)
        DoPlacementCalculations();
        else if (RotationActive)
        DoRotationCalculations();
        else if (ScaleActive) 
        DoScaleCalculations();
    }


    public void ResetPositionOnStart() //Button on pallette
    {
        transform.parent = Camera.main.transform;
        transform.localPosition = new Vector3(0.039f, -0.135f, 1.66f);
        transform.localRotation =  Quaternion.Euler(new Vector3(53.7f, 0f, 0f));
        transform.localScale = new Vector3(1f, 0.59f, 1f);
        transform.parent = null;
    }

    public void DoPlacementCalculations()
    {
        DetectTouchMovement.Calculate();

        float pinchAmount = 0f;

        if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
        { // zoom

            pinchAmount = DetectTouchMovement.pinchDistanceDelta;

            Matrix4x4 viewMatrix;

            viewMatrix = Matrix4x4.Inverse(Camera.main.transform.localToWorldMatrix);

            Vector3 forward;
            forward.x = viewMatrix.m20;
            forward.y = viewMatrix.m21;
            forward.z = viewMatrix.m22;

            transform.position += forward * pinchAmount / 2 * Time.deltaTime;
        }

        transform.parent = Camera.main.transform;
    }

    public void DoRotationCalculations()
    {
        Touch touch = Input.GetTouch(0);

        //test code for rotation
        Vector2 touchMovement = touch.deltaPosition;

        Quaternion rotation = Quaternion.Euler(touchMovement.y * 0.1f, 0, touchMovement.x * 0.1f);

        transform.rotation = rotation * transform.rotation;
    }

    public void DoScaleCalculations()
    {
        DetectTouchMovement.Calculate();

        float pinchAmount = 0f;

        if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
        { // zoom

            pinchAmount = DetectTouchMovement.pinchDistanceDelta;

            transform.localScale += (new Vector3(pinchAmount, pinchAmount, pinchAmount) / 3) * Time.deltaTime;
        }
    }

    public void SetForLocation()
    {
        if (LocationActive == false)
        {
            transform.parent = Camera.main.transform;
            LocationActive = true;
            RotationActive = false;
            ScaleActive = false;
        }
        else
        {
            transform.parent = null;
            LocationActive = false;
            RotationActive = false;
            ScaleActive = false;
        }
    }

    public void SetForRotation()
    {
        if (RotationActive == false)
        {
            LocationActive = false;
            RotationActive = true;
            ScaleActive = false;
        }
        else
        {
            LocationActive = false;
            RotationActive = false;
            ScaleActive = false;
        }
    }

    public void SetForScale()
    {
        if (ScaleActive == false)
        {
            LocationActive = false;
            RotationActive = false;
            ScaleActive = true;
        }
        else
        {
            LocationActive = false;
            RotationActive = false;
            ScaleActive = false;
        }
    }


}
