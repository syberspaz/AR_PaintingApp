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

    //Debug
    [SerializeField]
    private Text debugText;
    [SerializeField]
    private Text debugText2;
    public void Update()
    {
        var xrSessionSubsystem = arSession.subsystem;
        TrackingState trackingState = xrSessionSubsystem.trackingState;
        NotTrackingReason notTrackingReason = xrSessionSubsystem.notTrackingReason;
        debugText.text = trackingState.ToString();
        debugText2.text = notTrackingReason.ToString();
        
    }
}
