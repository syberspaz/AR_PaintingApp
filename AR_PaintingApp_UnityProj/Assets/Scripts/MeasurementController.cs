//This temporary script was taken from:
//https://youtu.be/vZalV7--_uA
//Locking made by Noah Glassford
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]
public class MeasurementController : MonoBehaviour
{
    [SerializeField]
    private GameObject measurementPointPrefab;

    [SerializeField]
    private Vector3 offsetMeasurement = Vector3.zero;

    [SerializeField]
    private Text DistanceText;

    [SerializeField]
    private Text ButtonLockText;

    [SerializeField]
    private ARCameraManager arCameraManager;

    [SerializeField]
    private Camera camera;

    private LineRenderer measureLine;

    private ARRaycastManager arRaycastManager;

    private GameObject startPoint;

    private GameObject endPoint;

    private Vector2 touchPosition = default;

    private bool Lock;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        startPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);
        endPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);

        measureLine = GetComponent<LineRenderer>();

      


    }



    private void OnEnable()
    {
        if (measurementPointPrefab == null)
        {
            Debug.LogError("measurementPointPrefab must be set");
            enabled = false;
        }

    }

    void Update()
    {
        if (!Lock)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touchPosition = touch.position;

                    if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                    {
                        startPoint.SetActive(true);

                        Pose hitPose = hits[0].pose;
                        startPoint.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                    }
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    touchPosition = touch.position;

                    if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                    {
                        measureLine.gameObject.SetActive(true);
                        endPoint.SetActive(true);

                        Pose hitPose = hits[0].pose;
                        endPoint.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                    }
                }
            }

            if (startPoint.activeSelf && endPoint.activeSelf)
            {
                //distanceText.transform.position = endPoint.transform.position + offsetMeasurement;
                //distanceText.transform.rotation = endPoint.transform.rotation;
                measureLine.SetPosition(0, startPoint.transform.position);
                measureLine.SetPosition(1, endPoint.transform.position);

                DistanceText.text = $"Distance: " + (Vector3.Distance(startPoint.transform.position, endPoint.transform.position)).ToString();
            }
        }
        else if (Lock)
        {
            if (startPoint.activeSelf && endPoint.activeSelf)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Moved)
                {

                    Vector3 Movement = new Vector3(0, 0, 0);
                    
                    //get the values for camera up (Taken from the view matrix)
                    Vector3 up;
                    up.x = camera.worldToCameraMatrix.m10;
                    up.y = camera.worldToCameraMatrix.m11;
                    up.z = camera.worldToCameraMatrix.m12;

                    //get the values for camera forward (Taken from the view matrix)
                    Vector3 forward;
                    forward.x = camera.worldToCameraMatrix.m20;
                    forward.y = camera.worldToCameraMatrix.m21;
                    forward.z = camera.worldToCameraMatrix.m22;



                    Vector3 sideways = Vector3.Cross(up, camera.transform.forward);
                    Vector2 touchInput = touch.deltaPosition;

                    touchInput.x = touchInput.x / Screen.width;
                    touchInput.y = touchInput.y / Screen.height;

                    Movement += up * touchInput.y;
                    Movement += sideways * touchInput.x;

                    DistanceText.text = Movement.ToString();

                   Vector3 newStartPointPosition = startPoint.transform.position;
                    Vector3 newEndPointPosition = endPoint.transform.position;

                    newStartPointPosition += Movement;
                    newEndPointPosition += Movement;

                    startPoint.transform.SetPositionAndRotation(newStartPointPosition, Quaternion.identity);
                    endPoint.transform.SetPositionAndRotation(newEndPointPosition, Quaternion.identity);

                }
            }
        }
    }

    public void ToggleLock()
    {
        Lock = !Lock;

        if (Lock)
        {
            ButtonLockText.text = "Unlock Caliper";
        }
        else if (!Lock)
        {
            ButtonLockText.text = "Lock Caliper";
        }
    }

}