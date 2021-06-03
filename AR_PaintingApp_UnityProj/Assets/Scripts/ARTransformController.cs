using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARTransformController : MonoBehaviour
{

    //Used for its transform, for both the lock type movement and drag type movement
    [Tooltip("Use the main AR camera transform here")]
    [SerializeField]
    private Camera cameraTransform;

    [Tooltip("Scene should have a reticle object, place its transform here")]
    [SerializeField]
    private Transform reticleTransform;

    public int activeMovementType = 1;

   public void CameraLockMove()
    {

        //Parents object to camera, so all movements in physical space to the phone also happen in AR space to the object
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Stationary)
        {
            transform.parent = cameraTransform.transform;
        }
        else
        {
            transform.parent = null;
        }
      
   }

    public void TouchScreenDragMove()
    {
        Touch touch = Input.GetTouch(0);


        //get the values for camera up (Taken from the view matrix)
        Vector3 up;
        up.x = cameraTransform.worldToCameraMatrix.m10;
        up.y = cameraTransform.worldToCameraMatrix.m11;
        up.z = cameraTransform.worldToCameraMatrix.m12;

        //get the values for camera forward (Taken from the view matrix)
        Vector3 forward;
        forward.x = cameraTransform.worldToCameraMatrix.m20;
        forward.y = cameraTransform.worldToCameraMatrix.m21;
        forward.z = cameraTransform.worldToCameraMatrix.m22;

        //Cross returns a vector that is perpendicular to both vectors
        Vector3 sideways = Vector3.Cross(up, forward);

        //The delta position is the main thing used to move it around
        Vector2 touchInput = touch.deltaPosition;

        touchInput.x = touchInput.x / Screen.width;
        touchInput.y = touchInput.y / Screen.height;

       

        Vector3 Movement = new Vector3(0, 0, 0);

        //By multiplying by the up and sideways vectors, we can move in 3d space based on the orientation of the camera
        Movement += up * touchInput.y;
        Movement -= sideways * touchInput.x;


        transform.position += Movement;

    }

    public void ReticleMove()
    {
        //Just sets the position and rotation to match the reticle

        transform.position = reticleTransform.position;
        transform.rotation = reticleTransform.rotation;
    }

    public void Update()
    {
        //mostly self explanatory, uses activeMovementType (controlled by slider and ARMovementControllerManger script)
        //to decide which movement function to use
        if (activeMovementType == 0)
        {
            CameraLockMove();
        }
        else if(activeMovementType == 1)
        {
            ReticleMove();
        }
        else if (activeMovementType == 2)
        {
            TouchScreenDragMove();
        }
    }
}
