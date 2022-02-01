//This temporary script was taken from:
//https://youtu.be/vZalV7--_uA

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Used for a measuring tape type caliper, currently not being used but could very well be added back
public class MeasurementController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject LeftTipOfCaliper;
    [SerializeField]
    private GameObject RightTipOfCaliper;
    [SerializeField]
    private GameObject Caliper;





    [Tooltip("Gameobject for reticle")]
    [SerializeField]
    private GameObject reticleObject;

    //Stores the instantiated objects used for measuring
    [SerializeField]
    private GameObject startPoint;
    [SerializeField]
    private GameObject endPoint;

    private bool isLocked;

    private bool visualEnabled = false;

    private bool isPlacing = false;

    private bool firstPoint = true;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Text text;

    public void StartPlacing()
    {
        isPlacing = true;
    }

    public void ToggleLock()
    {
        isLocked = !isLocked;
    }

    public void ToggleEnabled()
    {
        visualEnabled = !visualEnabled;
        startPoint.SetActive(visualEnabled);
        endPoint.SetActive(visualEnabled);
    }

    public void PlaceStartPoint()
    {
        //makes sure the point is active before attempting to move it
        if (startPoint.activeSelf)
        {
            startPoint.transform.position = reticleObject.transform.position;
        }
        else
        {
            startPoint.SetActive(true);
            startPoint.transform.position = reticleObject.transform.position;

        }
    }

    public void PlaceEndPoint()
    {
        if (endPoint.activeSelf)
        {
            endPoint.transform.position = reticleObject.transform.position;
        }
        else
        {
            endPoint.SetActive(true);
            endPoint.transform.position = reticleObject.transform.position;

        }
    }
    //need to be outside the loop
    bool animSwitch = false;
    float animationFloat = 0;

    public void Update()
    {
        if (isLocked)
        {
            Vector3 up;
            up.x = camera.worldToCameraMatrix.m10;
            up.y = camera.worldToCameraMatrix.m11;
            up.z = camera.worldToCameraMatrix.m12;

            //get the values for camera forward (Taken from the view matrix)
            Vector3 forward;
            forward.x = camera.worldToCameraMatrix.m20;
            forward.y = camera.worldToCameraMatrix.m21;
            forward.z = camera.worldToCameraMatrix.m22;

            Vector3 sideways = Vector3.Cross(up, forward);

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchInput = touch.deltaPosition;

                touchInput.x = -touchInput.x / Screen.width;
                touchInput.y = touchInput.y / Screen.height;

                Vector3 Movement = new Vector3(0, 0, 0);

                Movement += up * touchInput.y;
                Movement += sideways * touchInput.x;


                startPoint.transform.position += Movement;
                endPoint.transform.position += Movement;
            }
        }

        if (isPlacing && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (firstPoint)
            {
                PlaceStartPoint();
                firstPoint = false;
            }
            else
            {
                PlaceEndPoint();
                firstPoint = true;

                isPlacing = false;
            }


        }
    }

    //For future use
    void MoveWithDrag()
    {
        /*
       
        */
    }



}