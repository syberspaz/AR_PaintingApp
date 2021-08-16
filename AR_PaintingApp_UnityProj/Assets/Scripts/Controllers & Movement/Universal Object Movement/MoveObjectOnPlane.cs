using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectOnPlane : MonoBehaviour
{

    public bool isEnabled;

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


        if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
        { // rotate
            Quaternion desiredRotation = transform.rotation;
            Vector3 rotationDeg = Vector3.zero;
            rotationDeg = GameObject.FindGameObjectWithTag("UserToolPlane").transform.up;

            rotationDeg *= -DetectTouchMovement.turnAngleDelta;

            // rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
            desiredRotation *= Quaternion.Euler(rotationDeg);

            transform.localRotation = desiredRotation;
        }

        if (Physics.Raycast(cam.ScreenPointToRay(touch.position), out hit))
        {
            if(hit.transform.gameObject.tag == "UserToolPlane")
            {
                transform.position = hit.point;
                transform.rotation = hit.transform.rotation;
            }
        }
        else
        {
            return;
        }
    }
}
