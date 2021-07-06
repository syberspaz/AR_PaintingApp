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
    private List<Outline> outlines;


    public float DragMovementSpeed;

    public float GyroMovementSpeed;

    public float pinchScalingSpeed;

    private float prevAccelerationY = 0;




  
    public void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    public void Update()
    {
        isSelected = true;

        viewMatrix = Matrix4x4.Inverse(cameraTransform.localToWorldMatrix);
        //Updates the values to what we set in the settings
        /*
        DragMovementSpeed = PlayerPrefs.GetFloat("DragMovementSpeed");
        GyroMovementSpeed = PlayerPrefs.GetFloat("GyroMovementSpeed");
        pinchScalingSpeed = PlayerPrefs.GetFloat("PinchScaleSpeed");
        */

        float pinchAmount = 0f;
        Quaternion desiredRotation = transform.rotation;

        DetectTouchMovement.Calculate();

        Input.gyro.enabled = gyroEnabled;

        Vector3 rotationRate = Input.gyro.rotationRateUnbiased;

        Quaternion phoneRotation = Quaternion.Euler(rotationRate);

       // text.text = phoneRotation.eulerAngles + " " + cameraTransform.rotation.eulerAngles;

        Touch touch = Input.GetTouch(0);

        if (isSelected)
        {
            if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
            { // zoom
                pinchAmount = DetectTouchMovement.pinchDistanceDelta;
            }

            if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
            { // rotate
                Vector3 rotationDeg = Vector3.zero;
                rotationDeg = (transform.position - cameraTransform.position).normalized;

                rotationDeg *= -DetectTouchMovement.turnAngleDelta;

               // rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
                desiredRotation *= Quaternion.Euler(rotationDeg);
            }


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

                Vector3 localScale = transform.localScale;

                localScale.x += (pinchAmount / 500) * pinchScalingSpeed;
                localScale.y += (pinchAmount / 500) * pinchScalingSpeed;
                localScale.z += (pinchAmount / 500) * pinchScalingSpeed;


                transform.position += Movement;
                transform.rotation = desiredRotation;
                transform.localScale = localScale;


            }
            else if (touch.phase == TouchPhase.Stationary)
            {
              

                //First get vector between camera and object
                Vector3 camToObject;
                camToObject = transform.position - cameraTransform.position;

                camToObject.Normalize();

                float gyroMovement;

                //This takes the rotation from the phone and the camera and averages the 2 of them to get a more accurate value
                if (Input.gyro.enabled)
                {
                    gyroMovement = cameraTransform.rotation.x;
                    gyroMovement += phoneRotation.x;
                    gyroMovement = gyroMovement / 2;
                }
                else //if the gyroscope somehow gets disabled, fallback to just camera rotation
                {
                    gyroMovement = cameraTransform.rotation.x;
                }

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
         ///   for (int i = 0; i < outlines.Count; i++)
           // {
        //        outlines[i].enabled = true;
        //    }
        }
        else
        {
           // for (int i = 0; i < outlines.Count; i++)
           // {
          //      outlines[i].enabled = false;
           // }
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
