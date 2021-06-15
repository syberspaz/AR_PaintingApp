using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class TrackingStateManager : MonoBehaviour
{
    [SerializeField]
    private ARSession arSession;

    [SerializeField]
    private GameObject warningUIPannel; //to control the activeness of the ui pannel

    [SerializeField]
    private Text warningUIText;

    [SerializeField]
    private Text helpUIText;

    public void Update()
    {

        var xrSessionSubsystem = arSession.subsystem;
        TrackingState trackingState = xrSessionSubsystem.trackingState;
        NotTrackingReason notTrackingReason = xrSessionSubsystem.notTrackingReason;

        if (trackingState == TrackingState.None)
        {
            warningUIPannel.SetActive(true); //if there is no tracking, enable the ui pannel to alert user
            warningUIText.text = "Tracking Lost! Reason: " + notTrackingReason.ToString();

            //using the notTrackingReason, we can give a fairly good suggestion to the user on how to fix the tracking issues
            if (notTrackingReason == NotTrackingReason.Initializing || notTrackingReason == NotTrackingReason.Relocalizing)
            {
                helpUIText.text = "Simply wait until this message dissapears, the tracking is still starting up or recalculating.";
            }

            if (notTrackingReason == NotTrackingReason.InsufficientLight)
            {
                helpUIText.text = "There is insufficient light. Make sure your camera isn't covered and try to get more light in your room.";
            }

            if (notTrackingReason == NotTrackingReason.InsufficientFeatures)
            {
                helpUIText.text = "There are not enough features in your room to track properly. First try slowly moving your phone from side to side, and" +
                    "if that doesn't work, try placing some more objects in your room to track.";
            }
            if (notTrackingReason == NotTrackingReason.ExcessiveMotion)
            {
                helpUIText.text = "You moved your phone too quickly. Slowly move it from side to side until tracking is regained.";
            }
            if(notTrackingReason == NotTrackingReason.Unsupported)
            {
                helpUIText.text = "Your device doesn't give a reason the tracking stopped. Try moving your phone slowly from side to side to help it regain" +
                    "tracking";
            }
            if (notTrackingReason == NotTrackingReason.CameraUnavailable)
            {
                helpUIText.text = "The camera is unavailable. Make sure this app has permissions to use the camera (device settings)";
            }

        }

        if (trackingState == TrackingState.Limited)
        {
            warningUIPannel.SetActive(true); //if there is limited tracking, enable the ui pannel to alert user
            warningUIText.text = "Tracking Limited! Reason: " + notTrackingReason.ToString();

            //using the notTrackingReason, we can give a fairly good suggestion to the user on how to fix the tracking issues
            if (notTrackingReason == NotTrackingReason.Initializing || notTrackingReason == NotTrackingReason.Relocalizing)
            {
                helpUIText.text = "Simply wait until this message dissapears, the tracking is still starting up or recalculating.";
            }

            if (notTrackingReason == NotTrackingReason.InsufficientLight)
            {
                helpUIText.text = "There is insufficient light. Make sure your camera isn't covered and try to get more light in your room.";
            }

            if (notTrackingReason == NotTrackingReason.InsufficientFeatures)
            {
                helpUIText.text = "There are not enough features in your room to track properly. First try slowly moving your phone from side to side, and" +
                    "if that doesn't work, try placing some more objects in your room to track.";
            }
            if (notTrackingReason == NotTrackingReason.ExcessiveMotion)
            {
                helpUIText.text = "You moved your phone too quickly. Slowly move it from side to side until tracking is regained.";
            }
            if (notTrackingReason == NotTrackingReason.Unsupported)
            {
                helpUIText.text = "Your device doesn't give a reason the tracking stopped. Try moving your phone slowly from side to side to help it regain" +
                    "tracking";
            }
            if (notTrackingReason == NotTrackingReason.CameraUnavailable)
            {
                helpUIText.text = "The camera is unavailable. Make sure this app has permissions to use the camera (device settings)";
            }

        }

        if (trackingState == TrackingState.Tracking)
        {
            warningUIPannel.SetActive(false); //no issues with tracking, hide the warnings
        }

        
    }
}
