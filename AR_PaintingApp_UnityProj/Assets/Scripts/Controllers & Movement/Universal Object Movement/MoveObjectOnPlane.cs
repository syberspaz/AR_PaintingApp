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

    public Text debugText;

    public bool isFreeForm; //Feature point tracking mostly just for caliper, refering to it as freeform

    public int FreeFormRadius;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = Camera.main;
        Touch touch = Input.GetTouch(0);
        RaycastHit hit;

        DetectTouchMovement.Calculate();



            if (Input.touchCount > 1) //2 touches, don't want to raycast the plane
            {
                GameObject.FindGameObjectWithTag("UserToolPlane").layer = 2;
            }
            else
            {
                GameObject.FindGameObjectWithTag("UserToolPlane").layer = 0;
            }




            if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
            { // rotate
                Quaternion desiredRotation = transform.rotation;
                Vector3 rotationDeg = new Vector3(0, 0, 1);


                rotationDeg *= -DetectTouchMovement.turnAngleDelta;

                // rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
                desiredRotation *= Quaternion.Euler(rotationDeg);

                transform.localRotation = desiredRotation;
            }


        if (isFreeForm)
        {

            for (int i = -FreeFormRadius; i < FreeFormRadius; i++)
            {
                Vector2 newPos = touch.position;
                newPos.x += i;
                newPos.y += i;

                var ray = Camera.main.ScreenPointToRay(newPos);
                var hits = new List<ARRaycastHit>();
                var hasHit = raycastManager.Raycast(ray, hits, trackableTypeMask);

                if (hasHit)
                {
                    var pose = hits[0].pose;
                    transform.position = pose.position;
                    transform.rotation = pose.rotation;

                    debugText.text = "Hit point";

                    return;
                }

            }


        }
        else
        {


            if (Physics.Raycast(cam.ScreenPointToRay(touch.position), out hit))
            {
                if (hit.transform.gameObject.tag == "UserToolPlane")
                {
                    transform.position = hit.point;
                    transform.rotation = hit.transform.rotation;
                }
            }

        }



    }
}
