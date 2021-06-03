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

    public int activeMovementType;

   public void CameraLockMove()
    {
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

        Vector3 up;
        up.x = cameraTransform.worldToCameraMatrix.m10;
        up.y = cameraTransform.worldToCameraMatrix.m11;
        up.z = cameraTransform.worldToCameraMatrix.m12;

        //get the values for camera forward (Taken from the view matrix)
        Vector3 forward;
        forward.x = cameraTransform.worldToCameraMatrix.m20;
        forward.y = cameraTransform.worldToCameraMatrix.m21;
        forward.z = cameraTransform.worldToCameraMatrix.m22;

        Vector3 sideways = Vector3.Cross(up, forward);

        Vector2 touchInput = touch.deltaPosition;

        touchInput.x = touchInput.x / Screen.width;
        touchInput.y = touchInput.y / Screen.height;

       

        Vector3 Movement = new Vector3(0, 0, 0);

        Movement += up * touchInput.y;
        Movement += sideways * touchInput.x;


        transform.position += Movement;

    }

    public void ReticleMove()
    {
        transform.position = reticleTransform.position;
        transform.rotation = reticleTransform.rotation;
    }

    public void Update()
    {
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
