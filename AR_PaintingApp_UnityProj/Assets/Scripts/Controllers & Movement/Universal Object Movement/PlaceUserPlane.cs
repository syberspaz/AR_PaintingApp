using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceUserPlane : MonoBehaviour
{

    [SerializeField]
    private RawImage GizmoPannel;

    [SerializeField]
    private Texture[] GizmoTextures;


    private bool startedPlacing = false;
    private bool progressFlagPlace = false;
    private bool progressFlagRotate = false;
    private bool progressFlagScale = false;

    private bool RotationSnapFlag = false;

    private bool canSnapRotate = true;


    // Update is called once per frame
    void Update()
    {


        DetectTouchMovement.Calculate();
        Touch touch = Input.GetTouch(0);


        float pinchAmount = 0f;
        if (progressFlagPlace)
        {
            GizmoPannel.texture = GizmoTextures[2];

            if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
            { // zoom

                pinchAmount = DetectTouchMovement.pinchDistanceDelta;

                Matrix4x4 viewMatrix;

                viewMatrix = Matrix4x4.Inverse(Camera.main.transform.localToWorldMatrix);

                Vector3 forward;
                forward.x = viewMatrix.m20;
                forward.y = viewMatrix.m21;
                forward.z = viewMatrix.m22;

                transform.position += forward * pinchAmount/2 * Time.deltaTime;
            }

            transform.parent = Camera.main.transform;

        }
        else
        {
            transform.parent = null; //when not placing unparent
        }
        if (progressFlagRotate)
        {
            GizmoPannel.texture = GizmoTextures[0];

            if (Input.touchCount >= 2 && canSnapRotate)
            {


                if (RotationSnapFlag)
                {

                    transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                RotationSnapFlag = !RotationSnapFlag;
                canSnapRotate = false;
            }

           

            //test code for rotation
            Vector2 touchMovement = touch.deltaPosition;

            Quaternion rotation = Quaternion.Euler(touchMovement.y * 0.1f, 0, touchMovement.x * 0.1f);



            transform.rotation = rotation * transform.rotation;
        }
        if (progressFlagScale)
        {
            GizmoPannel.texture = GizmoTextures[1];

            if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
            { // zoom

                pinchAmount = DetectTouchMovement.pinchDistanceDelta;

                transform.localScale += (new Vector3(pinchAmount, pinchAmount, pinchAmount) / 3) * Time.deltaTime;
            }
        }

        if (touch.phase == TouchPhase.Ended)
        {
            canSnapRotate = true;
        }

    }

    public void PlacePlane()
    {
        gameObject.SetActive(true);
    }

    public void DestroyPlane()
    {
        gameObject.SetActive(false);
    }

    public void StartSequence()
    {
        startedPlacing = true;
        gameObject.transform.parent = Camera.main.transform;
        gameObject.SetActive(true);
        gameObject.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 2);
        progressFlagPlace = false;
        progressFlagRotate = true;
        progressFlagScale = true;
}

    //these exist to confirm progress using the ui buttons
    public void SetPlace()
    {
        gameObject.transform.parent = null;
        progressFlagPlace = true;
        progressFlagRotate = false;
    }

    public void SetRotation()
    {
        progressFlagRotate = true;
        progressFlagScale = false;
    }

    public void SetScale()
    {
        progressFlagScale = true;
    }

    public void TogglePlace()
    {
        progressFlagPlace = true;
        progressFlagRotate = false;
        progressFlagScale = false;


    }
    public void ToggleRotation()
    {
        progressFlagPlace = false;
        progressFlagRotate = true;
        progressFlagScale = false;
    }
    public void ToggleScale()
    {
        progressFlagPlace = false;
        progressFlagRotate = false;
        progressFlagScale = true;
    }

    public void EndSequence()
    {
        progressFlagPlace = false;
        progressFlagRotate = false;
        progressFlagScale = false;
    }

    public void ResetPositionOnStart()
    {
        transform.parent = Camera.main.transform;
        transform.localPosition = new Vector3(0.039f, -0.135f, 1.66f);
        transform.localRotation =  Quaternion.Euler(new Vector3(53.7f, 0f, 0f));
        transform.localScale = new Vector3(1f, 0.59f, 1f);
        transform.parent = null;
    }

}
