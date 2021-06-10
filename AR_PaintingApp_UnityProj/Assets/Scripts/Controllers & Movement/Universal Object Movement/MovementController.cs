using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MovementController : MonoBehaviour
{
    public Transform cameraTransform;
    public bool isSelected;
    public bool gyroEnabled;
    public Matrix4x4 viewMatrix;

    [Tooltip("Drag in all renderers that have a edge material here.")]
    [SerializeField]
    private List<Renderer> renderers;

    [SerializeField]
    private float DragMovementSpeed;
    [SerializeField]
    private float GyroMovementSpeed;

    private float prevAccelerationY = 0;

    //debug
    [SerializeField]
    private Text text;

    public void Update()
    {
        Input.gyro.enabled = gyroEnabled;

        Touch touch = Input.GetTouch(0);

        if (isSelected)
        {
            //if the finger is moving, we are trying to move it along x/y relative to user, no gyro
            if (touch.phase == TouchPhase.Moved)
            {
                //get the values for camera up (Taken from the view matrix)
                Vector3 up;
                up.x = viewMatrix.m10;
                up.y = viewMatrix.m11;
                up.z = viewMatrix.m12;

                //get the values for camera forward (Taken from the view matrix)
                Vector3 forward;
                forward.x = viewMatrix.m20;
                forward.y = viewMatrix.m21;
                forward.z = viewMatrix.m22;

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

                Movement = Movement * DragMovementSpeed;

                transform.position += Movement;

                

            }
            else if (touch.phase == TouchPhase.Stationary)
            {
              

                //First get vector between camera and object
                Vector3 camToObject;
                camToObject = transform.position - cameraTransform.position;

                camToObject.Normalize();

                float gyroMovement;

                gyroMovement = cameraTransform.rotation.x;


                float GyroMovementDelta;

                GyroMovementDelta = gyroMovement - prevAccelerationY;
                if (GyroMovementDelta > 0.0f )
                {
                    camToObject *= (GyroMovementSpeed + GyroMovementDelta * GyroMovementSpeed);
                    transform.position = transform.position + camToObject;
                }
                else
                {
                    camToObject *= (GyroMovementSpeed + GyroMovementDelta * GyroMovementSpeed);
                    transform.position = transform.position - camToObject;
                }

               

                prevAccelerationY = gyroMovement;

            }
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].material.SetFloat("EdgeOpacity", 0.8f);
            }
        }
        else
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].material.SetFloat("EdgeOpacity", 0.0f);
            }
        }


    }


    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;


    }

}
