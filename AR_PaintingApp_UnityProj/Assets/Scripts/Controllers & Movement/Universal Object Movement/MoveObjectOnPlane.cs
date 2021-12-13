using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class MoveObjectOnPlane : MonoBehaviour
{

    public bool isEnabled;

    
    [SerializeField] ARRaycastManager raycastManager = null;
    [SerializeField] ARSessionOrigin origin = null;
    [SerializeField] TrackableType trackableTypeMask = TrackableType.FeaturePoint;



    public bool isFreeForm; //Feature point tracking mostly just for caliper, refering to it as freeform

    public int FreeFormRadius;




 

    // Update is called once per frame
    void Update()
    {
        Camera cam = Camera.main;
        Touch touch = Input.GetTouch(0);
        RaycastHit hit;

        DetectTouchMovement.Calculate();

        /*
        if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0) //if 2 or 0 touches, don't rotate since user is most likely trying to pinch
        { // rotate
            Quaternion desiredRotation = transform.rotation;
            Vector3 rotationDeg = new Vector3(0, 0, 1);


            rotationDeg *= -DetectTouchMovement.pinchDistanceDelta;

            rotationDeg *= Time.deltaTime;

            // rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
            desiredRotation *= Quaternion.Euler(rotationDeg);

            transform.localRotation = desiredRotation;
        }
        */

        if (Input.touchCount < 2)
        {
            float rotationAmount = touch.deltaPosition.x;
            rotationAmount *= Time.deltaTime;


            Quaternion desiredRotation = transform.rotation;
            Vector3 rotationDeg = new Vector3(0, 0, 1);

            rotationDeg *= rotationAmount;
            desiredRotation *= Quaternion.Euler(rotationDeg);


            transform.localRotation = desiredRotation;


        }


        if (Physics.Raycast(cam.ScreenPointToRay(touch.position), out hit))
        {
            if (hit.transform.gameObject.tag == "UserToolPlane")
            {
                transform.position = hit.point;

                Vector3 rot = transform.rotation.eulerAngles;

                rot.x = hit.transform.rotation.eulerAngles.x;
               // rot.y = hit.transform.rotation.eulerAngles.y;

                transform.rotation = Quaternion.Euler(rot);

                
          
            }
        }


      


           
    



    }
}
