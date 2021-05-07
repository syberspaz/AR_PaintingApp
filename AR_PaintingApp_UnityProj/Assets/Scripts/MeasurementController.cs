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
                    //gets the deltaPosition for the touch, adds that to the start and end points
                    //Divide by screen width is to avoid getting deltaPositions in the hundreds
                   float newX = startPoint.transform.localPosition.x + touch.deltaPosition.x/Screen.width;
                   float newY = startPoint.transform.localPosition.y;
                   float newZ = startPoint.transform.localPosition.z + touch.deltaPosition.y/Screen.height;

                   float newXEnd = endPoint.transform.localPosition.x + touch.deltaPosition.x / Screen.width;
                    float newYEnd = endPoint.transform.localPosition.y;
                   float newZEnd = endPoint.transform.localPosition.z + touch.deltaPosition.y / Screen.height;

                    startPoint.transform.SetPositionAndRotation(new Vector3(newX, newY, newZ), Quaternion.identity);
                    endPoint.transform.SetPositionAndRotation(new Vector3(newXEnd, newYEnd, newZEnd), Quaternion.identity);

                    measureLine.SetPosition(0, startPoint.transform.position);
                    measureLine.SetPosition(1, endPoint.transform.position);

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